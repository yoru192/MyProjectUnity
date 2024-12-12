using System;
using UnityEngine;

public class Wallet : MonoBehaviour, IDataPersistence
{
    public int Money { get; private set; }

    public event Action AmountChanged;

    public void LoadData(GameData data)
    {
        this.Money = data.money;
        AmountChanged?.Invoke();
    }

    public void SaveData(GameData data)
    {
        data.money = this.Money;
    }

    public void AddMoney(int amount)
    {
        if(amount <= 0)
            return;
        Money += amount;
        
        AmountChanged?.Invoke();
    }

    public bool TrySpendMoney(int amount)
    {
        if (amount <= 0)
            return false;
        bool isEnough = Money >= amount;

        if (isEnough)
        {
            Money -= amount;
            AmountChanged?.Invoke();
        }

        return isEnough;
    }

    public void NotifyAmountChanged()
    {
        AmountChanged?.Invoke();
    }
}