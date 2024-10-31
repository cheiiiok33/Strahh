using UnityEngine;

public class LeverController : MonoBehaviour
{
    [SerializeField] private Animator leverAnimator;
    private bool isActivated = false;
    private bool playerInRange = false;

    public bool IsActivated { get { return isActivated; } }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log($"������ E ������. PlayerInRange: {playerInRange}, IsActivated: {isActivated}");
        }

        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !isActivated)
        {
            Debug.Log("�������� ������������ �����");
            ActivateLever();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"���-�� ����� � �������: {other.gameObject.name} � �����: {other.tag}");

        if (other.CompareTag("Player"))
        {
            Debug.Log("����� ����� � ���� ������");
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("����� ����� �� ���� ������");
            playerInRange = false;
        }
    }

    // ������ ���� ����� ActivateLever ������ ���� � ������
    private void ActivateLever()
    {
        Debug.Log("��������� ������");
        isActivated = true;
        leverAnimator.SetBool("IsActivated", true);
        Debug.Log("����� �����������");
    }
}