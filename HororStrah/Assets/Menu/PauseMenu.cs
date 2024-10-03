using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool PauseGame; // Статическая переменная для отслеживания состояния паузы
    public GameObject pauseMenu; // Объект меню паузы
    public MonoBehaviour cameraController; // Ссылка на скрипт управления камерой

    void Update()
    {
        // Проверка нажатия клавиши Escape для паузы/возобновления игры
        if (Input.GetKeyDown(KeyCode.Escape))
        {
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
        pauseMenu.SetActive(false); // Скрыть меню паузы
        Time.timeScale = 1f; // Возобновить время
        PauseGame = false; // Изменить состояние паузы на false

        // Включить управление камерой
        if (cameraController != null)
        {
            cameraController.enabled = true; // Включить скрипт управления камерой
        }
        else
        {
            Debug.LogWarning("Camera controller is not assigned!"); // Предупреждение, если ссылка не задана
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true); // Показать меню паузы
        Time.timeScale = 0f; // Остановить время
        PauseGame = true; // Изменить состояние паузы на true

        // Отключить управление камерой
        if (cameraController != null)
        {
            cameraController.enabled = false; // Отключить скрипт управления камерой
        }
        else
        {
            Debug.LogWarning("Camera controller is not assigned!"); // Предупреждение, если ссылка не задана
        }
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f; // Возобновить время перед загрузкой меню
        SceneManager.LoadScene("Menu"); // Загрузить сцену меню
    }
}
