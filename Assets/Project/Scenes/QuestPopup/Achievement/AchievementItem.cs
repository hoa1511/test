using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SS.View;
using UnityEngine.Android;

public class AchievementItem : MonoBehaviour
{

    [SerializeField] private TMP_Text title, progressText, rewardedText;
    [SerializeField] private Image icon;
    [SerializeField] private GameObject rewardHolder, rewardedHolder, progressHolder;
    [SerializeField] private Transform startPos;

    private int index, rewardedGem;

    public void SetupItem(string title, int totalNumber, int finishNumber, int rewardedGem, Sprite icon, int index, int maxAchievement, int currentProgress)
    {
        this.rewardedGem = rewardedGem;
        this.icon.sprite = icon;
        this.title.text = title;
        progressText.text = totalNumber + "/" + finishNumber;
       
        rewardedText.text = "x"+rewardedGem;
        this.index = index;

        if(totalNumber >= finishNumber)
        {
            progressHolder.SetActive(false);
            rewardHolder.SetActive(true);
        }


        if(currentProgress > maxAchievement)
        {
            progressHolder.SetActive(false);
            rewardHolder.SetActive(false);
            rewardedHolder.SetActive(true);
        }

    }

    public void Claim()
    {
        StartCoroutine(QuestManager.Instance.StartSpawnGemImage(rewardedGem, startPos.position, index));
    }
}
