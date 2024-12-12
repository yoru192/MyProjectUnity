using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] private GameObject[] objectsInHand;
    [SerializeField] private GameObject hand;
    [SerializeField] private ItemType[] objectsTypesInHand;
    
    public int maxStackedItems = 4;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    [NonSerialized] public InventoryItem inventoryItemInSlot;
    private InventoryItem _inventoryItem;
    
    private int _selectedSlot = -1;
    private Dictionary<ItemType, GameObject> _itemSetActive = new Dictionary<ItemType, GameObject>();
    private bool _itemInSlotActive;

    
    public void SaveData(GameData data)
    {
        data.DataInventoryItems.Clear();
    
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            var slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot is not null)
            {
                data.DataInventoryItems.Add(new ItemData 
                { 
                    item = itemInSlot.item, 
                    count = itemInSlot.count, 
                    selectedSlot = i
                });
            }
        }
    }

    public void LoadData(GameData data)
    {
        foreach (var slot in inventorySlots)
        {
            InventoryItem existingItem = slot.GetComponentInChildren<InventoryItem>();
            if (existingItem != null)
            {
                Destroy(existingItem.gameObject);
            }
        }

        foreach (var itemData in data.DataInventoryItems)
        {
            if (itemData.selectedSlot >= 0 && itemData.selectedSlot < inventorySlots.Length)
            {
                InventorySlot targetSlot = inventorySlots[itemData.selectedSlot];
                SpawnNewItem(itemData.item, targetSlot);
                InventoryItem newItem = targetSlot.GetComponentInChildren<InventoryItem>();
                newItem.count = itemData.count;
                newItem.RefreshCount();
            }
        }
    }



    
    private void Start()
    {
        ChangeSelectedSlot(0);
        for (int i = 0; i < objectsInHand.Length; i++)
        {
            _itemSetActive.Add(objectsTypesInHand[i], objectsInHand[i]);
        }
    }

    private void Update()
    {
        if (!string.IsNullOrEmpty(Input.inputString))
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number <= inventorySlots.Length)
            {
                ChangeSelectedSlot(number - 1);
            }
        }
    }

    void ChangeSelectedSlot(int newValue)
    {
        if (newValue < 0 || newValue >= inventorySlots.Length)
        {
            Debug.LogWarning("Selected slot index is out of bounds");
            return;
        }
        if (_selectedSlot >= 0)
        {
            inventorySlots[_selectedSlot].Deselect();
        }
        inventorySlots[newValue].Select();
        _selectedSlot = newValue;
        NewItemSelected();
    }

    void OnDestroy()
    {
        // Очищення посилань на знищені слоти
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i] != null)
            {
                Destroy(inventorySlots[i].gameObject); // Видаляємо об'єкти
                inventorySlots[i] = null;             // Очищаємо посилання
            }
        }
    }
    
    public bool AddItem(Item item)
    {
        foreach (var slot in inventorySlots)
        {
            inventoryItemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (inventoryItemInSlot != null &&
                inventoryItemInSlot.item == item &&
                inventoryItemInSlot.count < maxStackedItems &&
                inventoryItemInSlot.item.stackable)
            {
                inventoryItemInSlot.count++;
                inventoryItemInSlot.RefreshCount();
            
                // Оновлюємо предмет в руці, якщо доданий предмет знаходиться в вибраному слоті
                if (slot == inventorySlots[_selectedSlot])
                {
                    NewItemSelected();
                }
                return true;
            }
        }

        foreach (var slot in inventorySlots)
        {
            inventoryItemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (inventoryItemInSlot == null)
            {
                SpawnNewItem(item, slot);
            
                // Оновлюємо предмет в руці, якщо новий предмет додано в вибраний слот
                if (slot == inventorySlots[_selectedSlot])
                {
                    NewItemSelected();
                }
                return true;
            }
        }
        return false;
    }


    private void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        inventoryItemInSlot = newItemGo.GetComponent<InventoryItem>();
        inventoryItemInSlot.InitialiseItem(item);
    }

    private void ActivateObject(InventoryItem inventoryItem)
    {
        if (!_itemSetActive.TryGetValue(inventoryItem.item.itemType, out GameObject activeItem)) return;
        Transform handTransform = hand.transform;
        _itemInSlotActive = true;
        activeItem.transform.SetParent(handTransform);
        activeItem.transform.localPosition = Vector3.zero;
        activeItem.SetActive(_itemInSlotActive);
    }

    public Item GetSelectedItem(bool use)
    {
        InventorySlot slot = inventorySlots[_selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot == null) return null;
        if (!use) return itemInSlot.item;

        itemInSlot.count--;
        if (itemInSlot.count <= 0)
        {
            Destroy(itemInSlot.gameObject);
        }
        else
        {
            itemInSlot.RefreshCount();
        }
        return itemInSlot.item;
    }

    private void NewItemSelected()
    {
        if (_selectedSlot < 0 || _selectedSlot >= inventorySlots.Length) return;
        foreach (GameObject item in _itemSetActive.Values)
        {
            _itemInSlotActive = false;
            item.SetActive(_itemInSlotActive);
        }

        InventorySlot slot = inventorySlots[_selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

        if (itemInSlot != null)
        {
            ActivateObject(itemInSlot);
        }
    }
}