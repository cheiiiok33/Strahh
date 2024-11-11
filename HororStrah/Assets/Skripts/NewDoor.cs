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

        // Убедимся, что дверь закрыта при старте
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
            Debug.Log("Игрок в зоне двери");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            Debug.Log("Игрок вышел из зоны двери");
        }
    }

    private void ToggleDoor()
    {
        isOpen = !isOpen;
        doorAnimator.SetBool("isOpen", isOpen);
        Debug.Log($"Переключение двери. isOpen = {isOpen}");
    }
}