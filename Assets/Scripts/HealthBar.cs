using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HealthBar : MonoBehaviour, IDataPersistence
{
    public Slider healthSlider;
    public Slider easeHealthSlider;
    public float maxHealth = 100f;
    public float health;
    private float lerpSpeed = 0.05f;
    private HealthBar _healthBar;

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

    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        HealthValue();
    }

    void HealthValue()
    {
        if (healthSlider.value != health)
        {
            healthSlider.value = health;
            Debug.Log("Health changed to: " + health);
            HealthChanged?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
        }

        if (healthSlider.value != easeHealthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, health, lerpSpeed);
        }
    }

    void TakeDamage(float damage)
    {
        health -= damage;
    }
}
