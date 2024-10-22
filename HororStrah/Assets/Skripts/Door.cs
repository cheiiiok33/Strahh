using UnityEngine;

public class DoorInteractionWithButtons : MonoBehaviour
{
    public Animator doorAnimator;   // Аниматор двери
    [SerializeField] private GameObject codePanel;    // Панель с кнопками для ввода кода
    [SerializeField] private CameraController cameraController;  // Ссылка на скрипт управления камерой
    private string enteredCode = "";  // Введённый игроком код
    private string correctCode = "322";  // Правильный код для открытия двери
    private bool isPlayerNearby = false;  // Флаг для проверки, рядом ли игрок с дверью
    private bool isOpen = false;  // Состояние двери (открыта или закрыта)
    private bool isPanelOpen = false;  // Флаг состояния панели
    private Collider doorCollider;  // Коллайдер двери

    void Start()
    {
        // Убедитесь, что панель изначально скрыта
        codePanel.SetActive(false);

        // Получаем коллайдер двери
        doorCollider = GetComponent<Collider>();

        if (doorAnimator == null)
        {
            doorAnimator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        // Если игрок рядом и нажимает "E", показываем или скрываем панель
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (!isPanelOpen)
            {
                OpenCodePanel();  // Показываем панель и отключаем управление камерой
            }
            else
            {
                CloseCodePanel();  // Закрываем панель и включаем управление камерой
            }
        }
    }

    // Метод для обработки нажатия на цифровые кнопки
    public void OnNumberButtonPressed(string number)
    {
        if (enteredCode.Length < 3)
        {
            enteredCode += number;  // Добавляем цифру в введённый код
        }
    }

    // Метод для подтверждения кода (кнопка "ОК")
    public void OnSubmitCode()
    {
        if (enteredCode == correctCode)
        {
            ToggleDoor();  // Открываем дверь
            CloseCodePanel();  // Скрываем панель и включаем управление камерой
        }
        else
        {
            enteredCode = "";  // Если код неправильный, сбрасываем ввод
            Debug.Log("Неверный код! Попробуйте снова.");
        }
    }

    // Метод для очистки кода (если кнопка "Сброс" нужна)
    public void OnClearCode()
    {
        enteredCode = "";  // Сбрасываем введённый код
    }

    // Открытие или закрытие двери
    private void ToggleDoor()
    {
        isOpen = !isOpen;
        doorAnimator.SetBool("isOpen", isOpen);

        // Отключаем коллайдер, когда дверь открыта
        if (doorCollider != null)
        {
            doorCollider.enabled = !isOpen;  // Включаем коллайдер, когда дверь закрыта
        }
    }

    // Открытие панели и отключение управления камерой
    private void OpenCodePanel()
    {
        codePanel.SetActive(true);  // Показываем панель
        if (cameraController != null)
        {
            cameraController.enabled = false;  // Отключаем управление камерой
        }
        isPanelOpen = true;  // Панель открыта
        Debug.Log("Панель открыта. Управление камерой отключено.");
    }

    // Закрытие панели и включение управления камерой
    private void CloseCodePanel()
    {
        codePanel.SetActive(false);  // Скрываем панель
        if (cameraController != null)
        {
            cameraController.enabled = true;  // Включаем управление камерой
        }
        isPanelOpen = false;  // Панель закрыта
        Debug.Log("Панель закрыта. Управление камерой включено.");
    }

    // Определяем, когда игрок входит в зону триггера (возле двери)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;  // Игрок рядом с дверью
            Debug.Log("Игрок подошёл к двери, нажмите 'E' для взаимодействия.");
        }
    }

    // Определяем, когда игрок выходит из зоны триггера
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;  // Игрок отошёл от двери
            Debug.Log("Игрок отошёл от двери.");
            CloseCodePanel();  // Автоматически закрываем панель и включаем управление камерой, если игрок уходит
        }
    }
}
