using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSkinArchive : MonoBehaviour
{
    [Header("Skin Archives")]
        [SerializeField] private List<PlayerWeapon> weaponSkinList = new List<PlayerWeapon>();

    [Header("Skin Frame")]
        [SerializeField] private Sprite commonFrame;
        [SerializeField] private Sprite rareFrame;
        [SerializeField] private Sprite epicFrame;
        [SerializeField] private Sprite legendaryFrame;

    public static WeaponSkinArchive Instance;
    public List<PlayerWeapon> WeaponSkinList {get=>weaponSkinList;}
    
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

    public PlayerWeapon GetWeaponSkinFromArchive(int savedSkinIndex)
    {
        for(int i = 0 ; i < weaponSkinList.Count; i++)
        {
            if(weaponSkinList[i].weaponSkinIndex == savedSkinIndex)
            {
                return weaponSkinList[i];
            }
        }
        return weaponSkinList[0];
    }

    public void EndGameTracking(int currentLevel)
    {
        for(int i = 0; i < weaponSkinList.Count; i++)
        {
            if(weaponSkinList[i].levelToUnlock == currentLevel)
            {
                UnlockSkin(weaponSkinList[i]);
            }
        }
    }

    public void UnlockSkin(PlayerWeapon weapon)
    {
        weapon.isOwned = true;
        QuestManager.Instance.AchievementController.DoAchievementUnlock(AchievementType.UnlockTrident);
        PlayerPrefs.SetInt(weapon.weaponId.ToString(), 1);
    } 

    public void EquipWeapon(PlayerWeapon weapon)
    {
        if(weapon.isOwned == true)
        {
            SoundController.Instance.PlayButtonSound();
            WeaponSkinLoader.Instance.SetUpSkinToLoad(weapon);

            PlayerPrefs.SetInt("savedWeaponSkinIndex", weapon.weaponSkinIndex);
            PlayerPrefs.SetInt("equipedWeapon", weapon.weaponSkinIndex);
        }

        else
        {
            InGameShopController.Instance.InstantiateText("Reach level <color=green>" + weapon.levelToUnlock + "</color> to unlock skin!");
        }
    }

    public Sprite GetItemFrameFromRarity(itemRarity rarity)
    {
        switch(rarity)
        {
            case itemRarity.Common:
            return commonFrame;
            case itemRarity.Rare:
            return rareFrame;
            case itemRarity.Epic:
            return epicFrame;
            case itemRarity.Legendary:
            return legendaryFrame;
        }
        return null;
    }
}
