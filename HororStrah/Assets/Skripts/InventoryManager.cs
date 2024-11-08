using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject UIPanel;
    public GameObject crosshair;
    public Transform inventoryPanel;
    public List<InventorySlot> slots = new List<InventorySlot>();
    public bool isOpened;
    public float reachDistance = 3f;
    private Camera mainCamera;

    private void Awake()
    {
        UIPanel.SetActive(true);
    }

    void Start()
    {
        mainCamera = Camera.main;
        for (int i = 0; i < inventoryPanel.childCount; i++)
        {
            if (inventoryPanel.GetChild(i).GetComponent<InventorySlot>() != null)
            {
                slots.Add(inventoryPanel.GetChild(i).GetComponent<InventorySlot>());
            }
        }

        UIPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isOpened = !isOpened;
            if (isOpened)
            {
                UIPanel.SetActive(true);
                crosshair.SetActive(false);
            }
            else
            {
                UIPanel.SetActive(false);
                crosshair.SetActive(true);

                if (UIManager.instance != null)
                {
                    UIManager.instance.HideNoteText();
                }
            }
        }

        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, reachDistance))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (hit.collider.gameObject.GetComponent<Item>() != null)
                {
                    AddItem(hit.collider.gameObject.GetComponent<Item>().item, hit.collider.gameObject.GetComponent<Item>().amount);
                    Destroy(hit.collider.gameObject);
                }
            }

            Debug.DrawRay(ray.origin, ray.direction * reachDistance, Color.green);
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * reachDistance, Color.red);
        }
    }

    private void AddItem(ItemScriptableObject _item, int _amount)
    {
        // Проверяем, является ли предмет запиской и добавляем время
        if (_item is NoteItem)
        {
            TimerController timerController = FindObjectOfType<TimerController>();
            if (timerController != null)
            {
                timerController.AddTime(60f);
                Debug.Log($"Подобрана записка: {_item.itemName}. Добавлено 60 секунд.");
            }
            else
            {
                Debug.LogWarning("TimerController не найден на сцене!");
            }
        }

        // Проверяем, есть ли уже такой предмет в инвентаре
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == _item)
            {
                if (slot.amount + _amount <= _item.maximumAmount)
                {
                    slot.amount += _amount;
                    slot.itemAmountText.text = slot.amount.ToString();
                    return; // Выходим из метода, так как предмет уже добавлен
                }
                break;
            }
        }

        // Если предмет не был добавлен к существующему стаку, ищем пустой слот
        foreach (InventorySlot slot in slots)
        {
            if (slot.isEmpty)
            {
                slot.item = _item;
                slot.amount = _amount;
                slot.isEmpty = false;
                slot.SetIcon(_item.icon);
                slot.itemAmountText.text = _amount.ToString();
                return;
            }
        }
        Debug.LogWarning("Инвентарь полон! Невозможно добавить предмет: " + _item.itemName);
    }
}