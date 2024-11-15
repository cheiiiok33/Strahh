using UnityEngine;

public class DoorInteractionWithRaycast : MonoBehaviour
{
    public Animator doorAnimator;
    private bool isOpen = false;
    private bool playerInTrigger = false;
    public KeyCode interactionKey = KeyCode.E;

    void Start()
    {
        if (doorAnimator == null)
        {
            doorAnimator = GetComponent<Animator>();
        }

        // ��������, ��� ����� ������� ��� ������
        doorAnimator.SetBool("Open", false);
    }

    void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(interactionKey))
        {
            ToggleDoor();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            Debug.Log("����� � ���� �����");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            Debug.Log("����� ����� �� ���� �����");
        }
    }

    private void ToggleDoor()
    {
        isOpen = !isOpen;
        doorAnimator.SetBool("isOpen", isOpen);
        Debug.Log($"������������ �����. isOpen = {isOpen}");
    }
}