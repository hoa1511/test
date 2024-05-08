using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SS.View;

public enum ChestType
{
    CommonChest,
    RareChest,
    EpicChest,
    LegendaryChest
}

public class ItemChest : MonoBehaviour
{
    [Header("Chest Info")]
        [SerializeField] public ChestType chestType;
        [SerializeField] private int numberOfItem;
        [SerializeField] private int chestPrice;
        [SerializeField] private Button ChestButtonInfo;

    [Header("Rarity Percent")]
        [SerializeField] private float mythicPercent;
        [SerializeField] private float legendaryPercent;
        [SerializeField] private float epicPercent;
        [SerializeField] private float rarePercent;
        [SerializeField] private float uncommonPercent;
        [SerializeField] private float commonPercent;
    
    [Header("Items In Chest")]
        [SerializeField] private List<ShopChestItem> newItemInChest = new List<ShopChestItem>();
        [SerializeField] private Button openChestButton;

    private ItemChestController itemChestController;
    private ItemChestComponents itemChestComponent;

    private void Start()
    {
        itemChestController = ItemChestController.Instance;
        SetUpRarityPercent(itemChestController);
        SetUpButton();
    }

    private void SetUpRarityPercent(ItemChestController itemChestController)
    {
        itemChestComponent = itemChestController.GetItemChestComponent(chestType);

        numberOfItem = itemChestComponent.numberOfItem;
        chestPrice = itemChestComponent.chestPrice;

        mythicPercent = itemChestComponent.mythicPercent;
        legendaryPercent = itemChestComponent.legendaryPercent;
        epicPercent = itemChestComponent.epicPercent;
        rarePercent = itemChestComponent.rarePercent;
        uncommonPercent = itemChestComponent.uncommonPercent;
        commonPercent = itemChestComponent.commonPercent;
    }   

    private void PurchaseChest()
    {
        if(Bank.Instance.TryRemoveMoney(chestPrice, Currency.Gem))
        {
            OpenChest();
            Manager.Add("OpenChestScene");
        }
        else
        {
            //Manager.Add(NotEnoughMoneySceneController.NOTENOUGHMONEYSCENE_SCENE_NAME);
            ShopCashUI.Instance.InstantiateText("Not Enough <color=purple>Gems</color> To Unlock");
        }
    }

    public void OpenChest()
    {
        for(int i = 0; i < numberOfItem - 1; i++)
        {
            int randomValue = Random.Range(0,101);
            if (randomValue < mythicPercent)
            {
                AddItemToList(itemRarity.Mythic);
            }

            else if (randomValue < mythicPercent + legendaryPercent)
            {
                AddItemToList(itemRarity.Legendary);
            }

            else if (randomValue < mythicPercent + legendaryPercent + epicPercent)
            {
                AddItemToList(itemRarity.Epic);
            }

            else if (randomValue < mythicPercent + legendaryPercent + epicPercent + rarePercent)
            {
                AddItemToList(itemRarity.Rare);
            }

            else if (randomValue < mythicPercent + legendaryPercent + epicPercent + rarePercent + uncommonPercent)
            {
                AddItemToList(itemRarity.Uncommon);
            }
            
            else
            {
                AddItemToList(itemRarity.Common);
            }
        }

        SkinCard finalCard = GetLastItem(chestType);
        ShopChestItem finalItem = new ShopChestItem(); 

        finalItem.itemInChest = finalCard;
        finalItem.rarity = finalCard.itemRarity;
        finalItem.quantity = GetQuantityByRarity(finalCard.ItemRarity, ItemType.Card);

        newItemInChest.Add(finalItem);

        PlayerSkin skinFromCard = PlayerSkinArchive.Instance.GetPlayerSkinFromArchiveWithCard(finalCard);
        PlayerItemController.Instance.AddSkinCard(skinFromCard, finalItem.quantity);
        
        OnSpawnCompleted();
    }

