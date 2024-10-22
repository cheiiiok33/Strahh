using UnityEngine;

public class LeverDoorController : MonoBehaviour
{
    // ������ �������
    public Lever[] levers;

    // ������ �� �����
    public GameObject door;

    
    private bool doorOpened = false;

    void Update()
    {
        
        if (doorOpened)
            return;

         
        if (AllLeversDown())
        {
            OpenDoor();
        }
    }

    // ����� ��� �������� ��������� ���� �������
    private bool AllLeversDown()
    {
        foreach (Lever lever in levers)
        {
            if (!lever.isLeverDown)
            {
                return false; // ���� ���� �� ���� ����� �� ������
            }
        }
        return true; // ��� ������ �������
    }

    // ����� ��� �������� �����
    private void OpenDoor()
    {
        doorOpened = true;
        // ����� ����� �������� �������� �������� ����� ��� ������ �������
        Debug.Log("����� �������!");
        // ��������, ���������� ����� �����
        door.transform.position += new Vector3(0, 5, 0); // ������� ����� �������� �� �����
    }
}

// ������ ��� ������
public class Lever : MonoBehaviour
{
    public bool isLeverDown = false; // ��������� ������

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            // ����������� ��������� ������ ��� ��������������
            isLeverDown = !isLeverDown;
            Debug.Log("����� " + (isLeverDown ? "������" : "������"));
            // ����� ����� �������� �������� �������� ������
        }
    }
}
