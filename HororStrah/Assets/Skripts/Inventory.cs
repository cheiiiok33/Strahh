using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject inventoryUI;  // ������ UI, ������� �� ����� ���������� � ��������
    public int maxInventorySize = 6; // �������� 6 ������ ��� ���������
    public List<Item> items = new List<Item>(); // ������ ��� �������� ���������
    public Image[] itemSlots;

    private bool isInventoryOpen = false; // ����, ������ �� ���������

    void Update()
    {
        // ��������/�������� ��������� �� ������ Tab
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    // ������� ��������/�������� ���������
    void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryUI.SetActive(isInventoryOpen); // ��������/������ UI ���������
    }

    // ������� ���������� �������� � ���������
    public void AddItem(Item newItem)
    {
        if (items.Count < maxInventorySize)
        {
            items.Add(newItem);
            Debug.Log("������� ��������: " + newItem.name);
        }
        else
        {
            Debug.Log("��������� �����!");
        }
    }
    void UpdateInventoryUI()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (i < items.Count)
            {
                itemSlots[i].sprite = items[i].icon; // ������������� ������ ��������
                itemSlots[i].enabled = true; // �������� �����������
            }
            else
            {
                itemSlots[i].sprite = null;
                itemSlots[i].enabled = false; // ��������� ����������� ������� �����
            }
        }
    }
}
