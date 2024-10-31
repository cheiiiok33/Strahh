using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private LeverController[] levers;

    private bool isDoorOpen = false;
    private static readonly string DOOR_ANIMATION_PARAM = "isOpen";

    private void Start()
    {
        // Проверка наличия компонентов
        if (doorAnimator == null)
        {
            Debug.LogError($"Door Animator не назначен на {gameObject.name}!");
        }

        if (levers == null || levers.Length == 0)
        {
            Debug.LogError($"Рычаги не назначены на {gameObject.name}!");
        }
    }

    private void Update()
    {
        CheckLevers();
    }

    private void CheckLevers()
    {
        if (levers == null) return;

        bool allLeversActivated = true;

        foreach (LeverController lever in levers)
        {
            if (lever == null)
            {
                Debug.LogError("Один из рычагов не назначен!");
                return;
            }

            if (!lever.IsActivated)
            {
                allLeversActivated = false;
                break;
            }
        }

        if (allLeversActivated && !isDoorOpen)
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        isDoorOpen = true;
        if (doorAnimator != null)
        {
            doorAnimator.SetBool(DOOR_ANIMATION_PARAM, true);
            Debug.Log($"Дверь {gameObject.name} открывается!");
        }
    }
}