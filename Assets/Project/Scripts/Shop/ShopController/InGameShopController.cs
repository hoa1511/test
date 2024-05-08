using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class InGameShopController : MonoBehaviour
{
    [Header("Shop Item Info")]
        [SerializeField] private TextMeshProUGUI itemPreviewName;
        [SerializeField] private TextMeshProUGUI itemStatusText;
        [SerializeField] private Transform previewSkinHolder;
        [SerializeField] private Transform lockedPreviewSkinHolder;
        [SerializeField] private Transform previewWeaponHolder;

    [Header("Spawn Texts")]
        [SerializeField] private GameObject spawnTextPrefab;
        [SerializeField] private Transform spawnTextHolder;

    [Header("Skin Stat")]
        [SerializeField] private GameObject skinStatField;
        [SerializeField] private TextMeshProUGUI skinStatText;

    [Header("Current Skin")]
        [SerializeField] private PlayerWeapon currentWeapon;
        [SerializeField] private PlayerSkin currentPlayerSkin;
        [SerializeField] private GameObject currentPreviewSkin;

    public static InGameShopController Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }    
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        InstantiatePreviewSkin();
    }
    
#region Skin
    public void PreviewSkin(PlayerSkin playerSkin)
    {
        if(playerSkin.itemName != itemPreviewName.text)
        {
            itemPreviewName.text = playerSkin.itemName;
            itemPreviewName.color = playerSkin.skinColor;
            itemStatusText.text = GetSkinStatus(playerSkin.isOwned, playerSkin.numberOfCardToUnlock);
            itemStatusText.color = Color.white;
            ChangePreviewSkin(playerSkin);
        }
    }

    private string GetSkinStatus(bool isOwned, int numberOfCardToUnlock)
    {
        switch(isOwned)
        {
            case(true):
            return "";
            case(false):
            return "Need " + "<color=green>" + numberOfCardToUnlock + "</color>" + " cards To Unlock";
        }
    }

    private void ChangePreviewSkin(PlayerSkin playerSkin)
    {
        DisableAllPreviewSkin();
        if(playerSkin.isOwned == true)
        {
            skinStatField.gameObject.SetActive(true);
            DisplaySkinStat(playerSkin);
            LoadUnLockedPreviewSkin(playerSkin);
        }
        else if(playerSkin.isOwned == false)
        {
            skinStatField.gameObject.SetActive(false);
            LoadLockedPreviewSkin(playerSkin);
        }
        ChangePrevieuWeapon(currentWeapon);
    }

    private void DisableAllPreviewSkin()
    {
        foreach(Transform child in lockedPreviewSkinHolder)
        {  
            child.gameObject.SetActive(false);
        }

        foreach(Transform child in previewSkinHolder)
        {  
            child.gameObject.SetActive(false);
        }
    }

    private void LoadUnLockedPreviewSkin(PlayerSkin playerSkin)
    {
        foreach(Transform child in previewSkinHolder)
        {
            if(child.name == playerSkin.previewSkin.name)
            {
                child.gameObject.SetActive(true);
                currentPreviewSkin = child.gameObject;
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    private void LoadLockedPreviewSkin(PlayerSkin playerSkin)
    {
        foreach(Transform child in lockedPreviewSkinHolder)
        {
            if(child.name == playerSkin.unknowSkin.name)
            {
                child.gameObject.SetActive(true);
                currentPreviewSkin = child.gameObject;
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
    }
#endregion

#region Weapon Skin
    public void PreviewWeapon(PlayerWeapon playerWeapon)
    {
        currentWeapon = playerWeapon;

        skinStatField.gameObject.SetActive(false);
        
        itemPreviewName.text = playerWeapon.itemName;
        itemPreviewName.color = playerWeapon.weaponColor;
        itemStatusText.color = Color.white;
        ChangePrevieuWeapon(playerWeapon);
    }

    private string GetWWeaponStatus(bool isOwned, int numberOfGoldToUnlock)    
    {
        switch(isOwned)
        {
            case(true):
            return "Owned";
            case(false):
            return "Need " + "<color=yellow>" + numberOfGoldToUnlock + "</color>" + " gold To Unlock";
        }
    }

    private void ChangePrevieuWeapon(PlayerWeapon playerWeapon)
    {
        currentPreviewSkin.transform.GetChild(0).GetChild(5).GetChild(1).GetChild(1).GetComponent<Renderer>().material = playerWeapon.weaponMaterial;
        //Instantiate(playerWeapon.previewWeaponModel, currentPreviewSkin.transform.GetChild(0).GetChild(5));
    }
#endregion

#region Skin Stat
    private void DisplaySkinStat(PlayerSkin playerSkin)
    {
        skinStatText.text = "";
        if(playerSkin.skinStats.Count > 0)
        {
            for(int i = 0 ; i < playerSkin.skinStats[playerSkin.skinLevel].passiveStats.Count; i++)
            {
                skinStatText.text += "<color=#00ff00ff>Passive :</color> " + "+" + playerSkin.skinStats[playerSkin.skinLevel].passiveStats[i].statNumber + " " + PlayerSkinArchive.Instance.GetPassiveStatText(playerSkin.skinStats[playerSkin.skinLevel].passiveStats[i].skinStat) + "\n";
            }

            skinStatText.text += "\n";

            for(int i = 0; i < playerSkin.skinStats[playerSkin.skinLevel].activeStats.Count; i++)
            {
                skinStatText.text += "<color=#ff0000ff>Active :</color> " + PlayerSkinArchive.Instance.GetActiveStatText(playerSkin.skinStats[playerSkin.skinLevel].activeStats[i].skinStat);
            }
        }
    }
#endregion

    public void InstantiatePreviewSkin()
    {
        currentPlayerSkin = PlayerSkinArchive.Instance.GetPlayerSkinFromArchive(PlayerPrefs.GetInt("savedSkinIndex"));
        currentWeapon = WeaponSkinArchive.Instance.GetWeaponSkinFromArchive(PlayerPrefs.GetInt("savedWeaponSkinIndex"));
        PreviewSkin(currentPlayerSkin);
    }

    public void InstantiateText(string textToSpawn)
    {
        GameObject spawnText = Instantiate(spawnTextPrefab, spawnTextHolder);
        spawnText.GetComponent<TextMeshProUGUI>().text = textToSpawn;
        Destroy(spawnText,2f);
    }
}
