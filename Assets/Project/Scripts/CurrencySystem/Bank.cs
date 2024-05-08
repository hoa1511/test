using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum Currency
{
    Gold,
    Gem
}

public class Bank : MonoBehaviour
{
    private int numberOfCoinCollect;
    private int earnBalance;

    private int gold;
    private int gem;

    public int Gold { get => gold; set => gold = value; }
    public int Gem { get => gem; set => gem = value; }
    public int NumberOfCoinCollect { get => numberOfCoinCollect; set => numberOfCoinCollect = value; }
    public int EarnBalance { get => earnBalance; set => earnBalance = value; }

    public static Bank Instance;

    private void Awake()
    {
        PlayerPrefs.SetInt("BonusBalance", 0);

        if(Instance == null)
        {
            Instance = this;
        }    
        else
        {
            Destroy(this);
        }

        earnBalance = 0;
        numberOfCoinCollect = 0;
    }

    public void SaveCurrency()
    {
        PlayerPrefs.SetInt("curGold", gold);
        PlayerPrefs.SetInt("curGem", gem);
    }

    private void Start()
    {
        gold = PlayerPrefs.GetInt("curGold");
        gem = PlayerPrefs.GetInt("curGem");
    }

    public void Deposit(int amount, Currency currency)
    {
        switch (currency)
        {
            case Currency.Gold:
                gold += amount;
                break;
            case Currency.Gem:
                gem += amount;
                break;
        }
        SaveCurrency();
    }

    public void Withdraw(int amount, Currency currency)
    {
        switch(currency)
        {
            case Currency.Gold:
                gold -= amount;
                QuestManager.Instance.AchievementController.DoAchievementSpendMoney(amount);
                break;
            case Currency.Gem:
                gem -= amount;
                break;
        }
        SaveCurrency();
    }

    public bool TryRemoveMoney(int moneyToRemove, Currency currency)
    {
        var canBuy = false;
        switch(currency)
        {
            case Currency.Gold:
                canBuy = (gold - moneyToRemove) >= 0;
                break;
            case Currency.Gem:
                canBuy = (gem - moneyToRemove) >= 0;
                break;
        }
        if (canBuy) { Withdraw(moneyToRemove, currency); }
        return canBuy;
    }

    public void UpdateEarnBalance(int amount)
    {
        EarnBalance += amount;
    }

    public void UpdateNumberOfCoinCollect()
    {
        NumberOfCoinCollect += 1;
    }

}
