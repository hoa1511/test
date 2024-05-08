using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemController : MonoBehaviour
{
    public static PlayerItemController Instance;
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

    public void AddSkinCard(PlayerSkin skinToAdd, int amountofCardToAdd)
    {
        skinToAdd.numberOfCardOwned += amountofCardToAdd;
        UpdateNumberOfCard(skinToAdd);
    }

    public void RemoveSkinCard(PlayerSkin skinToRemove, int amountOfSkinToRemove)
    {
        skinToRemove.numberOfCardOwned -= amountOfSkinToRemove;
        UpdateNumberOfCard(skinToRemove);
    }

    public void UpdateNumberOfCard(PlayerSkin playerSkin)
    {
        PlayerPrefs.SetInt("skinCardOwned"+playerSkin.skinId.ToString(), playerSkin.numberOfCardOwned);
    }
}
