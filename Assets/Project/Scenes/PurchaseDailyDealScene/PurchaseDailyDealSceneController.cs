using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.View;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class PurchaseDailyDealSceneController : Controller
{
    public const string PURCHASEDAILYDEALSCENE_SCENE_NAME = "PurchaseDailyDealScene";
    private DailyDealItem dailyDealItem;

    [SerializeField] private Image itemFrame;
    [SerializeField] private Image itemSprite;
    [SerializeField] private TextMeshProUGUI itemQuantityText;
    [SerializeField] private TextMeshProUGUI itemPriceText;
    [SerializeField] private Button confirmButton;

    [SerializeField] private GameObject backGround;
    [SerializeField] private GameObject itemFrameGameObject;
    [SerializeField] private GameObject glowEffect;
    [SerializeField] private Button claimButton;

    public override string SceneName()
    {
        return PURCHASEDAILYDEALSCENE_SCENE_NAME;
    }

    public override void OnActive(object data)
    {
        if(data != null)
        {
            dailyDealItem = (DailyDealItem)data;
        }
        OnPurchaseSuccess();
    }

    private void Start()
    {
        itemFrame.sprite = dailyDealItem.itemFrame.sprite;
        itemSprite.sprite = dailyDealItem.dailyDealSprite.sprite;
        itemQuantityText.text = "x" + dailyDealItem.cardQuantity.ToString();
        itemPriceText.text = FormatCurrency.Format(dailyDealItem.amountOfCurrencyToUnlock);

        claimButton.onClick.AddListener(delegate{ClosePopUp();});
    }

    private void ClosePopUp()
    {
        Manager.Close();
    }

    public void OnPurchaseSuccess()
    {
        claimButton.gameObject.SetActive(true);
        glowEffect.gameObject.SetActive(true);
        itemFrame.gameObject.SetActive(true);
        itemFrameGameObject.transform.DOScale(new Vector3(1.4f, 1.4f, 1.4f), 0.5f).SetUpdate(true);
    }

    
}