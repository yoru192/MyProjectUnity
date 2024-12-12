using System;
using UnityEngine;

public class Resource : MonoBehaviour, IDataPersistence
{
    public int Wood { get; private set; }
    public int Stone { get; private set; }
    public int Steel { get; private set; }

    public event Action ResourcesChanged;

    public void LoadData(GameData data)
    {
        this.Wood = data.wood;
        this.Stone = data.stone;
        this.Steel = data.steel;
        ResourcesChanged?.Invoke();
    }

    public void SaveData(GameData data)
    {
        data.wood = this.Wood;
        data.stone = this.Stone;
        data.steel = this.Steel;
    }

    public void AddResource(string type, int amount)
    {
        if (amount <= 0) return;

        switch (type)
        {
            case "Wood":
                Wood += amount;
                break;
            case "Stone":
                Stone += amount;
                break;
            case "Steel":
                Steel += amount;
                break;
        }

        ResourcesChanged?.Invoke();
    }

    public bool TrySpendResource(string type, int amount)
    {
        if (amount < 0) return false;

        bool isEnough = false;

        switch (type)
        {
            case "Wood":
                isEnough = Wood >= amount;
                if (isEnough) Wood -= amount;
                break;
            case "Stone":
                isEnough = Stone >= amount;
                if (isEnough) Stone -= amount;
                break;
            case "Steel":
                isEnough = Steel >= amount;
                if (isEnough) Steel -= amount;
                break;
        }

        if (isEnough) ResourcesChanged?.Invoke();
        return isEnough;
    }
}