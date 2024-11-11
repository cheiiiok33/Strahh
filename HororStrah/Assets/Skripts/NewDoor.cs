using UnityEngine;

public class DoorInteractionWithRaycast : MonoBehaviour
{
    public Animator doorAnimator;
    [SerializeField] private Camera playerCamera;
    private bool isOpen = false;

    void Start()
    {
        if (doorAnimator == null)
        {
            doorAnimator = GetComponent<Animator>();
        }

        if (playerCamera == null)
        {
            Debug.LogError("Player camera is not assigned!");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerCamera != null)
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 3f)) // 3f - максимальная дистанция рейкаста
            {
                if (hit.collider.CompareTag("InteractableDoor"))
                {
                    ToggleDoor();
                }
            }
        }
    }

    private void ToggleDoor()
    {
        isOpen = !isOpen;
        doorAnimator.SetBool("isOpen", isOpen);
    }
}