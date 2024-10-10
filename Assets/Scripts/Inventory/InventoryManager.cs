using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsInHand;
    [SerializeField] private GameObject hand;
    [SerializeField] private ItemType[] objectsTypesInHand;
    
    public int maxStackedItems = 4;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    private int _selectedSlot = -1;
    private readonly Dictionary<ItemType, GameObject> _itemSetActive = new Dictionary<ItemType, GameObject>();
    

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
        if (_selectedSlot >= 0)
        {
            inventorySlots[_selectedSlot].Deselect();
        }

        inventorySlots[newValue].Select();
        _selectedSlot = newValue;
        NewItemSelected();
    }

    public bool AddItem(Item item)
    {
        foreach (var slot in inventorySlots)
        {
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null &&
                itemInSlot.item == item &&
                itemInSlot.count < maxStackedItems &&
                itemInSlot.item.stackable)
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
            
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
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
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
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }

    private void ActivateObject(InventoryItem inventoryItem)
    {
        if (!_itemSetActive.TryGetValue(inventoryItem.item.type, out GameObject activeItem)) return;
        Transform handTransform = hand.transform;
        activeItem.transform.SetParent(handTransform);
        activeItem.transform.localPosition = Vector3.zero;
        activeItem.SetActive(true);
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
        foreach (var item in _itemSetActive.Values)
        {
            item.SetActive(false);
        }

        InventorySlot slot = inventorySlots[_selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

        if (itemInSlot != null)
        {
            ActivateObject(itemInSlot);
        }
    }
}