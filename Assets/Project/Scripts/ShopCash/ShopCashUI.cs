using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class ShopCashUI : MonoBehaviour
{
    [SerializeField] private Sprite commonFrame, rareFrame, epicFrame, legendaryFrame;

    [Header ("Item In Daily Deal")]
        [SerializeField] private Transform group;

    [Header ("Slider")]
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI sliderText;

    [Header ("Open Chest")]
        [SerializeField] private GameObject openChest;
        [SerializeField] private GameObject UIOpen;

    [Header("Spawn Texts")]
        [SerializeField] private GameObject spawnTextPrefab;
        [SerializeField] private Transform spawnTextHolder;

    [Header("SaveItemDeal")]
        private List<DailyDealItem> dailyDealItems;

        public List<DailyDealItem> DailyDealItems { get => dailyDealItems; set => dailyDealItems = value;}

    [Header("Daily Deal Refresh")]
        [SerializeField] private TextMeshProUGUI timeText;

    private int luckyRoll = 0;
    private DateTime exitTime;
    private DateTime startTime;
    private CrateOpenerManager crateOpener;


    public static ShopCashUI Instance;
    private void Awake()
    {

        if(Instance == null)
        {
            Instance = this;
        }    
        else
        {
            Destroy(this);
        }
    }


    private void Start() 
    {
        crateOpener = openChest.GetComponent<CrateOpenerManager>();
    }

    private void Update()
    {
        DisplayTime(CountDownTimer.Instance.timeRemaining);

        if(Input.GetKey("s"))
        {
            SaveManager.instance.DailyDealItemsToJson(DailyDealItems);
        }
    }

    public void OnReset()
    {
        luckyRoll++;

        if(luckyRoll > 20)
        {
            luckyRoll = 1;
        }

        for (int i = 0; i < group.childCount; i++)
        {
            DailyDealItem daily = group.GetChild(i).GetChild(0).GetComponent<DailyDealItem>();
            if(luckyRoll == 20 && i == group.childCount - 1)
            {
                for (int j = 0; j < daily.outputRatios.Length; j++)
                {
                    if(j == daily.outputRatios.Length - 1)
                    {
                        daily.outputRatios[j] = 1;
                    }
                    else
                    {
                        daily.outputRatios[j] = 0;
                    }
                }
            }
            else if(luckyRoll != 20 && i == group.childCount - 1)
            {
                daily.outputRatios[5] = 0.05f;
                daily.outputRatios[4] = 0.05f;
                daily.outputRatios[3] = 0.1f;
                daily.outputRatios[2] = 0.4f;
                daily.outputRatios[1] = 0.4f;
            }
            daily.RefreshItem();
        }

        slider.value = luckyRoll;
        sliderText.text = luckyRoll + "/20";
    }

    public void UpdateNumberOfCardInfo()
    {
        for(int i = 0 ; i < group.childCount; i++)
        {
            group.GetChild(i).GetChild(0).GetComponent<DailyDealItem>().UpdateNumberOfCardInfo();
        }
    }
    
    void DisplayTime(float timeToDisplay)
    {
         timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60) % 60;
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float hours = Mathf.FloorToInt(timeToDisplay / 3600);

        timeText.text = string.Format("New Item Will Appear In: {0:00}:{1:00}:{2:00}",hours, minutes, seconds);
    }

    public void InstantiateText(string textToSpawn)
    {
        GameObject spawnText = Instantiate(spawnTextPrefab, spawnTextHolder);
        spawnText.GetComponent<TextMeshProUGUI>().text = textToSpawn;
        Destroy(spawnText,2f);
    }
}
