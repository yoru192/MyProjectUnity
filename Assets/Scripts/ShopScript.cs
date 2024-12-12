using UnityEngine;
using System;

public class ShopScript : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Item[] itemsToPickup;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private Resource _resource;

    
    public void PickupItem(int id)
    {
        if (id < 0 || id >= itemsToPickup.Length)
        {
            Debug.LogError("Invalid item ID!");
            return;
        }

        Item selectedItem = itemsToPickup[id];
        int price = selectedItem.price;
        int woodPrice = selectedItem.woodPrice;
        int stonePrice = selectedItem.stonePrice;
        int steelPrice = selectedItem.steelPrice;
        
        if (_wallet.TrySpendMoney(price) && _resource.TrySpendResource("Wood", woodPrice)
                                         && _resource.TrySpendResource("Stone", stonePrice)
                                         && _resource.TrySpendResource("Steel", steelPrice))
        {
            bool result = inventoryManager.AddItem(selectedItem);
            Debug.Log(result ? $"Item '{selectedItem.name}' added to inventory." : "Failed to add item to inventory.");
        }
        else
        {
            Debug.Log("Not enough money to buy this item!");
        }
    }



    public void GetSelectedItem()
    {
        Item receiveItem = inventoryManager.GetSelectedItem(false);
        if (receiveItem != null)
        {
            Debug.Log("Received item: " + receiveItem);
        }
        else
        {
            Debug.Log("No item received!");
        }
    }

    public void UseSelectedItem()
    {
        Item receiveItem = inventoryManager.GetSelectedItem(true);
        if (receiveItem != null)
        {
            Debug.Log("Used item: " + receiveItem);
        }
        else
        {
            Debug.Log("No item used!");
        }
    }
}
