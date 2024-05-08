using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Pagination;
using SS.View;
using TMPro;

public class UnlockSkinSceneController : Controller
{
    [SerializeField] private TextMeshProUGUI skinName;
    [SerializeField] private Transform skinHolder;

    private GameObject previewSkin;
    private PlayerSkin playerSkin;
    private SkinLoader skinLoader;
    private GameManager gameManager;

    public const string UNLOCKSKINSCENE_SCENE_NAME = "UnlockSkinScene";

    public override void OnActive(object data)
    {
        if(data != null)
        {
            playerSkin = (PlayerSkin)data;
        }
    }

    private void Awake()
    {
        skinLoader = SkinLoader.Instance;
        gameManager = GameManager.Instance;
    }

    private void Start()
    {
        SoundController.Instance.PLayWinGameSound();
        ShowUnlockSkin();
    }

    private void ShowUnlockSkin()
    {
        foreach(Transform child in skinHolder)
        {
            if(child.name == playerSkin.previewSkin.name)
            {
                child.gameObject.SetActive(true);
                previewSkin = child.gameObject;
                previewSkin.transform.GetChild(0).GetChild(5).GetChild(1).GetChild(1).GetComponent<Renderer>().material = WeaponSkinLoader.Instance.PlayerWeapon.weaponMaterial;

            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
        skinName.text = playerSkin.itemDescription;
        skinName.color = playerSkin.skinColor;
    }

    public override string SceneName()
    {
        return UNLOCKSKINSCENE_SCENE_NAME;
    }

    public void ClaimButton()
    {
        SoundController.Instance.PlayButtonSound();
        // Manager.Load(ShopController.SHOP_SCENE_NAME);
    }

    public void OnEquipButtonPress()
    {
        SoundController.Instance.PlayButtonSound();
        if(playerSkin != null)
        {
            skinLoader.SetUpSkinToLoad(playerSkin);
            PlayerPrefs.SetInt("equiped", playerSkin.skinIndex);
            PlayerPrefs.SetInt("savedSkinIndex", playerSkin.skinIndex);
            Manager.Load("MenuScene");
        }
    }

    public void OnNoThanksButtonPress()
    {
        SoundController.Instance.PlayButtonSound();
        Manager.Load("MenuScene");
    }
}