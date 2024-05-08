using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.View;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class ConfirmPurchaseSceneController : Controller
{
    [Header("Purchase Info")]
        [SerializeField] private TextMeshProUGUI itemPriceText;
        [SerializeField] private TextMeshProUGUI gemQuantityText;
        [SerializeField] private TextMeshProUGUI confirmText;
        [SerializeField] private TextMeshProUGUI confirmButtonText;
        [SerializeField] private Image itemImage;
    
    [Header("Purchase Completed Info")]
        [SerializeField] private GameObject successPurchaseGameObject;
        [SerializeField] private GameObject backGroundGameObject;

    [Header("Buttons")]
        [SerializeField] private Button purchaseButton;
        [SerializeField] private Button noThanksButton;
        [SerializeField] private Button claimButton;
    

    public const string CONFIRMPURCHASESCENE_SCENE_NAME = "ConfirmPurchaseScene";
    private GemPackItem gemPackItem;

    private void Start()
    {
        SetUpPopUpInfo();
        SetUpButton();
    }

    public override string SceneName()
    {
        return CONFIRMPURCHASESCENE_SCENE_NAME;
    }

    public override void OnActive(object data)
    {
        if(data != null)
        {
            gemPackItem = (GemPackItem)data;
        }
    }

    private void SetUpPopUpInfo()
    {
        itemPriceText.text = "$" + gemPackItem.itemPrice.ToString();
        gemQuantityText.text = "+" + gemPackItem.gemQuantity.ToString();
        confirmText.text = "Are you sure you want to spend <color=green>$" + gemPackItem.itemPrice.ToString() + "</color> on this product?";
        itemImage.sprite = gemPackItem.itemImage.sprite;
        confirmButtonText.text = "Spend $" + gemPackItem.itemPrice.ToString();
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
        backGroundGameObject.SetActive(false);
        successPurchaseGameObject.SetActive(true);
        claimButton.gameObject.SetActive(true);

        gemPackItem.PurchaseItem();
        itemImage.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f);
    }
}