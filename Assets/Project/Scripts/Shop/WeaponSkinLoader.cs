using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSkinLoader : MonoBehaviour
{
    private GameObject skinToLoad;
    [SerializeField]private PlayerWeapon playerWeapon;
    public static WeaponSkinLoader Instance;

    public GameObject SkinToLoad {get => skinToLoad; set => skinToLoad = value;}
    public PlayerWeapon PlayerWeapon {get => playerWeapon; set => playerWeapon = value;}

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
        LoadLastSavedSkin();
    }

    private void LoadLastSavedSkin()
    {
        playerWeapon = WeaponSkinArchive.Instance.GetWeaponSkinFromArchive(PlayerPrefs.GetInt("savedWeaponSkinIndex"));
        //skinToLoad = playerWeapon.inGameWeaponModel;
    }

    public void SetUpSkinToLoad(PlayerWeapon playerWeapon)
    {
        this.playerWeapon = playerWeapon;
        LoadSkin();
    }

    private void LoadSkin()
    {
        //playerSkin = PlayerSkinArchive.Instance.GetPlayerSkinFromArchive(PlayerPrefs.GetInt("savedSkinIndex"));
        //skinToLoad = playerWeapon.inGameWeaponModel;
    }
}
