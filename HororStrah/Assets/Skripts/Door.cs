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
    private string correctCode = "322";
    private bool isPlayerNearby = false;
    private bool isOpen = false;
    private bool isPanelOpen = false;
    private bool isCodeCorrect = false; // ����� ����
    private Collider doorCollider;

    void Start()
    {
        codePanel.SetActive(false);
        doorCollider = GetComponent<Collider>();

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
            if (isCodeCorrect)
            {
                ToggleDoor();               
            }
            else if (!isOpen)
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
    }

    public void OnNumberButtonPressed(string number)
    {
        if (enteredCode.Length < 3)
        {
            enteredCode += number;
            UpdateCodeDisplay();
            Debug.Log("������ ������: " + number);
        }
    }

    private void UpdateCodeDisplay()
    {
        if (codeDisplayText != null)
        {
            codeDisplayText.text = enteredCode;
            Debug.Log("������� ���: " + enteredCode);
        }
        else
        {
            Debug.LogError("codeDisplayText �� ���������������!");
        }
    }

    public void OnSubmitCode()
    {
        if (enteredCode == correctCode)
        {
            isCodeCorrect = true; // ������������� ����
            ToggleDoor();
            CloseCodePanel();
            
        }
        else
        {
            enteredCode = "";
            UpdateCodeDisplay();
            Debug.Log("�������� ���! ���������� �����.");
        }
    }

    public void OnClearCode()
    {
        enteredCode = "";
        UpdateCodeDisplay();
    }

    private void ToggleDoor()
    {
        isOpen = !isOpen;
        doorAnimator.SetBool("isOpen", isOpen);

        if (doorCollider != null)
        {
            doorCollider.enabled = !isOpen;
        }

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
            playerController.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        isPanelOpen = true;

        Debug.Log("������ �������. ���������� ������� ���������.");
    }

    private void CloseCodePanel()
    {
        codePanel.SetActive(false);
        if (cameraController != null)
        {
            cameraController.enabled = true;
            playerController.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        isPanelOpen = false;
        enteredCode = "";
        UpdateCodeDisplay();
        Debug.Log("������ �������. ���������� ������� ��������.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            Debug.Log("����� ������� � �����, ������� 'E' ��� ��������������.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            Debug.Log("����� ������ �� �����.");
            CloseCodePanel();
        }
    }
}