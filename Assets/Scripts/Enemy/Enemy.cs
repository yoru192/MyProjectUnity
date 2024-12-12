using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] protected Resource _resource;
    protected int _currentHealth;
    public int maxHealth;
    protected int _damage = 20;

    // Подія смерті для всіх ворогів
    public static event Action<int> AnyEnemyDied;
    
    // Публічний метод для виклику події
    protected void TriggerEnemyDied()
    {
        AnyEnemyDied?.Invoke(maxHealth);
    }

    protected virtual void Awake()
    {
        if (_wallet == null)
        {
            _wallet = FindObjectOfType<Wallet>();
        }

        if (_healthBar == null)
        {
            _healthBar = FindObjectOfType<HealthBar>();
        }

        if (_resource == null)
        {
            _resource = FindObjectOfType<Resource>();
        }
        _currentHealth = maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _healthBar.TakeDamage(_damage);
        }
    }

    public virtual void TakeDamage(int amount)
    {
        _currentHealth -= amount;

        if (_currentHealth <= 0)
        {
            Death();
        }
    }

    public virtual void Death()
    {
        // Генеруємо число від 0 до 99 (100 значень)
        int randomChance = UnityEngine.Random.Range(0, 100);

        if (randomChance < 45) // 0-59 (45%)
        {
            int amount = UnityEngine.Random.Range(3, 5);
            _resource.AddResource("Wood", amount);
            Debug.Log($"Dropped {amount} Wood");
        }
        else if (randomChance < 80) // 60-89 (35%)
        {
            int amount = UnityEngine.Random.Range(2, 6);
            _resource.AddResource("Stone", amount);
            Debug.Log($"Dropped {amount} Stone");
        }
        else // 90-99 (20%)
        {
            int amount = UnityEngine.Random.Range(1, 5);
            _resource.AddResource("Steel", amount);
            Debug.Log($"Dropped {amount} Steel");
        }
        
        AnyEnemyDied?.Invoke(maxHealth);
        // Знищуємо ворога
        Destroy(gameObject);
    }

}
