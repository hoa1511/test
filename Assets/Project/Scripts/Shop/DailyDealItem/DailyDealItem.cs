using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SS.View;
using DG.Tweening;

public class DailyDealItem : MonoBehaviour
{
    [Header("Daily Deal Info")]
        [SerializeField] public Image dailyDealSprite;
        [SerializeField] public TextMeshProUGUI itemQuantityText;
        [SerializeField] public TextMeshProUGUI dailyDealPriceText;
        [SerializeField] public TextMeshProUGUI numberOfCardInfoText;
        [SerializeField] public Image itemFrame;

        [SerializeField] public SkinCard skinCard;
        [SerializeField] public PlayerSkin skinFromCard;
        [SerializeField] public int cardQuantity;
        [SerializeField] public int amountOfCurrencyToUnlock;

    [Header("Shader")]
        [SerializeField] public Image imageShaders;

    [Header("OnClickChange")]
        [SerializeField] public GameObject buyButton;
        [SerializeField] public GameObject bought;
        public bool hasPurchase;

    [Header("Random Item Daily Deal")]
        [SerializeField] public float[] outputRatios = { 0.6f, 0.3f, 0.09f, 0.01f};

    [Header("Animation")]
        [SerializeField] public Animator itemAnimator;
        [SerializeField] private GameObject update;

    private itemRarity randomRarity;

    void Start()
    {
        buyButton.GetComponent<Button>().onClick.AddListener(delegate{OpenConfirmPurchaseScene();});
        RefreshItem();
    }

    private void Update()
    {
        if(skinFromCard is not null)
        {
            if(skinFromCard.numberOfCardOwned >= skinFromCard.cardToUpgradeSkin[skinFromCard.skinLevel + 1])
            {
                update.SetActive(true);
            }
            else
            {
                update.SetActive(false);
            }
        }
    }

    public void PurchaseDailyDealItem()
    {
        PlayerItemController.Instance.AddSkinCard(skinFromCard, cardQuantity);
        imageShaders.materialForRendering.SetFloat("_GrayscaleAmount", 1);

        buyButton.SetActive(false);
        bought.SetActive(true);

        ShopCashUI.Instance.UpdateNumberOfCardInfo();
    }

    private itemRarity getRandomEnum()
    {
        float randomValue = Random.value;
        float cumulativeWeight = 0f;

        for (int i = 0; i < outputRatios.Length; i++)
        {
            cumulativeWeight += outputRatios[i];
            if (randomValue < cumulativeWeight)
            {
                return (itemRarity)i;
            }
        }

        return itemRarity.Common;
    }

    private int GetRarityValue(itemRarity rarity)
    {
        int minValue = 0;
        int maxValue = 0;

        switch (rarity)
        {
            case itemRarity.Common:
                minValue = 1800;
                maxValue = 2200;
                break;
            case itemRarity.Rare:
                minValue = 4800;
                maxValue = 5200;
                break;
            case itemRarity.Epic:
                minValue = 9800;
                maxValue = 12200;
                break;
            case itemRarity.Legendary:
                minValue = 14800;
                maxValue = 16200;
                break;
            default:
                minValue = 0;
                maxValue = 0;
                break;
        }

        return Random.Range(minValue, maxValue + 1);
    }

    public void OnRefresh()
    {
        randomRarity = getRandomEnum();
        skinCard = PlayerSkinArchive.Instance.GetRandomSkinCardWithRarity(randomRarity);
        amountOfCurrencyToUnlock = GetRarityValue(randomRarity);
        cardQuantity = GameManager.Instance.GenerateRandomNumber(1, 5);

        skinFromCard = PlayerSkinArchive.Instance.GetPlayerSkinFromArchiveWithCard(skinCard);

        UpdateNumberOfCardInfo();
        dailyDealSprite.sprite = skinCard.itemThumpNail;
        itemQuantityText.text = "x" + cardQuantity;
        dailyDealPriceText.text = FormatCurrency.Format(amountOfCurrencyToUnlock);
        itemFrame.sprite = PlayerSkinArchive.Instance.GetItemFrameFromRarity(skinCard.itemRarity);

        imageShaders.materialForRendering.SetFloat("_GrayscaleAmount", 0);
        buyButton.SetActive(true);

        bought.SetActive(false);
    }

    public void RefreshItem()
    {
        itemAnimator.Play(0);
    }

    public void UpdateNumberOfCardInfo()
    {
        numberOfCardInfoText.text = skinFromCard.numberOfCardOwned + "/" +  skinFromCard.cardToUpgradeSkin[skinFromCard.skinLevel + 1].ToString();
    }

    public void OpenConfirmPurchaseScene()
    {
        if (Bank.Instance.TryRemoveMoney(amountOfCurrencyToUnlock, Currency.Gold))
        {
            PurchaseDailyDealItem();
            Manager.Add(PurchaseDailyDealSceneController.PURCHASEDAILYDEALSCENE_SCENE_NAME, this);
        }
        else
        {
            Debug.Log("Not Enough Money To Unlock");
            ShopCashUI.Instance.InstantiateText("Not Enough <color=yellow>Gold</color> To Unlock");
        }
    }
}
