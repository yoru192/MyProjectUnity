using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable object/Item")]
public class Item : ScriptableObject {
    [Header("Only Gameplay")]
    public float cooldown;
    public ItemType type;
    public ActionType actionType;
    public Vector2Int range = new Vector2Int(5, 4);

    [Header("Only UI")]
    public bool stackable = true;

    [Header("Both")]
    public Sprite image;
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
    Attack
}