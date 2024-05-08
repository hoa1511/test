using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SS.View;

public class GemPackItem : MonoBehaviour
{
    [Header("Item Info UI")]
        [SerializeField] private TextMeshProUGUI itemPriceText;
        [SerializeField] private TextMeshProUGUI gemQuantityText;
        [SerializeField] private Button purchaseButton;

    [Header("Item Info")]
        public Image itemImage;
        public float itemPrice;
        public int gemQuantity;

    private void Start()
    {
        SetUpItemInfo();
        SetUpPurchaseButton();
    }

    public void PurchaseItem()
    {
        //If Successful Purchase
        //Popup Confirm UI
        //When User Press Confirm Button
        //Call Deposit Function

        Bank.Instance.Deposit(gemQuantity, Currency.Gem);
    }

    private void SetUpItemInfo()
    {
        itemPriceText.text = "$" + itemPrice.ToString();
        gemQuantityText.text = gemQuantity.ToString();
    }

    private void SetUpPurchaseButton()
    {
        purchaseButton.onClick.AddListener(delegate{OpenConfirmPurchasePopup(this);});
    }

    private void OpenConfirmPurchasePopup(GemPackItem item)
    {
        Manager.Add(ConfirmPurchaseSceneController.CONFIRMPURCHASESCENE_SCENE_NAME, item);
    }
}
