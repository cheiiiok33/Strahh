using UnityEngine;

public class LeverController : MonoBehaviour
{
    [SerializeField] private Animator leverAnimator;
    [SerializeField] private float interactionDistance = 2f; // ���������� ��������������
    private bool isActivated = false;
    private Camera mainCamera;

    public bool IsActivated { get { return isActivated; } }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isActivated)
        {
            // ������� ��� �� ������ ������
            Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            RaycastHit hit;

            // ���������, ����� �� ��� � ������ � ��������� �� �� � �������� ��������� ��������������
            if (Physics.Raycast(ray, out hit, interactionDistance))
            {
                // ���������, ������ �� �� � ���� �����
                if (hit.collider.gameObject == gameObject)
                {
                    Debug.Log("�������� ������������ �����");
                    ActivateLever();
                }
            }
        }
    }

    private void ActivateLever()
    {
        Debug.Log("��������� ������");
        isActivated = true;
        leverAnimator.SetBool("IsActivated", true);
        Debug.Log("����� �����������");
    }
}