using UnityEngine;

public class WoodBoard : MonoBehaviour
{
    [SerializeField] private float destroyDistance = 3f;
    private InventoryManager inventoryManager;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        inventoryManager = FindObjectOfType<InventoryManager>();

        // ���������, ��� � ������� ���� ��� "Wood"
        gameObject.tag = "Wood";
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ����� ���� ����
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, destroyDistance))
            {
                if (hit.collider.CompareTag("Wood"))
                {
                    TryChopWood();
                }
            }
        }
    }

    private void TryChopWood()
    {
        bool hasAxe = false;
        foreach (InventorySlot slot in inventoryManager.slots)
        {
            if (!slot.isEmpty && slot.item.itemType == ItemType.Axe)
            {
                hasAxe = true;
                break;
            }
        }

        if (hasAxe)
        {
            Debug.Log("����� ����������");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("����� ����� ����� ��������� �����");
        }
    }
}