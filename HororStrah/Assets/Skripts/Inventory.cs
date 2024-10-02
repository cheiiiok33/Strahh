using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject inventoryUI;  // Панель UI, которую мы будем показывать и скрывать
    public int maxInventorySize = 6; // Максимум 6 слотов для предметов
    public List<Item> items = new List<Item>(); // Список для хранения предметов
    public Image[] itemSlots;

    private bool isInventoryOpen = false; // Флаг, открыт ли инвентарь

    void Update()
    {
        // Открытие/закрытие инвентаря на кнопку Tab
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    // Функция открытия/закрытия инвентаря
    void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryUI.SetActive(isInventoryOpen); // Показать/скрыть UI инвентаря
    }

    // Функция добавления предмета в инвентарь
    public void AddItem(Item newItem)
    {
        if (items.Count < maxInventorySize)
        {
            items.Add(newItem);
            Debug.Log("Предмет добавлен: " + newItem.name);
        }
        else
        {
            Debug.Log("Инвентарь полон!");
        }
    }
    void UpdateInventoryUI()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (i < items.Count)
            {
                itemSlots[i].sprite = items[i].icon; // Устанавливаем иконку предмета
                itemSlots[i].enabled = true; // Включаем отображение
            }
            else
            {
                itemSlots[i].sprite = null;
                itemSlots[i].enabled = false; // Отключаем отображение пустого слота
            }
        }
    }
}
