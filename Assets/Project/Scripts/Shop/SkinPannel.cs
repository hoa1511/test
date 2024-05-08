using System.Collections;
using SS.View;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkinPannel : MonoBehaviour
{
    [Header("Player Skin")]
        [SerializeField] private PlayerSkin playerSkin;

    [Header("Hero Skin Image")]
        [SerializeField] private Image skinImage;
        [SerializeField] private GameObject lockImage;

    [Header("Preview Button")]
        [SerializeField] private Button previewSkinButton;

    [Header("Claim Skin Button")]
        [SerializeField] private Button claimSkinButton;
        [SerializeField] private TextMeshProUGUI claimSkinbuttonText;
    
    [Header("Upgrade Skin Button")]
        [SerializeField] private Button upgradeSkinButton;
        [SerializeField] private TextMeshProUGUI upgradeSkinButtonText;
        [SerializeField] private GameObject upgradeImage;

    [Header("Skin Info Button")]
        [SerializeField] private Button skinInfoButton;

    [Header("Text Level")]
        [SerializeField] private TextMeshProUGUI skinLevelText;
    
    [Header("Equiped Text")]
        [SerializeField] private GameObject isEquipedText;

    private void Start()
    {
        LoadSkinInfo(playerSkin);
        SetUpButtons();
        SetUpSkinSprite();
    }

    private void Update()
    {
        ToggleisEquipedText(playerSkin);
        SetUpButtonText(playerSkin);
        SetUpUpgradeSkinButtonText(playerSkin);
    }

    private void SetUpSkinSprite()
    {
        if(playerSkin.isOwned == true)
        {
            lockImage.SetActive(false);
            skinImage.material.SetFloat("_GrayscaleAmount", 0);
        }
        
        else
        {
            lockImage.SetActive(true);
            lockImage.GetComponent<Image>().material.SetFloat("_GrayscaleAmount", 1);
        }
    }

    private void SetUpButtons()
    {
        skinInfoButton.onClick.AddListener(delegate{LoadSkinInfoPopup(playerSkin);});
        previewSkinButton.onClick.AddListener(delegate{InGameShopController.Instance.PreviewSkin(playerSkin);});
        claimSkinButton.onClick.AddListener(delegate{InGameShopController.Instance.PreviewSkin(playerSkin); OnSkinButtonPress(playerSkin);});
        upgradeSkinButton.onClick.AddListener(delegate{InGameShopController.Instance.PreviewSkin(playerSkin); OnUpgradeButtonPress(playerSkin);});
    }

    private void SetUpButtonText(PlayerSkin playerSkin)
    {
        if(playerSkin.isOwned == false)
        {
            upgradeSkinButton.gameObject.SetActive(false);
            claimSkinButton.gameObject.SetActive(true);

            if(playerSkin.numberOfCardOwned >= playerSkin.numberOfCardToUnlock)
            {
                claimSkinbuttonText.text = "Claim";
            }

            else
            {
                claimSkinbuttonText.text = playerSkin.numberOfCardOwned + "/" + playerSkin.numberOfCardToUnlock;
            }
            
        }
        else if(playerSkin.isOwned == true)
        {
            upgradeSkinButton.gameObject.SetActive(true);
            claimSkinButton.gameObject.SetActive(false);
        }
    }

    private void ToggleisEquipedText(PlayerSkin playerSkin)
    {
        if(PlayerPrefs.GetInt("equiped") == playerSkin.skinIndex)
        {
            isEquipedText.SetActive(true);
        }
        else if(PlayerPrefs.GetInt("equiped") != playerSkin.skinIndex)
        {
            isEquipedText.SetActive(false);
        }
    }

    public void OnSkinButtonPress(PlayerSkin playerSkin)
    {
        SoundController.Instance.PlayButtonSound();
        if (playerSkin.numberOfCardOwned >= playerSkin.numberOfCardToUnlock)
        {
            RemoveCardFromArchive(playerSkin, playerSkin.numberOfCardToUnlock);
            UnlockSkin();
        }

        else
        {
            InGameShopController.Instance.InstantiateText("Not enough <color=green>Card</color> to unlock");
        }
    }

    public void OnAdsButtonPressed()
    {
        PlayerPrefs.SetInt("currentSkinPannel", playerSkin.skinIndex);

        if (AdManager.Instance.CanShowRewardedInterstitialAd())
        {
            var skinEvent = new UnityEvent();
            //skinEvent.AddListener(GiveSkinReward);
            AdManager.Instance.ShowRewardedInterstitialAd(skinEvent);
        }
    }

    IEnumerator OpenUnlockSkinScene()
    {
        yield return new WaitForSeconds(0);
        Manager.Load(UnlockSkinSceneController.UNLOCKSKINSCENE_SCENE_NAME, playerSkin);
    }

    public void OnUpgradeButtonPress(PlayerSkin playerSkin)
    {
        if(playerSkin.isOwned == true && playerSkin.skinLevel < 2)
        {
            if(playerSkin.numberOfCardOwned >= playerSkin.cardToUpgradeSkin[playerSkin.skinLevel + 1])
            {
                RemoveCardFromArchive(playerSkin, playerSkin.cardToUpgradeSkin[playerSkin.skinLevel + 1]);
                playerSkin.skinLevel += 1;
                
                skinLevelText.text = "Lv." + playerSkin.skinLevel.ToString();
                InGameShopController.Instance.PreviewSkin(playerSkin);
                InGameShopController.Instance.InstantiateText("<color=green>Skin Upgraded!!</color>");
                GameManager.Instance.SetUpNumberOfLifeShield();
                PlayerPrefs.SetInt("skinLv"+playerSkin.skinId.ToString(), playerSkin.skinLevel);

                CheckIsSkinEquiped(playerSkin);
            } 
            else
            {
                InGameShopController.Instance.InstantiateText("Not Enough <color=green>Cards</color> To Upgrade");
            }
        }
        else if(playerSkin.skinLevel == 2)
        {
             InGameShopController.Instance.InstantiateText("<color=yellow>Skin has upgraded to max level</color>");
        }

        else
        {
            InGameShopController.Instance.InstantiateText("<color=green>To upgrade the skin you need to own the skin first</color>");
        }
    }

    private void CheckIsSkinEquiped(PlayerSkin playerSkin)
    {
        if(CharacterStatController.Instance.CurrentEquipedSkin == playerSkin)
        {
            CharacterStatController.Instance.AddActiveStatSkinToList(playerSkin);
        }
    }

    public void SetUpUpgradeSkinButtonText(PlayerSkin playerSkin)
    {
        if(playerSkin.skinLevel < 2)
        {
            if(playerSkin.numberOfCardOwned >  playerSkin.cardToUpgradeSkin[playerSkin.skinLevel + 1] && playerSkin.isOwned == true)
            {
                upgradeSkinButtonText.text = "<color=green>" + playerSkin.numberOfCardOwned.ToString() + "</color>" + "/" +  playerSkin.cardToUpgradeSkin[playerSkin.skinLevel + 1].ToString();
                upgradeImage.gameObject.SetActive(true);
            }
            else
            {
                upgradeSkinButtonText.text = playerSkin.numberOfCardOwned.ToString() + "/" +  playerSkin.cardToUpgradeSkin[playerSkin.skinLevel + 1].ToString();
                upgradeImage.gameObject.SetActive(false);
            } 
        }
        else
        {
            upgradeSkinButtonText.text = "Max Level";
            upgradeImage.gameObject.SetActive(false);
        }
    }

    public void EquipSkin()
    {
        if(playerSkin.isOwned == true)
        {
            SoundController.Instance.PlayButtonSound();
            SkinLoader.Instance.SetUpSkinToLoad(playerSkin);

            PlayerPrefs.SetInt("savedSkinIndex", playerSkin.skinIndex);
            PlayerPrefs.SetInt("equiped", playerSkin.skinIndex);

            DisplayCurrentCharacter.Instance.SetUpCurrentSkins(playerSkin, WeaponSkinArchive.Instance.GetWeaponSkinFromArchive(PlayerPrefs.GetInt("savedWeaponSkinIndex")));
        }
        else
        {
            InGameShopController.Instance.InstantiateText("Unlock the skin first");
        }
    }    

    private void UnlockSkin()
    {
        playerSkin.isOwned = true;
        PlayerPrefs.SetInt(playerSkin.skinId.ToString(), 1);
        QuestManager.Instance.AchievementController.DoAchievementUnlock(AchievementType.UnlockSkin);
        InGameShopController.Instance.PreviewSkin(playerSkin);
        SetUpSkinSprite();
        StartCoroutine(OpenUnlockSkinScene());
    }

    private void LoadSkinInfo(PlayerSkin playerSkin)
    {
        this.GetComponent<Image>().sprite = PlayerSkinArchive.Instance.GetItemFrameFromRarity(playerSkin.itemRarity);

        playerSkin.isOwned = PlayerPrefs.GetInt(playerSkin.skinId.ToString()) == 1;
        playerSkin.skinLevel = PlayerPrefs.GetInt("skinLv" + playerSkin.skinId.ToString(),1);
        playerSkin.numberOfCardOwned = PlayerPrefs.GetInt("skinCardOwned"+playerSkin.skinId.ToString());

        skinImage.sprite = playerSkin.itemThumpNail;
        skinLevelText.text = "Lv." + playerSkin.skinLevel.ToString();

        if(playerSkin.isOwned == true)
        {
            lockImage.SetActive(false);
        }
        if(playerSkin.skinId == 0)
        {
            playerSkin.isOwned = true;
            PlayerPrefs.SetInt("0",1);
        }
    }

    public void RemoveCardFromArchive(PlayerSkin playerSkin, int numberOfCardToRemove)
    {
        playerSkin.numberOfCardOwned -= numberOfCardToRemove;
        PlayerPrefs.SetInt("skinCardOwned"+playerSkin.skinId.ToString(), playerSkin.numberOfCardOwned);
    }

    public void AddCardToArchive(PlayerSkin playerSkin, int numberOfCardToAdd)
    {        
        playerSkin.numberOfCardOwned += numberOfCardToAdd;
        PlayerPrefs.SetInt("skinCardOwned"+playerSkin.skinId.ToString(), playerSkin.numberOfCardOwned);
    }

    private void LoadSkinInfoPopup(PlayerSkin playerSkin)
    {
        Manager.Add(SkinInfoSceneController.SKININFOSCENE_SCENE_NAME, playerSkin);
    }


    // private void GiveSkinReward()
    // {
    //     SkinPannel currentSkinPanel = (PlayerPrefs.GetInt("currentSkinPannel"));
    //     currentSkinPanel.AddAdsProgress(1);
    // }

    // public void AddAdsProgress(int amountToAdd)
    // {
    //     numberOfAdsWatch += amountToAdd;
    //     PlayerPrefs.SetInt(playerSkin.skinId + "numberAdsWatched", numberOfAdsWatch);

    //     if(numberOfAdsWatch == numberOfAdsToUnlockSkin)
    //     {
    //         UnlockSkin();
    //         PlayerPrefs.SetInt("CanPlaySound",1);
    //         StartCoroutine(OpenUnlockSkinScene());
    //     }
    // }
}
