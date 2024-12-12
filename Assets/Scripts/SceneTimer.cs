using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class SceneTimer : MonoBehaviour
{
    public float timerDuration = 30f; // Тривалість хвилі
    private static string sceneToLoad = "Lobby"; // Повернення до Lobby

    [SerializeField] private TextMeshProUGUI timerText;

    private float currentTime;

    void Start()
    {
        currentTime = timerDuration; // Запускаємо таймер при старті
        StartCoroutine(StartTimer());
    }

    private IEnumerator StartTimer()
    {
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerUI();
            yield return null;
        }

        EndWave();
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = currentTime.ToString("F2") + " сек";
        }
    }

    public static void EndWave()
    {
        // Повернення до Lobby після завершення хвилі
        DataPersistenceManager.instance.SaveGame();
        SceneManager.LoadScene(sceneToLoad);
    }
}