using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class CrateOpenerManager : MonoBehaviour {

    public GameObject UIOpen;
    public GameObject UIWon;

    [Header("Open")]
    public Animator anim;
    public List<Image> images;
    public int wonIndex;

    [Header("Won")]
    public Image spriteOnWon;
    private string wonItem;

    //Mini database of sprites (if you have a database of your entire itens you don't need this, down on OpenCrate
    //                          method i explain how to use your database system)
    [Header ("Item In Chest")]
        public ItemInChest[] cards;
        public ItemInChest[] sortedCards;

    private Dictionary<string, Sprite> sprites;

    void Awake()
    {
       sortedCards = cards.OrderBy(card => card.cards.itemRarity).ToArray();
    }

    //Sends the open command
    public void OpenCrate1()
    {
        OpenCrate(CrateManager.crate1);
    }

    //Sends the open command
    public void OpenCrate2()
    {
        OpenCrate(CrateManager.crate2);
    }

    //Opens the crate
    private void OpenCrate(Crate c)
    {
        sprites = new Dictionary<string, Sprite>();

        for(int i = 0; i < sortedCards.Length; i++)
        {
            sprites.Add("crate" + i, sortedCards[i].cards.itemThumpNail);
        }

        //Goes for all the images on the open crate array and sets a sprite to it
        for (int i = 0; i < images.Count; i++)
        {
            //If this is the sprite that shows the item won
            if (i == wonIndex)
            {
                //Gets the id of the item
                string index = c.getReward().getId();
                string iString = index.Substring("crate".Length);
                int posCard = int.Parse(iString);

                //Here is where you use your database system if have one just use it 
                //(ex: yourDatabase.getItem(index).getSprite())
                sortedCards[posCard].countCard = calculateCountCard((int)sortedCards[posCard].cards.itemRarity, posCard);
                images[i].transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = sortedCards[posCard].countCard.ToString();
                images[i].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = sortedCards[posCard].cards.itemThumpNail;
                images[i].GetComponent<Image>().sprite = PlayerSkinArchive.Instance.GetItemFrameFromRarity(sortedCards[posCard].cards.itemRarity);
                spriteOnWon.sprite = sortedCards[posCard].cards.itemThumpNail;
                wonItem = index;

                //Add Card Number To Player Archive
                PlayerSkin playerSkin = PlayerSkinArchive.Instance.GetPlayerSkinFromArchiveWithCard(sortedCards[posCard].cards);

                PlayerItemController.Instance.AddSkinCard(playerSkin, sortedCards[posCard].countCard);


                //Down here you can add it to the inventory of the user
            } 
            else
            {
                //Sets the other sprite that didn't won
                string index = c.getReward().getId();
                string iString = index.Substring("crate".Length);
                int posCard = int.Parse(iString);

                images[i].transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = calculateCountCard((int)sortedCards[posCard].cards.itemRarity, posCard).ToString();
                images[i].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = sortedCards[posCard].cards.itemThumpNail;
                images[i].GetComponent<Image>().sprite = PlayerSkinArchive.Instance.GetItemFrameFromRarity(sortedCards[posCard].cards.itemRarity);
            }
        }

        UIOpen.SetActive(true);

        anim.SetBool("open", true);
        StartCoroutine(ShowCrateOpening(c));
    }

    //Waits until the crate has open
    IEnumerator ShowCrateOpening(Crate c)
    {
        yield return new WaitForSeconds(2f);
        anim.SetBool("open", false);
        yield return new WaitForSeconds(1.5f);

        UIWon.SetActive(true);
        UIOpen.SetActive(false);

    }

    //Method for returning to main screen
    public void takeIt()
    {
        UIWon.SetActive(false);
    }

    private int calculateCountCard(int idx, int posCard)
    {
        switch(idx)
        {
            case 0:
                sortedCards[posCard].rangeRandomMin = 6;
                sortedCards[posCard].rangeRandomMax = 9;
                break;
            case 1:
                sortedCards[posCard].rangeRandomMin = 5;
                sortedCards[posCard].rangeRandomMax = 7;
                break;
            case 2:
                sortedCards[posCard].rangeRandomMin = 4;
                sortedCards[posCard].rangeRandomMax = 5;
                break;
            case 3:
                sortedCards[posCard].rangeRandomMin = 2;
                sortedCards[posCard].rangeRandomMax = 3;
                break;

        }

        return Random.Range(sortedCards[posCard].rangeRandomMin, sortedCards[posCard].rangeRandomMax);
    }

}