using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private LeverController[] levers;
    [SerializeField] private bool debugMode = true; // ��������� ��� �������

    private bool isDoorOpen = false;
    private static readonly string DOOR_ANIMATION_PARAM = "isOpen";

    private void Start()
    {
        // �������� ������� �����������
        if (doorAnimator == null)
        {
            Debug.LogError($"Door Animator �� �������� �� {gameObject.name}!");
            return;
        }

        if (levers == null || levers.Length == 0)
        {
            Debug.LogError($"������ �� ��������� �� {gameObject.name}!");
            return;
        }

        // ������� ��������� ���������
        if (debugMode)
        {
            Debug.Log($"����� {gameObject.name} ����������������. ���������� �������: {levers.Length}");
        }
    }

    private void Update()
    {
        if (!isDoorOpen) // ��������� ������ ���� ����� ��� �� �������
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
                Debug.LogError($"���� �� ������� �� �������� ��� ����� {gameObject.name}!");
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
            Debug.Log($"������������ �������: {activatedCount}/{levers.Length}");
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
            Debug.Log($"����� {gameObject.name} �����������!");
        }
    }

    // ��������� ����� ��� �������� ��������� ����� �� ������ ��������
    public bool IsDoorOpen()
    {
        return isDoorOpen;
    }
}