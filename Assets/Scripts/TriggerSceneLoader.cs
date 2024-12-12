using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TriggerSceneLoader : MonoBehaviour
{
    private string sceneToLoad = "Waves";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DataPersistenceManager.instance.SaveGame(); // Зберігаємо гру
            StartCoroutine(LoadSceneAsync(sceneToLoad));
        }
    }

    public static IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (operation != null && !operation.isDone)
        {
            yield return null; // Очікуємо завершення завантаження
        }
    }
}