using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SS.View;
using TMPro;
using DG.Tweening;


public class UISceneController : Controller
{
    [SerializeField] private GameObject tapToStartText;
    [SerializeField] private GameObject tapToStartButton;
    [SerializeField] private GameObject enemyCountUI;
    [SerializeField] private GameObject levelTextUI;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject gold;
    [SerializeField] private TextMeshProUGUI textGold, textGem;
    [SerializeField] private AudioClip coinSound;
    
    [SerializeField] GameObject backGround;
    [SerializeField] private Transform posMoveGold, posMoveGem;

    [SerializeField] private Slider sliderForce;

    CharacterHolder characterHolder;

    [Header("Active Skill Part")]
    [SerializeField] private GameObject skillButtonPrefab;
    [SerializeField] private Transform skillButtonsHolder;
    [SerializeField] private bool isInSurvivalMode = false;
    List<ActiveStats> activeStat = new List<ActiveStats>();


    private bool isTapSettingButton = false;

    private int phase = 0;

    public const string UISCENE_SCENE_NAME = "UIScene";

    public static UISceneController Instance;

    public GameObject GoldPrefab { get => gold; set => gold = value; }
    public Transform PosMoveGold { get => posMoveGold; set => posMoveGold = value; }
    public Transform PosMoveGem { get => posMoveGem; set => posMoveGem = value; }
    public AudioClip CoinSound { get => coinSound; set => coinSound = value; }

    private void Awake()
    {
        Instance = this;
        // TODO: Turn off shadows temporary, but it is not ideal to put it here. Move it to appropriate place.
        QualitySettings.shadows = ShadowQuality.Disable;
    }

    public override void OnActive(object data)
    {
        if(data != null)
        {
            isInSurvivalMode = (bool)data;
        }
    }

    private void Start()
    {
        levelTextUI.GetComponent<TextMeshProUGUI>().text = "Level " + PlayerPrefs.GetInt("displayLevel");
        characterHolder = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterHolder>();
        characterHolder.isWaiting = true;
        GameManager.Instance.isWaiting = true;

        sliderForce.maxValue = characterHolder.maxForce;
    }

    public void StartGame()
    {
        if(phase == 0)
        {
            SoundController.Instance.PlayButtonSound();

            backGround.GetComponent<Image>().color = new Color(255,255,255,0);
            tapToStartText.gameObject.SetActive(false);

            levelTextUI.gameObject.SetActive(false);
            enemyCountUI.gameObject.SetActive(true);
            backButton.gameObject.SetActive(false);
            restartButton.gameObject.SetActive(true);
            phase += 1;
        }

        else if(phase == 1)
        {
            characterHolder.isWaiting = false;
            PlayerPrefs.SetInt("hasPause", 0);
            GameManager.Instance.NormalTimeScale();
            tapToStartButton.gameObject.SetActive(false);
            GameManager.Instance.isWaiting = false;
            InstantiateSkillButton();
        }
    }

    public void SettingButton()
    {
        SoundController.Instance.PlayButtonSound();

        isTapSettingButton = true;
        GameManager.Instance.LoadSettingScene();
        characterHolder.isWaiting = true;
    }

    public void TutorialsButton()
    {
        SoundController.Instance.PlayButtonSound();

        GameManager.Instance.StartTutorialScene();
        characterHolder.isWaiting = true;
    }

    public override string SceneName()
    {
        return UISCENE_SCENE_NAME;
    }


    public void OnRestartButtonClick()
    {
        SoundController.Instance.PlayButtonSound();
        GameManager.Instance.ReloadLevel();
        if(isInSurvivalMode)
        {
            Manager.Load(SurvivalModeSceneController.SURVIVALMODESCENE_SCENE_NAME);
        }
        else
        {
            GameManager.Instance.ReloadLevel();
        }
    }

    public void OnBackMenu()
    {
        Manager.Load("MenuScene");
    }

    IEnumerator normalPlayer()
    {
        yield return new WaitForSeconds(0.5f);
        characterHolder.isWaiting = false;
    }

    private void Update()
    {
        textGold.text = FormatCurrency.Format(Bank.Instance.Gold);

        sliderForce.value = characterHolder.distanceFinger;
    }

    public void InstantiateSkillButton()
    {
        activeStat = CharacterStatController.Instance.ActiveStatList;

        foreach(ActiveStats stat in activeStat)
        {
            Button skillButton = Instantiate(skillButtonPrefab, skillButtonsHolder).GetComponent<Button>();
            skillButton.transform.localScale = Vector3.zero;
            skillButton.onClick.AddListener(delegate{ActiveStatController.Instance.PlaySkinSkill(stat);});
            skillButton.transform.DOScale(new Vector3(1.2f,1.2f,1.2f), 0.3f).OnComplete(()=>{
                 skillButton.transform.DOScale(new Vector3(1f,1f,1f), 0.2f);
            }).SetUpdate(true);
        }
    }
}