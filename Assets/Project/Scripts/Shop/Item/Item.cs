using UnityEngine;

public class Item : ScriptableObject 
{
    [Header("ItemID")]
        public string itemID;

    [Header("ItemRarity")]
        public itemRarity itemRarity;

    [Header("Item Info")]
        public ItemType itemType;
        public string itemName;
        public string itemDescription;

    [Header("Item Sprite")]
        public Sprite itemThumpNail;

    public itemRarity ItemRarity {get=>itemRarity; set => itemRarity = value;}
}
