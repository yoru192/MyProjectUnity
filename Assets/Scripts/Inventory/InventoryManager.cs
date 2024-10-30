using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsInHand;
    [SerializeField] private GameObject hand;
    [SerializeField] private ItemType[] objectsTypesInHand;
    
    public int maxStackedItems = 4;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    private int _selectedSlot = -1;
    //private readonly Dictionary<ItemType, GameObject> _itemSetActive = new Dictionary<ItemType, GameObject>();
    
    [Tooltip("Reference to the InputController attached to this player")]
    [SerializeField]
    private InputController _inputController;
    public InputController inputController
    {
        get { return _inputController; }
    }
    /// <summary>Equipped weapon</summary>
    private Weapon _weapon;
    /// <summary>Local controller for the player actions</summary>
    private ActionController _actionController;
    public ActionController actionController
    {
        get { return _actionController; }
    }
    
    [Tooltip("Bone we attach weapons to")]
    [SerializeField]
    private GameObject _weaponSlot;
    
    private void Start()
    {
        ChangeSelectedSlot(0);
        // for (int i = 0; i < objectsInHand.Length; i++)
        // {
        //     _itemSetActive.Add(objectsTypesInHand[i], objectsInHand[i]);
        // }
        Equip(EWeaponId.Sword);
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
        
        // Push the main weapon action if ActionController is empty
        if (!_actionController.HasAction && _weapon != null && inputController.IsPressed(EInput.Attack))
        {
            _actionController.Play(_weapon.data.actionId);
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

    // private void ActivateObject(InventoryItem inventoryItem)
    // {
    //     if (!_itemSetActive.TryGetValue(inventoryItem.item.type, out GameObject activeItem)) return;
    //     Transform handTransform = hand.transform;
    //     activeItem.transform.SetParent(handTransform);
    //     activeItem.transform.localPosition = Vector3.zero;
    //     activeItem.SetActive(true);
    // }

    public void Equip(EWeaponId weaponId)
    {
        // Check not already held
        if (_weaponSlot == null || (_weapon != null && weaponId == _weapon.id))
        { 
            return;
        }

        // Check registered
        WeaponData data = GameInstance.singleton.weaponBank.Get(weaponId);
        if (data == null || data.equipPrefab == null)
        {
            throw new Exception($"No data registered for weapon {weaponId}");
        }

        if (_weapon != null)
        {
            // Destroy old weapon
            Destroy(_weapon.gameObject);
        }

        // Instantiate new weapon
        _weapon = Instantiate(data.equipPrefab, _weaponSlot.transform).GetComponent<Weapon>();
        _weapon.Init(data);
        _weapon.transform.localPosition = Vector3.zero;
        _weapon.transform.localRotation = Quaternion.identity;

        GameEvents.WeaponEquipped(weaponId);
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
        // foreach (var item in _itemSetActive.Values)
        // {
        //     item.SetActive(false);
        // }

        InventorySlot slot = inventorySlots[_selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

        if (itemInSlot != null)
        {
            //ActivateObject(itemInSlot);
        }
    }
}
}
