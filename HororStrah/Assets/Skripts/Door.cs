using UnityEngine;

public class DoorInteractionWithButtons : MonoBehaviour
{
    public Animator doorAnimator;
    [SerializeField] private GameObject codePanel;
    [SerializeField] private CameraController cameraController;
    private string enteredCode = "";
    private string correctCode = "322";
    private bool isPlayerNearby = false;
    private bool isOpen = false;
    private bool isPanelOpen = false;
    private Collider doorCollider;

    void Start()
    {
        codePanel.SetActive(false);
        doorCollider = GetComponent<Collider>();

        if (doorAnimator == null)
        {
            doorAnimator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        // Добавлена проверка !isOpen
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && !isOpen)
        {
            if (!isPanelOpen)
            {
                OpenCodePanel();
            }
            else
            {
                CloseCodePanel();
            }
        }
    }

    public void OnNumberButtonPressed(string number)
    {
        if (enteredCode.Length < 3)
        {
            enteredCode += number;
        }
    }

    public void OnSubmitCode()
    {
        if (enteredCode == correctCode)
        {
            ToggleDoor();
            CloseCodePanel();
        }
        else
        {
            enteredCode = "";
            Debug.Log("Неверный код! Попробуйте снова.");
        }
    }

    public void OnClearCode()
    {
        enteredCode = "";
    }

    private void ToggleDoor()
    {
        isOpen = !isOpen;
        doorAnimator.SetBool("isOpen", isOpen);

        if (doorCollider != null)
        {
            doorCollider.enabled = !isOpen;
        }

        // Автоматически закрываем панель при открытии двери
        if (isOpen)
        {
            CloseCodePanel();
        }
    }

    private void OpenCodePanel()
    {
        codePanel.SetActive(true);
        if (cameraController != null)
        {
            cameraController.enabled = false;
        }
        isPanelOpen = true;
        Debug.Log("Панель открыта. Управление камерой отключено.");
    }

    private void CloseCodePanel()
    {
        codePanel.SetActive(false);
        if (cameraController != null)
        {
            cameraController.enabled = true;
        }
        isPanelOpen = false;
        enteredCode = ""; // Очищаем введенный код при закрытии панели
        Debug.Log("Панель закрыта. Управление камерой включено.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("Игрок подошёл к двери, нажмите 'E' для взаимодействия.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            Debug.Log("Игрок отошёл от двери.");
            CloseCodePanel();
        }
    }
}