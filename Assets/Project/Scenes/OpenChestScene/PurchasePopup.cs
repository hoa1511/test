using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using SS.View;

public class PurchasePopup : MonoBehaviour
{
    [SerializeField] private GameObject inChest;
    [SerializeField] private GameObject showAllItem;
    [SerializeField] private Transform instantiateTrans;

    [Header ("Get Set")]
    [SerializeField] private Sprite[] imgChest;
    [SerializeField] private string[] nameChest;
    [SerializeField] private ParticleSystem rotateLight;
    
    [Header ("Item Spawn")]
    public List<ShopChestItem> chestItems;

    private GameObject itemInChest;
    private GameObject popup;
    private int countItemInChest = 0;
    private bool dontOpenChest;
    private TextMeshProUGUI nameText;
    private Image frame;


    private void Start() 
    {
        showAllItem.SetActive(false);

        chestItems = ItemChestController.Instance.NewItemList;

        itemInChest = inChest.transform.GetChild(1).gameObject;  
        frame = itemInChest.GetComponent<Image>();
        nameText = inChest.transform.GetChild(1).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        popup = inChest.transform.parent.GetChild(0).gameObject;

        rotateLight.startColor = PlayerSkinArchive.Instance.GetRarityColor(chestItems[countItemInChest].rarity);

        frame.sprite = PlayerSkinArchive.Instance.GetItemFrameFromRarity(chestItems[countItemInChest].rarity);
        itemInChest.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = chestItems[countItemInChest].itemInChest.itemThumpNail;

        nameText.text = chestItems[countItemInChest].itemInChest.itemName;

        itemInChest.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = chestItems[countItemInChest].rarity.ToString();
        itemInChest.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text = "x" + chestItems[countItemInChest].quantity.ToString();
        itemInChest.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().color = PlayerSkinArchive.Instance.GetRarityColor(chestItems[countItemInChest].rarity); 
    }

    public void OnPurchased()
    {
        inChest.SetActive(true);
        inChest.transform.DOScale(new Vector3(1, 1, 1), 0.25f).SetEase(Ease.InOutBounce);
    }

    [System.Obsolete]
    public void OnChangeItems()
    {
        countItemInChest += 1; 
        itemInChest.transform.DOScaleY(0, 0.15f).SetEase(Ease.InOutSine).OnComplete(() => {
        frame.enabled = true;

        if(countItemInChest < chestItems.Count)
        {
            frame.sprite = PlayerSkinArchive.Instance.GetItemFrameFromRarity(chestItems[countItemInChest].rarity);
            itemInChest.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = chestItems[countItemInChest].itemInChest.itemThumpNail;

            nameText.text = chestItems[countItemInChest].itemInChest.itemName;

            itemInChest.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = chestItems[countItemInChest].rarity.ToString();
            itemInChest.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().color = PlayerSkinArchive.Instance.GetRarityColor(chestItems[countItemInChest].rarity); 
            itemInChest.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text = "x" + chestItems[countItemInChest].quantity.ToString();
            itemInChest.transform.DOScaleY(1f, 0.15f).SetEase(Ease.InOutSine);

            rotateLight.startColor = PlayerSkinArchive.Instance.GetRarityColor(chestItems[countItemInChest].rarity);
        }
        else
        {
            showAllItem.SetActive(true);
            if(!dontOpenChest)
            {
                showItems();
                dontOpenChest = true;
            }
            inChest.SetActive(false);
        }

        });
    }

    private void showItems()
    {
        GameObject itemTemplate = instantiateTrans.GetChild(0).gameObject;

        for(int i = 0; i < chestItems.Count; i++)
        {
            GameObject obj = Instantiate(itemTemplate, instantiateTrans);

            obj.GetComponent<Image>().sprite = PlayerSkinArchive.Instance.GetItemFrameFromRarity(chestItems[i].rarity);
            obj.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = chestItems[i].itemInChest.itemThumpNail;
            obj.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = chestItems[i].rarity.ToString();
            obj.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().color = PlayerSkinArchive.Instance.GetRarityColor(chestItems[i].rarity); 
            obj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "x" + chestItems[i].quantity.ToString();
        }

        Destroy(itemTemplate);
    }

    public void turnOffButton()
    {
        showAllItem.GetComponent<Button>().interactable = false;
        //ItemChestController.Instance.ClearList();
    }
}
