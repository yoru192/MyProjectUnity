using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class GameData
{
    public int money;
    public List<ItemData> DataInventoryItems;
    public InventorySlot[] DataInventorySlots;
    private InventoryManager manager;
    public float health;
    public int wood;
    public int stone;
    public int steel;
    public int waves;
    
    public GameData()
    {
        this.waves = 0;
        this.money = 0;
        this.wood = 0;
        this.stone = 0;
        this.steel = 0;
        this.DataInventoryItems = new List<ItemData>();
        this.DataInventorySlots = new InventorySlot[28];
    }
}

[System.Serializable]
public class ItemData
{
    public Item item;
    public int count;
    public int selectedSlot;
}

