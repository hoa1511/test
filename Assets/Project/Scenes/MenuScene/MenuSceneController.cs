using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using SS.View;
using TMPro;
using UnityEngine.EventSystems;
using UI.Pagination;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Android;
using System.IO;

public class MenuSceneController : Controller
{
    public const string MENUSCENE_SCENE_NAME = "MenuScene";

    public override string SceneName()
    {
        return MENUSCENE_SCENE_NAME;
    }

    [Header("All Pages")]
        [SerializeField] private TextMeshProUGUI gemText;
        [SerializeField] private TextMeshProUGUI goldText;

        [SerializeField] private GameObject noAds;


    [Header("Hero Page")]
        [SerializeField] private GameObject[] objsOnOffHeroPage;

    [Header("Hero Page")]
        [SerializeField] private GameObject[] objsOnOffBattlePage;

    [Header("Paged React")]
        [SerializeField] private PagedRect pagedRect;
        [SerializeField] private Page pagePack;

    [SerializeField] private ScrollRect scrollPack;

    public TextMeshProUGUI GemText { get => gemText; set => gemText = value; }
    public TextMeshProUGUI GoldText { get => goldText; set => goldText = value; }

    [SerializeField] private Transform dailyQuestView, achievementView, posMoveGold, posMoveGem, parent;
 
    private void Awake() 
    {
        Time.timeScale = 1;

        Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        Permission.RequestUserPermission(Permission.ExternalStorageRead);

        if (!Directory.Exists(Application.persistentDataPath))
              Directory.CreateDirectory(Application.persistentDataPath);

        QuestManager.Instance.PosMoveGem = posMoveGem;
        QuestManager.Instance.PosMoveGold = posMoveGold;
        QuestManager.Instance.DailyQuestView = dailyQuestView;
        QuestManager.Instance.AchievementView = achievementView;
        QuestManager.Instance.Parent = parent;

    }

    private void Start() 
    {
        if(PlayerPrefs.GetInt("noAds") == 1)
        {
            noAds.SetActive(false);
        }
        if(CheckOpenDaily())
        {
            Manager.Add(RewardWeeksController.REWARDWEEKS_SCENE_NAME);
        }
     
        QuestManager.Instance.FechDailyQuest();
        QuestManager.Instance.FetchAchievement();
    }

    private void Update() 
    {
        goldText.text = FormatCurrency.Format(Bank.Instance.Gold);
        gemText.text = FormatCurrency.Format(Bank.Instance.Gem);
    }

    
    private bool CheckOpenDaily()
    {
        string lastTimeLogin = PlayerPrefs.GetString("lastTimeLogin");
        if(string.IsNullOrEmpty(lastTimeLogin))
        {
            PlayerPrefs.SetString("lastTimeLogin", DateTime.Now.ToString());
            return true;
        } else
        {
            DateTime curTime = DateTime.Now;
            DateTime lastTime = DateTime.Parse(lastTimeLogin);
            DateTime tommorowLastTime = lastTime.AddDays(1);

            if(curTime.Day == tommorowLastTime.Day)
            {
                if(curTime.Hour >= 8)
                {
                    PlayerPrefs.SetString("lastTimeLogin", DateTime.Now.ToString());
                    return true;
                }
            } else if(curTime.Day > tommorowLastTime.Day)
            {
                PlayerPrefs.SetString("lastTimeLogin", DateTime.Now.ToString());
                return true;
            }
        }
        return false;
    }

    public void OnOffHeroPage(bool enable)
    {
        for(int i = 0; i < objsOnOffHeroPage.Length; i++)
        {
            objsOnOffHeroPage[i].SetActive(enable);
        }
    }

    public void OnOffBattlePage(bool enable)
    {
        for(int i = 0; i < objsOnOffBattlePage.Length; i++)
        {
            objsOnOffBattlePage[i].SetActive(enable);
        }
    }

    public void OnBattle()
    {
        Manager.Load("L" + PlayerPrefs.GetInt("level"));
        //Manager.Load(MiniGameTreasureChestController.MINIGAMETREASURECHEST_SCENE_NAME);
        //Manager.Load(SM2Controller.SM2_SCENE_NAME);
    }

    public void OnSurvive()
    {
        Manager.Load(MapSurviveController.MAPSURVIVE_SCENE_NAME);
    }

    public void OnNoAds()
    {
        PlayerPrefs.SetInt("noAds", 1);
        noAds.SetActive(false);
    }

    public void ClickPlusCurrency(float index)
    {
        pagedRect.SetCurrentPage(pagePack);
        scrollPack.DOVerticalNormalizedPos(index, 0.25f);
    }

    public void OnSetting()
    {
        Manager.Add("SettingScene");
    }
}