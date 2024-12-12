using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class HealthBar : MonoBehaviour, IDataPersistence
{
    public Slider healthSlider;
    public Slider easeHealthSlider;
    public float maxHealth = 100f;
    public float health;
    private float lerpSpeed = 0.05f;
    private HealthBar _healthBar;
    private InventoryManager _inventoryManager;

    public event Action HealthChanged;
    
    public void LoadData(GameData data)
    {
        this.health = data.health;
        HealthChanged?.Invoke();
    }

    public void SaveData(GameData data)
    {
        data.health = this.health;
    }

    private void OnEnable()
    {
        HealthChanged += HealthValue;
    }

    private void OnDisable()
    {
        HealthChanged -= HealthValue;
    }

    void Awake()
    {
        _inventoryManager = FindObjectOfType<InventoryManager>();
    }
    void Start()
    {
        if (health <= 0)
        {
            health = maxHealth;
        }
        HealthChanged?.Invoke();
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (healthSlider.value != easeHealthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, health, lerpSpeed);
        }

        if (health <= 0)
        {
            SceneManager.LoadScene("Lobby");
        }
        Heal();
    }

    void HealthValue()
    {
        if (healthSlider.value != health)
        {
            healthSlider.value = health;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        HealthChanged?.Invoke();
    }
    public void TakeHealth(float _health)
    {
        health += _health;
        HealthChanged?.Invoke();
    }
    
    private void Heal()
    {
        // Отримуємо вибраний предмет з InventoryManager
        var selectedItem = _inventoryManager.GetSelectedItem(false);

        // Якщо вибраний предмет існує та він є зброєю, беремо його damage
        if (selectedItem != null && selectedItem.itemType == ItemType.Tool && selectedItem.аctionType == ActionType.Heal && Input.GetKeyDown(KeyCode.E))
        {
            TakeHealth(selectedItem.damage);
        }
    }
}