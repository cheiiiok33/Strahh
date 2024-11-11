using UnityEngine;

public class GameEndItem : MonoBehaviour
{
    public GameObject endGamePanel; // Панель с картинкой
    private bool isInRange = false; // Игрок в зоне взаимодействия

    void Start()
    {
        // Убедимся, что панель скрыта при старте
        if (endGamePanel != null)
        {
            endGamePanel.SetActive(false);
        }
    }

    void Update()
    {
        // Проверяем, находится ли игрок рядом и нажал ли E
        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            EndGame();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, вошел ли игрок в зону взаимодействия
        if (other.CompareTag("Player"))
        {
            isInRange = true;
            Debug.Log("Игрок может взаимодействовать с предметом");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Проверяем, вышел ли игрок из зоны взаимодействия
        if (other.CompareTag("Player"))
        {
            isInRange = false;
        }
    }

    void EndGame()
    {
        // Показываем панель с картинкой
        endGamePanel.SetActive(true);

        // Останавливаем время
        Time.timeScale = 0f;

        // Показываем курсор
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Отключаем управление игроком (если есть)
        var playerCamera = FindObjectOfType<CameraController>();
        if (playerCamera != null)
        {
            playerCamera.enabled = false;
        }
    }
}