using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayCurrentCharacter : MonoBehaviour
{
    [SerializeField] private Transform characterHolder;

    private PlayerSkin currentSkin;
    private PlayerWeapon currentWeaponSkin;
    private GameObject currentModel;

    public static DisplayCurrentCharacter Instance;

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
        currentSkin = PlayerSkinArchive.Instance.GetPlayerSkinFromArchive(PlayerPrefs.GetInt("savedSkinIndex"));
        currentWeaponSkin =  WeaponSkinArchive.Instance.GetWeaponSkinFromArchive(PlayerPrefs.GetInt("savedWeaponSkinIndex"));

        SetUpCurrentSkins(currentSkin, currentWeaponSkin);
    }

    private void LoadCurrentCharacterSkin(PlayerSkin playerSkin)
    {
        foreach(Transform child in characterHolder)
        {
            if(child.name == playerSkin.previewSkin.name)
            {
                child.gameObject.SetActive(true);
                currentModel = child.gameObject;
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
    }
    private void LoadCurrentWeaponSkin(PlayerWeapon weaponSkin)
    {
        //Should refactor this later
        currentModel.transform.GetChild(0).GetChild(5).GetChild(1).GetChild(1).GetComponent<Renderer>().material = weaponSkin.weaponMaterial;
    }

    public void SetUpCurrentSkins(PlayerSkin skin, PlayerWeapon weaponSkin)
    {
        LoadCurrentCharacterSkin(skin);
        LoadCurrentWeaponSkin(weaponSkin);
    }
}
