using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private LeverController[] levers;
    [SerializeField] private bool debugMode = true; // Добавляем для отладки

    private bool isDoorOpen = false;
    private static readonly string DOOR_ANIMATION_PARAM = "isOpen";

    private void Start()
    {
        // Проверка наличия компонентов
        if (doorAnimator == null)
        {
            Debug.LogError($"Door Animator не назначен на {gameObject.name}!");
            return;
        }

        if (levers == null || levers.Length == 0)
        {
            Debug.LogError($"Рычаги не назначены на {gameObject.name}!");
            return;
        }

        // Выводим начальное состояние
        if (debugMode)
        {
            Debug.Log($"Дверь {gameObject.name} инициализирована. Количество рычагов: {levers.Length}");
        }
    }

    private void Update()
    {
        if (!isDoorOpen) // Проверяем только если дверь еще не открыта
        {
            CheckLevers();
        }
    }

    private void CheckLevers()
    {
        if (levers == null) return;

        bool allLeversActivated = true;
        int activatedCount = 0;

        foreach (LeverController lever in levers)
        {
            if (lever == null)
            {
                Debug.LogError($"Один из рычагов не назначен для двери {gameObject.name}!");
                return;
            }

            if (lever.IsActivated)
            {
                activatedCount++;
            }
            else
            {
                allLeversActivated = false;
            }
        }

        if (debugMode)
        {
            Debug.Log($"Активировано рычагов: {activatedCount}/{levers.Length}");
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

    // Добавляем метод для проверки состояния двери из других скриптов
    public bool IsDoorOpen()
    {
        return isDoorOpen;
    }
}