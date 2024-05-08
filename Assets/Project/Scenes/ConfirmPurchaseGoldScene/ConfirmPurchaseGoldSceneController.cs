using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.View;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class ConfirmPurchaseGoldSceneController : Controller
{
    [Header("Purchase Info")]
        [SerializeField] private TextMeshProUGUI itemPriceText;
        [SerializeField] private TextMeshProUGUI goldAmountText;
        [SerializeField] private TextMeshProUGUI confirmText;
        [SerializeField] private TextMeshProUGUI confirmButtonText;
        [SerializeField] private Image itemImage;

    [Header("Purchase Completed Info")]
        [SerializeField] private GameObject successPurchaseGameObject;
        [SerializeField] private GameObject backGroundGameObject;
        [SerializeField] private GameObject priceTextGameObject;

    [Header("Buttons")]
        [SerializeField] private Button purchaseButton;
        [SerializeField] private Button noThanksButton;
        [SerializeField] private Button claimButton;

    public const string CONFIRMPURCHASEGOLDSCENE_SCENE_NAME = "ConfirmPurchaseGoldScene";

    private GoldPackItem goldPackItem;
    private bool isNotEnoughMoney = false;

    private void Start()
    {
        SetUpPopUpInfo();
        SetUpButton();
    }

    public override string SceneName()
    {
        return CONFIRMPURCHASEGOLDSCENE_SCENE_NAME;
    }
    public override void OnActive(object data)
    {
        if(data != null)
        {
            goldPackItem = (GoldPackItem)data;
        }
    }

    private void SetUpPopUpInfo()
    {
        itemPriceText.text = goldPackItem.itemPrice.ToString();
        goldAmountText.text = "+" + goldPackItem.goldAmount.ToString();
        confirmText.text = "Are you sure you want to spend <color=purple>" + goldPackItem.itemPrice.ToString() + " gems</color> on this product?";
        itemImage.sprite = goldPackItem.itemImage.sprite;
        confirmButtonText.text = "Spend " + goldPackItem.itemPrice.ToString();
    }
    
    private void SetUpButton()
    {
        noThanksButton.onClick.AddListener(delegate{ClosePopUp();});
        purchaseButton.onClick.AddListener(delegate{PurchaseItem();});
        claimButton.onClick.AddListener(delegate{ClosePopUp();});

    }

    public void ClosePopUp()
    {
        Manager.Close();
    }

    private void PurchaseItem()
    {
        if (Bank.Instance.TryRemoveMoney(goldPackItem.itemPrice, Currency.Gem))
        {
            goldPackItem.PurchaseItem();

            priceTextGameObject.SetActive(false);
            backGroundGameObject.SetActive(false);
            successPurchaseGameObject.SetActive(true);
            claimButton.gameObject.SetActive(true);

            itemImage.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f);
        }
        else
        {   
            isNotEnoughMoney = true;
            Manager.Close();
        }
    }

    public override void OnHidden()
    {
        if(isNotEnoughMoney)
        {
            Manager.Add("NotEnoughMoneyScene");
        }
    }
}