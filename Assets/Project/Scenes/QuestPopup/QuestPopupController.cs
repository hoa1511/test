using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.View;

public class QuestPopupController : Controller
{
    public const string QUESTPOPUP_SCENE_NAME = "QuestPopup";

    [SerializeField] private Transform dailyQuestView, achievementView;

    public override string SceneName()
    {
        return QUESTPOPUP_SCENE_NAME;
    }

    public override void OnActive(object data)
    {
        base.OnActive(data);

        FechDailyQuest();
        FetchAchievement();
    }

    public void FechDailyQuest()
    {
        foreach(Transform child in dailyQuestView.transform)
        {
            Destroy(child.gameObject);
        }

        QuestManager.Instance.DailyQuestController.LoadQuestFromJsonToView(dailyQuestView);
    }

    public void FetchAchievement()
    {
        foreach (Transform child in achievementView.transform)
        {
            Destroy(child.gameObject);
        }

        QuestManager.Instance.AchievementController.LoadAchievementToView(achievementView);
    }

    public void Close()
    {
        Manager.Close();
    }
}