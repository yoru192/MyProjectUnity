using System.Collections;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using TMPro;

public class WaveManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int[] enemiesPerWave;
    [SerializeField] private TextMeshProUGUI waveText;

    private int currentWave = 0;
    
    public event Action WavesChanged;

    public void SaveData(GameData data)
    {
        data.waves = this.currentWave;
    }

    public void LoadData(GameData data)
    {
        this.currentWave = data.waves;
        WavesChanged?.Invoke();
    }

    void Start()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned!");
            return;
        }

        if (enemiesPerWave == null || enemiesPerWave.Length == 0)
        {
            Debug.LogError("No wave data assigned!");
            return;
        }

        StartCoroutine(StartWave());
    }
    private void UpdateWaveUI()
    {
        if (waveText != null)
        {
            waveText.text = $"Wave {currentWave + 1}";
        }
    }
    IEnumerator StartWave()
    {
        if (currentWave >= enemiesPerWave.Length)
        {
            Debug.Log("Game Over. All waves completed!");
            yield break;
        }

        int enemiesToSpawn = enemiesPerWave[currentWave];
        Debug.Log($"Wave {currentWave + 1} started with {enemiesToSpawn} enemies.");
        UpdateWaveUI();

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(1f); // Затримка між спавнами ворогів
        }

        Debug.Log($"Enemies in the scene: {GameObject.FindGameObjectsWithTag("Enemy").Length}");

        // Чекаємо, поки всі вороги будуть знищені
        yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 1);
        Debug.Log("Wave completed.");
        currentWave++;
        WavesChanged?.Invoke();
        SceneTimer.EndWave();
        //StartCoroutine(StartWave());
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy prefab is not assigned in the inspector!");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned!");
            return;
        }

        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(enemyPrefab, spawnPoints[spawnIndex].position, Quaternion.identity);
        Debug.Log($"Enemy spawned at: {spawnPoints[spawnIndex].position}");
    }
}
