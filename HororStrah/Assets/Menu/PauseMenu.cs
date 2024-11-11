using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool PauseGame;
    public GameObject pauseMenu;
    public CameraController cameraController; // Изменено на правильное имя скрипта

    void Start()
    {
        // Автоматически найти скрипт камеры, если он не назначен
        if (cameraController == null)
        {
            cameraController = Camera.main.GetComponent<CameraController>();
        }
        PauseGame = false;
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Current PauseGame state: " + PauseGame);
            if (PauseGame)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        PauseGame = false;
        Debug.Log("Resume - PauseGame: " + PauseGame);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (cameraController != null)
        {
            cameraController.enabled = true;
            Debug.Log("Camera enabled: " + cameraController.enabled);
        }
        else
        {
            Debug.LogWarning("Camera Controller не найден!");
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        PauseGame = true;
        Debug.Log("Pause - PauseGame: " + PauseGame);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (cameraController != null)
        {
            cameraController.enabled = false;
            Debug.Log("Camera disabled: " + cameraController.enabled);
        }
        else
        {
            Debug.LogWarning("Camera Controller не найден!");
        }
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}