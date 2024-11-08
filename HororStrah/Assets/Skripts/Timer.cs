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

        // �������� ������ ���������
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("GameOverPanel �� ��������!");
        }

        // ��������� ����� �������
        if (timeText == null)
        {
            Debug.LogError("�� �������� ��������� timeText!");
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
                Debug.Log("����� �����!");
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
        Debug.Log($"��������� {timeToAdd} ������. �������� �������: {timeRemaining}");
    }

    public void GameOver()
    {
        timeRemaining = 0;
        timerIsRunning = false;
        Time.timeScale = 0f;

        // ��������� ������ ���������� �������
        if (mainCamera != null)
        {
            var cameraScript = mainCamera.GetComponent<CameraController>();
            if (cameraScript != null)
            {
                cameraScript.enabled = false;
            }
        }

        // ���������� ������ ���������
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // ������������ � ���������� ������
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

        // ������������ � ���������� ������
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        timerIsRunning = true;
        Time.timeScale = 1f;

        // ��������� � �������� ������
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}