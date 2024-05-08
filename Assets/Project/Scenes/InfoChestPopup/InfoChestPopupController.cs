using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SS.View;

public class InfoChestPopupController : Controller
{
    public const string INFOCHESTPOPUP_SCENE_NAME = "InfoChestPopup";
    private ChestType chestType;

    public override string SceneName()
    {
        return INFOCHESTPOPUP_SCENE_NAME;
    }


    [SerializeField] private InfoChestController infoChestController;

    [Header ("Info")]
        [SerializeField] private Image chestImg;

        [SerializeField] private TextMeshProUGUI nameChest;

        [SerializeField] private TextMeshProUGUI textCommon;
        [SerializeField] private TextMeshProUGUI textUncommon;
        [SerializeField] private TextMeshProUGUI textRare;
        [SerializeField] private TextMeshProUGUI textEpic;
        [SerializeField] private TextMeshProUGUI textLegend;
        [SerializeField] private TextMeshProUGUI textMythic;

        [SerializeField] private TextMeshProUGUI chestPrice;


    public override void OnActive(object data)
    {
        if(data != null)
        {
            chestType = (ChestType)data;
        }
    }

    private void Start() 
    {
        showChestInfo();
    }

    private void showChestInfo()
    {
        for(int i = 0; i < infoChestController.item.Count; i++)
        {
            if(infoChestController.item[i].ChestType == chestType)
            {
                chestImg.sprite = infoChestController.item[i].chestImg;
                nameChest.text = infoChestController.item[i].chestName;
                nameChest.color = infoChestController.item[i].infoChestColor;

                textCommon.text = ItemChestController.Instance.GetItemChestComponent(chestType).commonPercent.ToString() + "%";
                textUncommon.text = ItemChestController.Instance.GetItemChestComponent(chestType).uncommonPercent.ToString() + "%";
                textRare.text = ItemChestController.Instance.GetItemChestComponent(chestType).rarePercent.ToString() + "%";
                textEpic.text = ItemChestController.Instance.GetItemChestComponent(chestType).epicPercent.ToString() + "%";
                textLegend.text = ItemChestController.Instance.GetItemChestComponent(chestType).legendaryPercent.ToString() + "%";
                textMythic.text = ItemChestController.Instance.GetItemChestComponent(chestType).mythicPercent.ToString() + "%";

                chestPrice.text = ItemChestController.Instance.GetItemChestComponent(chestType).chestPrice.ToString();
            }
        }
    }

    public void Close()
    {
        Manager.Close();
    }
}