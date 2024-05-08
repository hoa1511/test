using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SS.View;

public class GoldPackItem : MonoBehaviour
{
    [Header("Item Info UI")]
        [SerializeField] private TextMeshProUGUI itemPriceText;
        [SerializeField] private TextMeshProUGUI goldAmountText;
        [SerializeField] private Button purchaseButton;

    [Header("Item Info")]
        public Image itemImage;
        public int itemPrice;
        public int goldAmount;

    private void Start()
    {
        SetUpItemInfo();
        SetUpPurchaseButton();
    }

    public void PurchaseItem()
    {
        Bank.Instance.Deposit(goldAmount, Currency.Gold);
    }

    private void SetUpItemInfo()
    {
        itemPriceText.text =  itemPrice.ToString();
        goldAmountText.text = goldAmount.ToString();
    }

    private void SetUpPurchaseButton()
    {
        purchaseButton.onClick.AddListener(delegate{OpenConfirmPurchasePopup(this);});
    }

    private void OpenConfirmPurchasePopup(GoldPackItem item)
    {
        Manager.Add(ConfirmPurchaseGoldSceneController.CONFIRMPURCHASEGOLDSCENE_SCENE_NAME, item);
    }
}
