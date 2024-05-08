using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkinArchive : MonoBehaviour
{
    public static PlayerSkinArchive Instance;
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

    [Header("Skin Archives")]
        [SerializeField] private List<PlayerSkin> playerSkinList = new List<PlayerSkin>();
        [SerializeField] private List<SkinCard> skinCards = new List<SkinCard>();

    [Header("Skin Frame")]
        [SerializeField] private Sprite commonFrame;
        [SerializeField] private Sprite uncommonFrame;
        [SerializeField] private Sprite rareFrame;
        [SerializeField] private Sprite epicFrame;
        [SerializeField] private Sprite legendaryFrame;
        [SerializeField] private Sprite mythicFrame;
 
    [Header("Rarity Color")]    
        [SerializeField] private Color commonColor;
        [SerializeField] private Color unCommonColor;
        [SerializeField] private Color rareColor;
        [SerializeField] private Color epicColor;
        [SerializeField] private Color legendaryColor;
        [SerializeField] private Color mythichColor;

    public PlayerSkin GetPlayerSkinFromArchive(int savedSkinIndex)
    {
        for(int i = 0 ; i < playerSkinList.Count; i++)
        {
            if(playerSkinList[i].skinIndex == savedSkinIndex)
            {
                return playerSkinList[i];
            }
        }
        return playerSkinList[0];
    }   

    public PlayerSkin GetPlayerSkinFromArchiveWithCard(SkinCard skinCard)
    {
        for(int i = 0 ; i < playerSkinList.Count; i++)
        {
            if(playerSkinList[i].skinId == skinCard.skinsIDs)
            {
                return playerSkinList[i];
            }
        }
        return null;
    } 

    public SkinCard GetRandomSkinCardWithRarity(itemRarity itemRarity)
    {
        List<SkinCard> skinCardList = new List<SkinCard>();
        for(int i = 0 ; i < skinCards.Count; i++)
        {
            if(skinCards[i].itemRarity == itemRarity)
            {
                skinCardList.Add(skinCards[i]);
            }
        }
        if(skinCardList.Count > 0)
        {
            int randomNum  = Random.Range(0, skinCardList.Count);
            return skinCardList[randomNum];
        }
        else
        {
            return null;
        }
    }

    public Sprite GetItemFrameFromRarity(itemRarity rarity)
    {
        switch(rarity)
        {
            case itemRarity.Common:
            return commonFrame;
            case itemRarity.Uncommon:
            return uncommonFrame;
            case itemRarity.Rare:
            return rareFrame;
            case itemRarity.Epic:
            return epicFrame;
            case itemRarity.Legendary:
            return legendaryFrame;
            case itemRarity.Mythic:
            return mythicFrame;
        }
        return null;
    }

    public Color GetRarityColor(itemRarity rarity)
    {
        switch(rarity)
        {
            case itemRarity.Uncommon:
            return unCommonColor;
            case itemRarity.Common:
            return commonColor;
            case itemRarity.Rare:
            return rareColor;
            case itemRarity.Epic:
            return epicColor;
            case itemRarity.Legendary:
            return legendaryColor;
            case itemRarity.Mythic:
            return mythichColor;
        }
        return Color.white;
    }

    public string GetPassiveStatText(PassiveStats stat)
    {
        switch(stat)
        {
            case PassiveStats.PercentGoldPerStage:
                return "%<color=green>Gold</color> Per Stage";
            case PassiveStats.PercentGoldPerChest:
                return "%<color=yellow>Gold</color> Per Chest";
            case PassiveStats.AreaOfEffect:
                return "<color=orange>Area Of Effect</color>";
            case PassiveStats.ImmortalShield:
                return "<color=blue>Immotal Shield</color> For Every 5s";
            case PassiveStats.LifeShield:
                return "<color=blue>Life Shield</color>";
            case PassiveStats.EvadePercent:
                return "%<color=purple>Evade Percent</color>";
            case PassiveStats.PercentIncreaseTridentSpeed:
                return "%<color=purple>Trident Speed</color>";
        }
        return "N/A";
    } 

    public string GetActiveStatText(ActiveStats stat)
    {
        switch(stat)
        {
            case ActiveStats.Thunder:
                return "Thunder";
            case ActiveStats.TridentRain:
                return "Trident Rain";
            case ActiveStats.Roar:
                return "Roar";
            case ActiveStats.HugeTrident:
                return "Huge Trident";
        }
        return "N/A";
    }

}

