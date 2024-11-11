using UnityEngine;
using TMPro;

public class DoorInteractionWithButtons : MonoBehaviour
{
    public Animator doorAnimator;
    [SerializeField] private GameObject codePanel;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private TextMeshProUGUI codeDisplayText;
    [SerializeField] private PlayerController playerController;

    private string enteredCode = "";
    private const string correctCode = "322";
    private bool isPlayerNearby = false;
    private bool isOpen = false;
    private bool isPanelOpen = false;
    private bool isCodeCorrect = false;

    void Start()
    {
        codePanel.SetActive(false);
        if (doorAnimator == null)
        {
            doorAnimator = GetComponent<Animator>();
        }
        if (codeDisplayText != null)
        {
            codeDisplayText.text = "";
        }
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (isOpen || isCodeCorrect)
            {
                ToggleDoor();
            }
            else
            {
                ToggleCodePanel();
            }
        }
    }

    public void OnNumberButtonPressed(string number)
    {
        if (enteredCode.Length < 3)
        {
            enteredCode += number;
            UpdateCodeDisplay();
        }
    }

    private void UpdateCodeDisplay()
    {
        if (codeDisplayText != null)
        {
            codeDisplayText.text = enteredCode;
        }
    }

    public void OnSubmitCode()
    {
        if (enteredCode == correctCode)
        {
            isCodeCorrect = true;
            CloseCodePanel();
        }
        else
        {
            OnClearCode();
        }
    }

    public void OnClearCode()
    {
        enteredCode = "";
        UpdateCodeDisplay();
    }

    private void ToggleDoor()
    {
        if (!isPlayerNearby) return;

        isOpen = !isOpen;
        doorAnimator.SetBool("isOpen", isOpen);

        if (isOpen)
        {
            Debug.Log("Дверь открыта.");
        }
        else
        {
            Debug.Log("Дверь закрыта.");
        }
    }

    private void ToggleCodePanel()
    {
        if (isPanelOpen)
        {
            CloseCodePanel();
        }
        else
        {
            OpenCodePanel();
        }
    }

    private void OpenCodePanel()
    {
        codePanel.SetActive(true);
        SetPlayerControl(false);
        isPanelOpen = true;
    }

    private void CloseCodePanel()
    {
        codePanel.SetActive(false);
        SetPlayerControl(true);
        isPanelOpen = false;
        OnClearCode();
    }

    private void SetPlayerControl(bool enabled)
    {
        if (cameraController != null)
        {
            cameraController.enabled = enabled;
            playerController.enabled = enabled;
            Cursor.lockState = enabled ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !enabled;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            CloseCodePanel();
        }
    }
}