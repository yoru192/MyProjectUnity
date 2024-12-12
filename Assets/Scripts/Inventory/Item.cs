using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName ="Scriptable object/Item")]
public class Item : ScriptableObject {
    [Header("Only Gameplay")]
    public ItemType itemType;
    public ActionType аctionType;
    public int damage;

    [Header("Only UI")]
    public bool stackable = true;

    [Header("Both")]
    public Sprite image;
    
    [Header("Shop Settings")]
    [Tooltip("The price of the item in the shop.")]
    public int price;
    public int woodPrice;
    public int stonePrice;
    public int steelPrice;
}

public enum ItemType
{
    BuildingBlock,
    Tool,
    Weapon
}

public enum ActionType
{
    Dig,
    Mine,
    Attack,
    Heal
}