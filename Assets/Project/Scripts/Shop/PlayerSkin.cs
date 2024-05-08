using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "PlayerSkin", menuName = "Player/PlayerSkin", order = 0)]
public class PlayerSkin : Item
{
    [Header("Skin ID")]
        public SkinsIDs skinId;
        public int skinIndex;
    
    [Header("Skin Level")]
    [Range(1,5)]
        public int skinLevel;

    [Header("Skin Model")]
        public GameObject previewSkin;
        public GameObject inGameSkin;
        public GameObject unknowSkin;

    [Header("Skin Price")]
        public int numberOfCardToUnlock;
        public int numberOfCardOwned;

    [Header("Skin Description")]
        public Color skinColor;
        public bool isOwned;

    [Header("Card To UpgradeSkin")]
        public List<int> cardToUpgradeSkin;

    [Header("Bonus Skin Stat")]
        public List<SkinStat> skinStats;
}

[Serializable]
public class SkinStat
{
    public List<SkinActiveStat> activeStats;
    public List<SkinPassiveStat> passiveStats;
}

[Serializable]
public class SkinActiveStat
{
    public ActiveStats skinStat;
}

[Serializable]
public class SkinPassiveStat
{
    public PassiveStats skinStat;
    public int statNumber;
}



