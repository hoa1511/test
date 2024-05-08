using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SS.View;
using TMPro;

public class UnlockTridentSceneController : Controller
{
    [Header("Trident Skin Info")]
        [SerializeField] private TextMeshProUGUI tridentSkinName;
        [SerializeField] private Image tridentSkinSprite;

        [SerializeField] private Button equipNowButton;
        [SerializeField] private Button noThanksButton;

    private PlayerWeapon weaponSkinToUnlock;

    public const string UNLOCKTRIDENTSCENE_SCENE_NAME = "UnlockTridentScene";


    public override string SceneName()
    {
        return UNLOCKTRIDENTSCENE_SCENE_NAME;
    }

    public override void OnActive(object data)
    {
        if(data != null)
        {
            weaponSkinToUnlock = (PlayerWeapon)data;
        }
        base.OnActive(data);
    }

    private void Start()
    {
        SetUpScene();
    }

    private void SetUpScene()
    {
        tridentSkinName.text = weaponSkinToUnlock.itemName;
        tridentSkinName.color = weaponSkinToUnlock.weaponColor;
        tridentSkinSprite.sprite = weaponSkinToUnlock.itemThumpNail;

        noThanksButton.onClick.AddListener(delegate{OnNoThanksButtonPress();});
        equipNowButton.onClick.AddListener(delegate{OnEquipButtonPress();});
    }

    private void OnNoThanksButtonPress()
    {
        SoundController.Instance.PlayButtonSound();
        GameManager.Instance.LoadScene(PlayerPrefs.GetInt("level"));
    }

    private void OnEquipButtonPress()
    {
        SoundController.Instance.PlayButtonSound();
        WeaponSkinArchive.Instance.EquipWeapon(weaponSkinToUnlock);
        GameManager.Instance.LoadScene(PlayerPrefs.GetInt("level"));
    }
}