using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayNumberOfCoin : MonoBehaviour
{
   TextMeshProUGUI displayCurrentCoin;
    void Start()
    {
        displayCurrentCoin = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerPrefs.GetInt("currentBalance")>1000)
        {
            float numberOfCoin = (float)PlayerPrefs.GetInt("currentBalance")/1000;
            string numberOfCoinReformatted = numberOfCoin.ToString("0.##") + "K";

            displayCurrentCoin.text = numberOfCoinReformatted;
        }
        else
        {
            displayCurrentCoin.text = PlayerPrefs.GetInt("currentBalance").ToString();
        }
        
    }
}
