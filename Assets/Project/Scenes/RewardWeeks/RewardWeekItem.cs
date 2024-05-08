using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RewardWeekItem : MonoBehaviour
{
    [SerializeField] private RewardWeeksController rewardWeeksController;
    [SerializeField] private int day = 0, amountReward;
    [SerializeField] private TMP_Text textDay, focusText;
    [SerializeField] private GameObject focus, checkMarkHolder;
    [SerializeField] private Button btn;
    [SerializeField] private TypeRewardWeek typeRewardWeek;
    [SerializeField] private RewardWeekItem nextItem;

    public GameObject Focus { get => focus; set => focus = value; }
    public GameObject CheckMarkHolder { get => checkMarkHolder; set => checkMarkHolder = value; }

    private void Start()
    {
        textDay.text = "Day " + day;
        Focus.SetActive(false);
        if (day == rewardWeeksController.Day)
        {
            focusText.text = "Claim Now";
            Focus.SetActive(true);
            btn.interactable = true;
            btn.onClick.AddListener(delegate { OnClaim(typeRewardWeek, amountReward); });
            checkMarkHolder.SetActive(false);
        }
        else if(day < rewardWeeksController.Day)
        {
            btn.interactable = false;
            checkMarkHolder.SetActive(true);
        } else if(day > rewardWeeksController.Day)
        {
            btn.interactable = false;
        }

        //if (day == rewardWeeksController.NextDay)
        //{
        //    focus.SetActive(true);
        //}
        //else focus.SetActive(false);
    }

    private void OnClaim(TypeRewardWeek typeRewardWeek, int amount)
    {
        switch (typeRewardWeek)
        {
            case TypeRewardWeek.Gold:
                StartCoroutine(rewardWeeksController.PlayAnimationGetReward(transform, Currency.Gold, amount));
                break;
            case TypeRewardWeek.Gem:
                StartCoroutine(rewardWeeksController.PlayAnimationGetReward(transform, Currency.Gem, amount));
                break;
            case TypeRewardWeek.Chest:
                break;
            case TypeRewardWeek.SpecialChest:
                break;
        }
        focus.SetActive(false);
        rewardWeeksController.Day = rewardWeeksController.Day + 1 > 7 ? 1 : rewardWeeksController.Day + 1;
        PlayerPrefs.SetInt("rewardWeekDay", rewardWeeksController.Day);
        btn.interactable = false;
        checkMarkHolder.SetActive(true);
        rewardWeeksController.OnClaimItemFinish(nextItem);
    }
}
