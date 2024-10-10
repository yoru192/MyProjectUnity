using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Item itemScriptableObject;
    
    public void Equip()
    {
        gameObject.SetActive(true);
    }

    public void Unequip()
    {
        gameObject.SetActive(false);
    }
}
