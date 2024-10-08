using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject UIPanel;
    public GameObject crosshair;
    public Transform inventoryPanel;
    public List <InventorySlot> slots = new List <InventorySlot> ();
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
            if(isOpened) 
            { 
                UIPanel.SetActive(true);
                crosshair.SetActive(false);
            }
            else
            {
                UIPanel.SetActive(false);
                crosshair.SetActive(true);
            }
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
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
        }
    }

    private void AddItem(ItemSkriptableObject _item, int _amount)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == _item)
            {
                slot.amount += _amount;
                slot.itemAmountText.text = slot.amount.ToString();
                return;
            }

        }
        foreach (InventorySlot slot in slots)
        {
            if (slot.isEmpty == true )
            {
                slot.item = _item;
                slot.amount = _amount;
                slot.isEmpty = false;
                slot.SetIcon(_item.icon);
                slot.itemAmountText.text = _amount.ToString();
                break;
            }
        }
    }
}
