using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool PauseGame; // ����������� ���������� ��� ������������ ��������� �����
    public GameObject pauseMenu; // ������ ���� �����
    public MonoBehaviour cameraController; // ������ �� ������ ���������� �������

    void Update()
    {
        // �������� ������� ������� Escape ��� �����/������������� ����
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
        pauseMenu.SetActive(false); // ������ ���� �����
        Time.timeScale = 1f; // ����������� �����
        PauseGame = false; // �������� ��������� ����� �� false

        // �������� ���������� �������
        if (cameraController != null)
        {
            cameraController.enabled = true; // �������� ������ ���������� �������
        }
        else
        {
            Debug.LogWarning("Camera controller is not assigned!"); // ��������������, ���� ������ �� ������
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true); // �������� ���� �����
        Time.timeScale = 0f; // ���������� �����
        PauseGame = true; // �������� ��������� ����� �� true

        // ��������� ���������� �������
        if (cameraController != null)
        {
            cameraController.enabled = false; // ��������� ������ ���������� �������
        }
        else
        {
            Debug.LogWarning("Camera controller is not assigned!"); // ��������������, ���� ������ �� ������
        }
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f; // ����������� ����� ����� ��������� ����
        SceneManager.LoadScene("Menu"); // ��������� ����� ����
    }
}
