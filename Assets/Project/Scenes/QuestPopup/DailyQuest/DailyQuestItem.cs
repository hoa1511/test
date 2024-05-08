using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SS.View;

[System.Serializable]
public class DailyQuestItem : MonoBehaviour
{
    [SerializeField] private TMP_Text title, rewardText, progressText;
    [SerializeField] private GameObject rewardHolder, rewardedHolder, progressHolder;
    [SerializeField] private Image icon;
    [SerializeField] private Transform startPos;

    private QuestType questType;
    private int numberToComplete;
    private int rewardNumber;
    private int currentNumber = 0;
    private bool isClaim = false;

    public QuestType QuestType { get => questType; set => questType = value; }
    public int NumberToComplete { get => numberToComplete; set => numberToComplete = value; }
    public int RewardNumber { get => rewardNumber; set => rewardNumber = value; }
    public int CurrentNumber { get => currentNumber; set => currentNumber = value; }

    public string Title { get => title.text; }

    public bool IsClaim { get => isClaim; set => isClaim = value; }

    public void Setup(QuestType questType, int numberToComplete, int rewardNumber, string desc, bool isClaim, int currentNumber, Sprite sprite)
    {
        QuestType = questType;
        NumberToComplete = numberToComplete;
        RewardNumber = rewardNumber;
        IsClaim = isClaim;
        CurrentNumber = currentNumber;

        icon.sprite = sprite;
        progressText.text = currentNumber + "/" + numberToComplete;
        rewardText.text = "x"+rewardNumber;
        title.text = desc;

        if(isClaim)
        {
            progressHolder.SetActive(false);
            rewardedHolder.SetActive(true);
            rewardHolder.SetActive(false);
            return;
        }

        if(currentNumber == numberToComplete)
        {
            progressHolder.SetActive(false);
            rewardHolder.SetActive(true);
        } else
        {
            progressHolder.SetActive(true);
            rewardHolder.SetActive(false);
        }
    }

    public void Claim()
    {
        //Bank.Instance.Deposit(rewardNumber, Currency.Gem);
        if(!isClaim)
        {
            isClaim = true;
            StartCoroutine(QuestManager.Instance.StartSpawnGemImage(rewardNumber, startPos.position, questType));
        }
    }
}
