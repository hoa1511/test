using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoChestController : MonoBehaviour
{
    [System.Serializable]
    public class ItemChestComponents
    {
        [Header ("Rank Chest")]
            public ChestType ChestType;

        [Header ("Info Chest")]
            public Sprite chestImg;
            public Color infoChestColor;
            public string chestName;
    }

    public List<ItemChestComponents> item;

}
