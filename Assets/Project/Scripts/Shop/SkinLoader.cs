using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkinLoader : MonoBehaviour
{
    private GameObject lastSavedSkin;
    private PlayerSkin playerSkin;
    public static SkinLoader Instance;

    public GameObject LastSavedSkin {get => lastSavedSkin; set => lastSavedSkin = value;}

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
        LoadLastSavedSkin(playerSkin);
    }

    private void LoadLastSavedSkin(PlayerSkin playerSkin)
    {
        playerSkin = PlayerSkinArchive.Instance.GetPlayerSkinFromArchive(PlayerPrefs.GetInt("savedSkinIndex"));
        CharacterStatController.Instance.CurrentEquipedSkin = playerSkin;
        lastSavedSkin = playerSkin.inGameSkin;

        CharacterStatController.Instance.AddActiveStatSkinToList(playerSkin);
    }

    public void SetUpSkinToLoad(PlayerSkin playerSkin)
    {
        this.playerSkin = playerSkin;
        CharacterStatController.Instance.CurrentEquipedSkin = playerSkin;
        CharacterStatController.Instance.AddActiveStatSkinToList(playerSkin);
        GameManager.Instance.SetUpNumberOfLifeShield();
        LoadSkin();
    }

    private void LoadSkin()
    {
        lastSavedSkin = playerSkin.inGameSkin;
    }
}
