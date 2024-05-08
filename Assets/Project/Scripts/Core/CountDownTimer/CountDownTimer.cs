using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class CountDownTimer : MonoBehaviour
{
    public float setTime = 120;
    public float timeRemaining;
    public bool timeHasRunOut = false;

    private DateTime exitTime;
    private DateTime startTime;

    public static CountDownTimer Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        SetUpTimer();
    }

    private void Update()
    {
        if(timeRemaining > 0)
        {
            timeRemaining -= Time.unscaledDeltaTime;
        }
        else
        {
            ResetDailyDeal();
            timeRemaining = setTime;                  
        }
    }

    private void ResetDailyDeal()
    {
        if(ShopCashUI.Instance != null)
        {
            ShopCashUI.Instance.OnReset();
        }
        else
        {
            timeHasRunOut = true;
        }
    }

    private void OnApplicationQuit()
    {
        exitTime = DateTime.UtcNow;

        PlayerPrefs.SetString("ExitDateTime", exitTime.ToBinary().ToString());
        PlayerPrefs.SetFloat("ExitTime", timeRemaining);
    }

    private void SetUpTimer()
    {
        startTime = DateTime.UtcNow;
        string lastSavedTime = PlayerPrefs.GetString("ExitDateTime") == "" ? "0" : PlayerPrefs.GetString("ExitDateTime");
      
        long temp = Convert.ToInt64(lastSavedTime);
    
        
        exitTime = DateTime.FromBinary(temp);
        timeRemaining = PlayerPrefs.GetFloat("ExitTime") < 0 ? setTime : PlayerPrefs.GetFloat("ExitTime");

        TimeSpan diff = startTime.Subtract(exitTime);

        if(diff.TotalSeconds >= PlayerPrefs.GetFloat("ExitTime"))
        {
            timeRemaining = setTime;
        }

        else
        {
            timeRemaining -= (float)diff.TotalSeconds;
        }
    }
}
