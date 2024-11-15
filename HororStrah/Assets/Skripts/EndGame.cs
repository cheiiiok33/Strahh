using UnityEngine;

public class GameEndItem : MonoBehaviour
{
    public GameObject endGamePanel; // ������ � ���������
    private bool isInRange = false; // ����� � ���� ��������������

    void Start()
    {
        // ��������, ��� ������ ������ ��� ������
        if (endGamePanel != null)
        {
            endGamePanel.SetActive(false);
        }
    }

    void Update()
    {
        // ���������, ��������� �� ����� ����� � ����� �� E
        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            EndGame();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // ���������, ����� �� ����� � ���� ��������������
        if (other.CompareTag("Player"))
        {
            isInRange = true;
            Debug.Log("����� ����� ����������������� � ���������");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // ���������, ����� �� ����� �� ���� ��������������
        if (other.CompareTag("Player"))
        {
            isInRange = false;
        }
    }

    void EndGame()
    {
        // ���������� ������ � ���������
        endGamePanel.SetActive(true);

        // ������������� �����
        Time.timeScale = 0f;

        // ���������� ������
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // ��������� ���������� ������� (���� ����)
        var playerCamera = FindObjectOfType<CameraController>();
        if (playerCamera != null)
        {
            playerCamera.enabled = false;
        }
    }
}