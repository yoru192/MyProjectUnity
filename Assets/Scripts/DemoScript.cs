using UnityEngine;
using System;

public class DemoScript : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Item[] itemsToPickup;
    
    
    public void PickupItem(int id)
    {
        bool result = inventoryManager.AddItem(itemsToPickup[id]);
        Debug.Log(result ? "Item added" : "Item not added");
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
