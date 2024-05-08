using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponPannel : MonoBehaviour
{
    [Header("Player Weapon")]
        [SerializeField] private PlayerWeapon weapon;

    [Header("Weapon Pannel Part")]
        [SerializeField] private Button equipWeaponButton;

        [SerializeField] private Image weaponImage;
        [SerializeField] private GameObject weaponLockImage;
        [SerializeField] private Image weaponEffectImage;
        
        [SerializeField] private GameObject isEquipedText;
        [SerializeField] private GameObject lockImage;

        [SerializeField] private TextMeshProUGUI weaponNameText;
        [SerializeField] private TextMeshProUGUI weaponStatusText;
        

    private void Start()
    {
        SetUpWeaponSkinPannel(weapon);
        SetUpWeaponSkinStatus(weapon);

        this.GetComponent<Image>().sprite = WeaponSkinArchive.Instance.GetItemFrameFromRarity(weapon.itemRarity);
        
        equipWeaponButton.onClick.AddListener(delegate{InGameShopController.Instance.PreviewWeapon(weapon);});
    }

    private void Update()
    {
        ToggleisEquipedText();
        SetUpWeaponSkinStatus(weapon);
    }


    private void ToggleisEquipedText()
    {
        if(isEquipedText != null && isActiveAndEnabled == true)
        {
            if(PlayerPrefs.GetInt("equipedWeapon") == weapon.weaponSkinIndex)
            {
                isEquipedText.SetActive(true);
            }
            else if(PlayerPrefs.GetInt("equipedWeapon") != weapon.weaponSkinIndex)
            {
                isEquipedText.SetActive(false);
            }
        }
    }

    public void EquipWeapon()
    {
        WeaponSkinArchive.Instance.EquipWeapon(weapon);
        DisplayCurrentCharacter.Instance.SetUpCurrentSkins(PlayerSkinArchive.Instance.GetPlayerSkinFromArchive(PlayerPrefs.GetInt("savedSkinIndex")), weapon);
    }

    private void SetUpWeaponSkinPannel(PlayerWeapon playerWeapon)
    {
        weaponImage.sprite = weapon.itemThumpNail;
        weaponNameText.text = weapon.itemName;

        playerWeapon.isOwned = PlayerPrefs.GetInt(playerWeapon.weaponId.ToString()) == 1;

        if(playerWeapon.weaponId == WeaponIDs.Default)
        {
            playerWeapon.isOwned = true;
        }

        if(playerWeapon.isOwned == true)
        {
            lockImage.gameObject.SetActive(false);
        }
    }

    private void SetUpWeaponSkinStatus(PlayerWeapon weapon)
    {
        if(weapon.isOwned == true)
        {
            weaponStatusText.text = "<color=green>Owned</color>";
            lockImage.gameObject.SetActive(false);
            weaponLockImage.gameObject.SetActive(false);
        }
        else
        {
            weaponStatusText.text = "<color=#9a8a9a>Unlock at lv." + weapon.levelToUnlock;
            lockImage.gameObject.SetActive(true);
            weaponLockImage.gameObject.SetActive(true);
        }
    }
}
