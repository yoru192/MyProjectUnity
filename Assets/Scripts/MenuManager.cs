using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Для роботи з UI компонентами
using System.Collections;

public class MenuManager : MonoBehaviour
{
    // Зробимо поле для кнопки Play
    [SerializeField] private Button playButton;

    void Start()
    {
        // Призначаємо функцію кнопці Play
        playButton.onClick.AddListener(OnPlayButtonClicked);
        
    }

    void OnPlayButtonClicked()
    {
        // Запускаємо асинхронне завантаження сцени Lobby
        StartCoroutine(LoadSceneAsync("Lobby"));
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
