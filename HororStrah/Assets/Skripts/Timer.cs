using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TimerController : MonoBehaviour
{
    [Header("Timer Settings")]
    [SerializeField] private float timeRemaining = 300f;
    [SerializeField] private bool timerIsRunning = false;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Scene Settings")]
    [SerializeField] private string mainSceneName = "SampleScene";

    [Header("Camera Reference")]
    [SerializeField] private Camera mainCamera;

    private void Start()
    {
        InitializeTimer();
    }

    private void InitializeTimer()
    {
        timerIsRunning = true;
        Time.timeScale = 1f;

        // Скрываем панель геймовера
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("GameOverPanel не назначен!");
        }

        // Проверяем текст таймера
        if (timeText == null)
        {
            Debug.LogError("Не назначен компонент timeText!");
            enabled = false;
            return;
        }

    }

    private void Update()
    {
        UpdateTimer();
        CheckForRestartInput();
    }

    private void UpdateTimer()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Время вышло!");
                timeRemaining = 0;
                timerIsRunning = false;
                GameOver();
            }
        }
    }

    private void CheckForRestartInput()
    {
        if (!timerIsRunning && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    private void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void AddTime(float timeToAdd = 60f)
    {
        timeRemaining += timeToAdd;
        Debug.Log($"Добавлено {timeToAdd} секунд. Осталось времени: {timeRemaining}");
    }

    public void GameOver()
    {
        timeRemaining = 0;
        timerIsRunning = false;
        Time.timeScale = 0f;

        // Отключаем скрипт управления камерой
        if (mainCamera != null)
        {
            var cameraScript = mainCamera.GetComponent<CameraController>();
            if (cameraScript != null)
            {
                cameraScript.enabled = false;
            }
        }

        // Показываем панель геймовера
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // Разблокируем и показываем курсор
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainSceneName);
    }

    public void PauseGame()
    {
        timerIsRunning = false;
        Time.timeScale = 0f;

        // Разблокируем и показываем курсор
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        timerIsRunning = true;
        Time.timeScale = 1f;

        // Блокируем и скрываем курсор
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}