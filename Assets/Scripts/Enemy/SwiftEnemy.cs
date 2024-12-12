using UnityEngine;
using System;
public class SwiftEnemy : Enemy
{
   
    protected override void Awake()
    {
        base.Awake();
        maxHealth = 1; // Унікальне здоров'я для SwiftEnemy
        _damage = 30;  // Унікальна шкода для SwiftEnemy
    }
    
    public override void Death()
    {
        int amount = UnityEngine.Random.Range(1, 5); // кількість ресурсу

        // Генеруємо число від 0 до 99 (100 значень)
        int randomChance = UnityEngine.Random.Range(0, 100);

        if (randomChance < 40) // 0-39 (40%)
        {
            _resource.AddResource("Wood", amount);
            Debug.Log($"Dropped {amount} Wood");
        }
        else if (randomChance < 80) // 40-79 (40%)
        {
            _resource.AddResource("Stone", amount);
            Debug.Log($"Dropped {amount} Stone");
        }
        else // 80-99 (20%)
        {
            _resource.AddResource("Steel", amount);
            Debug.Log($"Dropped {amount} Steel");
        }
        TriggerEnemyDied();
        // Знищуємо ворога
        Destroy(gameObject);
    }
}