    private void AddItemToList(itemRarity itemRarity)
    {
        SkinCard card = new SkinCard();
        Gem gem = new Gem();
        ShopChestItem item = new ShopChestItem();

        int randomValue = Random.Range(0,3);

        if(randomValue == 1 && itemRarity != itemRarity.Legendary && itemRarity != itemRarity.Mythic)
        {   
            item.itemInChest = CurencyArchive.Instance.GetGem;
            item.rarity = itemRarity;
            item.quantity = GetQuantityByRarity(itemRarity, ItemType.Gem);

            Bank.Instance.Deposit(item.quantity, Currency.Gem);
        }
        else
        {
            card = PlayerSkinArchive.Instance.GetRandomSkinCardWithRarity(itemRarity);
            item.itemInChest = card;
            item.rarity = card.ItemRarity;
            item.quantity = GetQuantityByRarity(itemRarity, ItemType.Card);

            PlayerSkin skinFromCard = PlayerSkinArchive.Instance.GetPlayerSkinFromArchiveWithCard(card);
            PlayerItemController.Instance.AddSkinCard(skinFromCard, item.quantity);
        }
        
        newItemInChest.Add(item);
    }

    private int GetQuantityByRarity(itemRarity rarity, ItemType itemType)
    {
        switch(rarity)
        {
            case itemRarity.Common:
                if(itemType == ItemType.Card)
                {
                    return 16;
                }
                else if(itemType == ItemType.Gem)
                {
                    return 15;
                }
                else
                {
                    return 0;
                }
            case itemRarity.Uncommon:
                if(itemType == ItemType.Card)
                {
                    return 8;
                }
                else if(itemType == ItemType.Gem)
                {
                    return 30;
                }
                else
                {
                    return 0;
                }
            case itemRarity.Rare:
                if(itemType == ItemType.Card)
                {
                    return 6;
                }
                else if(itemType == ItemType.Gem)
                {
                    return 60;
                }
                else
                {
                    return 0;
                }
            case itemRarity.Epic:
                if(itemType == ItemType.Card)
                {
                    return 4;
                }
                else if(itemType == ItemType.Gem)
                {
                    return 120;
                }
                else
                {
                    return 0;
                }
            case itemRarity.Legendary:
                if(itemType == ItemType.Card)
                {
                    return 2;
                }
                else
                {
                    return 0;
                }
            case itemRarity.Mythic:
                if(itemType == ItemType.Card)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            default:
                return 0;
        }
    }

    private void SetUpButton()
    {
        openChestButton.onClick.AddListener(delegate{PurchaseChest();});
        ChestButtonInfo.onClick.AddListener(delegate{OpenInfoChestPopup();});
    }

    public void OpenInfoChestPopup()
    {
        ItemChestController.Instance.OpenChestInfo(chestType, this);
    }

    private void OnSpawnCompleted()
    {
        ItemChestController.Instance.NewItemList = newItemInChest;

        for(int i = 0; i<newItemInChest.Count; i++)
        {
            Debug.Log(newItemInChest[i].itemInChest.itemName + ":" + newItemInChest[i].quantity);
        }

        newItemInChest = new List<ShopChestItem>();
        
    }

    private SkinCard GetLastItem(ChestType chestType)
    {
        switch(chestType)
        {
            case ChestType.CommonChest:
                return PlayerSkinArchive.Instance.GetRandomSkinCardWithRarity(itemRarity.Common);
            case ChestType.RareChest:
                return PlayerSkinArchive.Instance.GetRandomSkinCardWithRarity(itemRarity.Rare);
            case ChestType.EpicChest:
                return PlayerSkinArchive.Instance.GetRandomSkinCardWithRarity(itemRarity.Epic);
            case ChestType.LegendaryChest:
                return PlayerSkinArchive.Instance.GetRandomSkinCardWithRarity(itemRarity.Legendary);
        }
        return null;
    }
}
