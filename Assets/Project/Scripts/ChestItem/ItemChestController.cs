using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.View;

public class ItemChestController : MonoBehaviour
{
    public static ItemChestController Instance;
    public List<ItemChestComponents> itemChestComponents;
    
    //Old Item
   [SerializeField] private List<Item> itemList = new List<Item>();
    public List<Item> ItemList {get => itemList; set => itemList = value;}

    //New Item
    [SerializeField] private List<ShopChestItem> newitemList = new List<ShopChestItem>();
    public List<ShopChestItem> NewItemList {get => newitemList; set => newitemList = value;}


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
    public ItemChestComponents GetItemChestComponent(ChestType chestType)
    {
        for(int i = 0; i < itemChestComponents.Count; i++)
        {
            if(itemChestComponents[i].ChestType == chestType)
            {
                return itemChestComponents[i];
            }
        }
        return null;
    }

    public void OpenChestInfo(ChestType chestType, ItemChest itemChest)
    {
        Manager.Add("InfoChestPopup", chestType);
    }
}

[System.Serializable]
public class ItemChestComponents
{
    public ChestType ChestType;

    public float mythicPercent;
    public float legendaryPercent;
    public float epicPercent;
    public float rarePercent;
    public float uncommonPercent;
    public float commonPercent;

    public int numberOfItem;
    public int chestPrice;
}

