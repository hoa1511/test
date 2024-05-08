using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SS.View;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class EndGameSceneController : Controller
{
    [Header("Display UI")]
        [SerializeField] TextMeshProUGUI numberOfCoinEarn, bonusCoinFromSkinText;
        [SerializeField] TextMeshProUGUI textGold, textGem;
        [SerializeField] TextMeshProUGUI bonnusBalanceText;
        [SerializeField] GameObject NoThankButton, AdsButton;
        [SerializeField] Transform startSpawnCoinPosition;
        [SerializeField] Transform parent;
        [SerializeField] Transform endSpawnCoinPosition, offAdsButton;
        [SerializeField] GameObject coinSpawnPrefab;

    [Header("Sound FX")]
        [SerializeField] AudioClip coinSound;

    [Header("Ads Bonnus Coin")]
        [SerializeField] AdsInitializerBonusCoin adsInitializerBonusCoin;
        [SerializeField] RewardedAdsButtonEndScene  rewardedAdsButtonEndScene;

    [Header("Ads Institutide")]
        [SerializeField] AdsInitializerEndScene adsInitializerEndScene;
        [SerializeField] InterstitialAdsButton interstitialAdsButton;
        //[SerializeField] InterstitialAdsButton interstitialAdsButton;

    [Header("Unlock Trident Progress")]
        [SerializeField] PlayerWeapon tridentSkinToUnlock;
        [SerializeField] private Image nextUnlockTridentFrame;
        [SerializeField] TextMeshProUGUI nextUnlockLevel;
        [SerializeField] private float startCheckPoint;
        [SerializeField] private float endCheckPoint;
        [SerializeField] private float checkPoint = 0;
        [SerializeField] private float diff;
        [SerializeField] private float fillAmount;
        [SerializeField] private Image fillImage;
        [SerializeField] private TextMeshProUGUI percentText;

        
    private int earnBalance;
    private int bonusBalance;
    private float skinBonusGold;

    private void Awake()
    {
        checkPoint = PlayerPrefs.GetFloat("CheckPoint");
        earnBalance = Bank.Instance.EarnBalance;
        earnBalance += 200;

        bonusBalance = earnBalance * 2 + 200;
        PlayerPrefs.SetInt("BonusBalance", bonusBalance);
       
        numberOfCoinEarn.text = "+" + earnBalance.ToString();
        bonnusBalanceText.text = "+" + bonusBalance.ToString();

        skinBonusGold = earnBalance * (CharacterStatController.Instance.GetCurrentSkinStat(PassiveStats.PercentGoldPerStage))/100;
        bonusCoinFromSkinText.text = skinBonusGold.ToString();
    }

    private void Start()
    {
        SoundController.Instance.PLayWinGameSound();

        GetNextUnlockTrident();
        FindCheckPoints();
        DisplayNextUnlockTridentInfo();

        adsInitializerBonusCoin.InitializeAds();
        rewardedAdsButtonEndScene.LoadAd();
        StartCoroutine(StartFill());
        StartCoroutine(StartSpawnCoinImage());

    }

    private void FindCheckPoints()
    {
        for(int i = 0; i < WeaponSkinArchive.Instance.WeaponSkinList.Count; i++)
        {
            if(WeaponSkinArchive.Instance.WeaponSkinList[i].isOwned == false)
            {
                endCheckPoint = WeaponSkinArchive.Instance.WeaponSkinList[i].levelToUnlock;
                startCheckPoint = WeaponSkinArchive.Instance.WeaponSkinList[i-1].levelToUnlock;
                diff = endCheckPoint - startCheckPoint;
                break;
            }
            else
            {
                endCheckPoint = 0;
                startCheckPoint = 0;
            }
        }

        checkPoint = (PlayerPrefs.GetInt("displayLevel") - startCheckPoint);

    }

    private IEnumerator StartFill()
    {
        float amountToFill = (1/diff)/30;
        fillImage.fillAmount = (checkPoint - 1)/diff;
        percentText.text = Mathf.Round(((checkPoint - 1)/diff)*100).ToString() + "%";

        for(int i = 0; i < 30; i++)
        {
            fillImage.fillAmount += amountToFill;
            percentText.text = Mathf.Round((fillImage.fillAmount)*100).ToString() + "%";
            yield return new WaitForSeconds(0.04f);
        }
    }

    private void Update()
    {
        numberOfCoinEarn.text = "+" + earnBalance.ToString();

        textGold.text = FormatCurrency.Format(Bank.Instance.Gold);
        textGem.text = FormatCurrency.Format(Bank.Instance.Gem);
    }

    private IEnumerator StartSpawnCoinImage()
    {
        yield return new WaitForSeconds(2);

        int loopNum = earnBalance / 50;

        for(int i = 0; i < loopNum ; i++)
        {
            GameObject coin = Instantiate(coinSpawnPrefab, startSpawnCoinPosition.position, Quaternion.identity, parent);
            coin.transform.localScale = new Vector3(1,1,1);
            earnBalance -= 50;
            if(coin != null)
            {
                coin.transform.DOMove(endSpawnCoinPosition.position, 0.25f).OnComplete(()=>{
                coin.GetComponent<AudioSource>().PlayOneShot(coinSound);
                Bank.Instance.Deposit(50, Currency.Gold);
                Destroy(coin.gameObject, 0.5f);
                });
                yield return new WaitForSeconds(0.05f);
            }
        }
        yield return new WaitForSeconds(0.05f);

        if(IsAbleToUnlockTrident(PlayerPrefs.GetInt("displayLevel")))
        {
            WeaponSkinArchive.Instance.UnlockSkin(tridentSkinToUnlock);
            StartCoroutine(OpenUnlockTridentScreen(tridentSkinToUnlock));
        }

        else
        {
            NoThankButton.gameObject.SetActive(true);
        }

        AdsButton.transform.DOMove(new Vector3(AdsButton.transform.position.x, AdsButton.transform.position.y, AdsButton.transform.position.z), 1.5f).OnComplete(() => {
            AdsButton.transform.DOMove(new Vector3(offAdsButton.position.x, AdsButton.transform.position.y, AdsButton.transform.position.z), 0.2f);
        });
    }

    public IEnumerator StrartSpawnCoinImage(int number)
    {
        int loopNum = number / 50;

        Bank.Instance.Deposit((int)skinBonusGold, Currency.Gold);
        for (int i = 0; i < loopNum; i++)
        {
            GameObject coin = Instantiate(coinSpawnPrefab, startSpawnCoinPosition.position, Quaternion.identity, parent);
            coin.transform.localScale = new Vector3(1, 1, 1);
            number -= 50;
            if (coin != null)
            {
                coin.transform.DOMove(endSpawnCoinPosition.position, 0.25f).OnComplete(() => {
                    coin.GetComponent<AudioSource>().PlayOneShot(coinSound);
                    Bank.Instance.Deposit(50, Currency.Gold);
                    
                    Destroy(coin, 0.5f);
                });
                yield return new WaitForSeconds(0.05f);
            }
        }
        yield return new WaitForSeconds(0.05f);
        PlayerPrefs.SetInt("BonusBalance", 0);
    }

    public const string ENDGAMESCENE_SCENE_NAME = "EndGameScene";

    public override string SceneName()
    {
        return ENDGAMESCENE_SCENE_NAME;
    }

    public void LoadNextLevel()
    {
        SoundController.Instance.PlayButtonSound();

        if(PlayerPrefs.GetInt("displayLevel") > 3 && AdManager.Instance.CanShowInterstitialAd() && PlayerPrefs.GetInt("noAds") != 1)
        {
            var levelEvent = new UnityEvent();
            levelEvent.AddListener(NextLevelListener);
            AdManager.Instance.ShowInterstitialAd(levelEvent);
        }
        else
        {
            GameManager.Instance.LoadNextLevel();
        }
    }

    private void NextLevelListener()
    {
        GameManager.Instance.LoadNextLevel();
    }

    IEnumerator OpenUnlockTridentScreen(PlayerWeapon playerWeapon)
    {
        yield return new WaitForSeconds(1);
        Manager.Add(UnlockTridentSceneController.UNLOCKTRIDENTSCENE_SCENE_NAME, playerWeapon);
    }

    private bool IsAbleToUnlockTrident(int currentLevel)
    {
        for(int i = 0; i < WeaponSkinArchive.Instance.WeaponSkinList.Count; i++)
        {
            if(tridentSkinToUnlock.levelToUnlock == currentLevel)
            {
                return true;
            }
        }
        return false;
    }

    private void GetNextUnlockTrident()
    {
        for(int i = 0; i < WeaponSkinArchive.Instance.WeaponSkinList.Count; i++)
        {
            if(WeaponSkinArchive.Instance.WeaponSkinList[i].isOwned == false)
            {
                tridentSkinToUnlock = WeaponSkinArchive.Instance.WeaponSkinList[i];
                break;
            }
        }
    }

    private void DisplayNextUnlockTridentInfo()
    {
        nextUnlockLevel.text = "Unlock At <color=green>Lv." + tridentSkinToUnlock.levelToUnlock + "</color>";
        nextUnlockTridentFrame.sprite = WeaponSkinArchive.Instance.GetItemFrameFromRarity(tridentSkinToUnlock.itemRarity);
    }
}