using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TriggerSceneLoader : MonoBehaviour
{
    // Ім'я сцени, яку потрібно завантажити
    public string sceneToLoad = "Waves";

    // Спрацьовує, коли інший об'єкт входить у зону тригера
    private void OnTriggerEnter(Collider other)
    {
        // Перевіряємо, чи це гравець (або інший об'єкт)
        if (other.CompareTag("Player"))
        {
            // Спершу зберігаємо поточний стан гри
            DataPersistenceManager.instance.SaveGame();
            
            // Запускаємо асинхронне завантаження сцени
            StartCoroutine(LoadSceneAsync(sceneToLoad));
        }
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (operation != null && !operation.isDone)
        {
            // Тут можна додати прогрес-бар або інші дії
            yield return null;
        }
    }
}