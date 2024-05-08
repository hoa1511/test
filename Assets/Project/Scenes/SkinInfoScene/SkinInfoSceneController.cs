using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SS.View;
using TMPro;

public class SkinInfoSceneController : Controller
{
    [Header("PopUp Info")]
        [SerializeField] private Image background;
        [SerializeField] private TextMeshProUGUI skinName;
        [SerializeField] private TextMeshProUGUI skinRarity;

        [SerializeField] private Image skinSprite;
        [SerializeField] private Button closeButton;

        [SerializeField] private TextMeshProUGUI statTexts;
        [SerializeField] private Button level1Button;
        [SerializeField] private Button level2Button;
        [SerializeField] private Button level3Button;
        [SerializeField] private Button level4Button;

    private PlayerSkin playerSkin;
    public const string SKININFOSCENE_SCENE_NAME = "SkinInfoScene";

    public override string SceneName()
    {
        return SKININFOSCENE_SCENE_NAME;
    }

    public override void OnActive(object data)
    {
        if(data != null)
        {
            playerSkin = (PlayerSkin)data;
        }
    }

    private void Start()
    {
        Color backGroundColor = PlayerSkinArchive.Instance.GetRarityColor(playerSkin.itemRarity);

        backGroundColor.a = 0.4f;
        background.color = backGroundColor;

        skinName.text = playerSkin.itemName;
        skinName.color = playerSkin.skinColor;
        skinSprite.sprite = playerSkin.itemThumpNail;
        skinRarity.text = playerSkin.itemRarity.ToString();
        skinRarity.color = PlayerSkinArchive.Instance.GetRarityColor(playerSkin.itemRarity);

        LoadSkinStat(1);
        SetupButton();  
    }

    private void SetupButton(){
        level1Button.onClick.AddListener(delegate{LoadSkinStat(1);});
        level2Button.onClick.AddListener(delegate{LoadSkinStat(2);});
        level3Button.onClick.AddListener(delegate{LoadSkinStat(3);});
        level4Button.onClick.AddListener(delegate{LoadSkinStat(4);});

        closeButton.onClick.AddListener(delegate{Close();});
    }

    private void LoadSkinStat(int level)
    {
        statTexts.text = "Level" + level + ":" + "\n" + "\n";
        if(playerSkin.skinStats.Count > 0)
        {
            for(int i = 0; i < playerSkin.skinStats[level-1].passiveStats.Count; i++)
            {
                statTexts.text += "<color=red>Passive:</color> " + "+" +  playerSkin.skinStats[level-1].passiveStats[i].statNumber.ToString() + " " + PlayerSkinArchive.Instance.GetPassiveStatText(playerSkin.skinStats[level-1].passiveStats[i].skinStat) + "\n";
            }

            for(int i = 0; i < playerSkin.skinStats[level-1].activeStats.Count; i++)
            {
                statTexts.text += "<color=green>Active:</color> " + PlayerSkinArchive.Instance.GetActiveStatText(playerSkin.skinStats[level-1].activeStats[i].skinStat) + "\n";
            }
        }
    }

    public void Close()
    {
        Manager.Close();
    }

    // private void SetUpSkinStat()
    // {
    //     for(int i = 0; i < statTexts.Length; i++)
    //     {
    //         statTexts[i].text = "Lv" + (i+1) +". " + "\n";
    //         if(playerSkin.skinStats.Count > 0)
    //         {
    //             if(playerSkin.skinStats[i] != null)
    //             {
    //                 for(int y = 0; y < playerSkin.skinStats[i].passiveStats.Count; y++)
    //                 {
    //                     statTexts[i].text += "+ " + playerSkin.skinStats[i].passiveStats[y].statNumber + playerSkin.skinStats[i].passiveStats[y].skinStat.ToString() + "\n"; 
    //                 }
    //             }
    //         }
    //     }
    // }

}