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
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 3f)) // 3f - максимальная дистанция рейкаста
            {
                if (hit.collider.gameObject == gameObject)
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