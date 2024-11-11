using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
/// IPointerDownHandler - Следит за нажатиями мышки по объекту на котором висит этот скрипт
/// IPointerUpHandler - Следит за отпусканием мышки по объекту на котором висит этот скрипт
/// IDragHandler - Следит за тем не водим ли мы нажатую мышку по объекту
public class DragAndDropItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public InventorySlot oldSlot;
    private Transform player;

    private void Start()
    {
        //ПОСТАВЬТЕ ТЭГ "PLAYER" НА ОБЪЕКТЕ ПЕРСОНАЖА!
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // Находим скрипт InventorySlot в слоте в иерархии
        oldSlot = transform.GetComponentInParent<InventorySlot>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the player has the 'Player' tag.");
        }
    }
   

    private bool isDragging = false; // Добавляем флаг для отслеживания перетаскивания

    public void OnDrag(PointerEventData eventData)
    {
        if (oldSlot.isEmpty || eventData.button != PointerEventData.InputButton.Left)
            return;

        isDragging = true;
        GetComponent<RectTransform>().position += new Vector3(eventData.delta.x, eventData.delta.y);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (oldSlot.isEmpty)
        {
            if (UIManager.instance != null)
            {
                UIManager.instance.HideNoteText();
            }
            return;
        }

        isDragging = false;

        if (eventData.button == PointerEventData.InputButton.Left && !isDragging)
        {
            if (oldSlot.item.itemType == ItemType.Note)
            {
                if (UIManager.instance != null)
                {
                    UIManager.instance.DisplayNoteText(oldSlot.item.itemDescription);
                }
            }
            else if (oldSlot.item.itemType == ItemType.Medkit)
            {
                oldSlot.UseItem();
            }
        }
        if (oldSlot.isEmpty)
        {
            // Если кликнули по пустому слоту, закрываем текст записки
            if (UIManager.instance != null)
            {
                UIManager.instance.HideNoteText();
            }
            return;
        }

        isDragging = false; // Сбрасываем флаг при начале клика

        // Обработка клика для записки
        if (oldSlot.item != null && oldSlot.item.itemType == ItemType.Note)
        {
            if (eventData.button == PointerEventData.InputButton.Left && !isDragging)
            {
                Debug.Log("Clicking on note: " + oldSlot.item.itemName);
                if (UIManager.instance != null)
                {
                    Debug.Log("Note text: " + oldSlot.item.itemDescription);
                    UIManager.instance.DisplayNoteText(oldSlot.item.itemDescription);
                }
                return;
            }
        }

        // Начало перетаскивания
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0.75f);
            GetComponentInChildren<Image>().raycastTarget = false;
            transform.SetParent(transform.parent.parent);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (oldSlot.isEmpty || eventData.button != PointerEventData.InputButton.Left)
            return;

        // Если это была записка и мы её не перетаскивали, не обрабатываем дроп
        if (oldSlot.item.itemType == ItemType.Note && !isDragging)
        {
            isDragging = false;
            return;
        }

        GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1f);
        GetComponentInChildren<Image>().raycastTarget = true;

        GameObject hitObject = eventData.pointerCurrentRaycast.gameObject;

        if (hitObject != null)
        {
            InventorySlot newSlot = hitObject.GetComponent<InventorySlot>();
            if (newSlot == null)
            {
                newSlot = hitObject.GetComponentInParent<InventorySlot>();
            }

            if (newSlot != null && newSlot != oldSlot)
            {
                ExchangeSlotData(newSlot);

                // Закрываем текст записки при перемещении
                if (oldSlot.item != null && oldSlot.item.itemType == ItemType.Note)
                {
                    if (UIManager.instance != null)
                    {
                        UIManager.instance.HideNoteText();
                    }
                }
            }
            else if (hitObject.name == "UIPanel")
            {
                if (player != null)
                {
                    GameObject itemObject = Instantiate(oldSlot.item.itemPrefab,
                        player.position + Vector3.up + player.forward,
                        Quaternion.identity);

                    Item itemComponent = itemObject.GetComponent<Item>();
                    if (itemComponent != null)
                    {
                        itemComponent.item = oldSlot.item;
                        itemComponent.amount = oldSlot.amount;
                    }

                    // Закрываем текст записки при выбрасывании
                    if (UIManager.instance != null)
                    {
                        UIManager.instance.HideNoteText();
                    }
                }
            }
        }

        isDragging = false;
        transform.SetParent(oldSlot.transform);
        transform.position = oldSlot.transform.position;
    }

    private void ExchangeSlotData(InventorySlot newSlot)
{
    // Сохраняем данные нового слота
    ItemScriptableObject tempItem = newSlot.item;
    int tempAmount = newSlot.amount;
    bool tempIsEmpty = newSlot.isEmpty;
    Sprite tempSprite = newSlot.iconGO?.GetComponent<Image>()?.sprite;

    // Переносим данные из старого слота в новый
    newSlot.item = oldSlot.item;
    newSlot.amount = oldSlot.amount;
    newSlot.isEmpty = oldSlot.isEmpty;
    if (!oldSlot.isEmpty)
    {
        newSlot.SetIcon(oldSlot.iconGO.GetComponent<Image>().sprite);
        newSlot.itemAmountText.text = oldSlot.amount.ToString();
    }

    // Переносим сохраненные данные в старый слот
    oldSlot.item = tempItem;
    oldSlot.amount = tempAmount;
    oldSlot.isEmpty = tempIsEmpty;
    
    if (!tempIsEmpty && tempSprite != null)
    {
        oldSlot.SetIcon(tempSprite);
        oldSlot.itemAmountText.text = tempAmount.ToString();
    }
    else
    {
        oldSlot.iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        oldSlot.iconGO.GetComponent<Image>().sprite = null;
        oldSlot.itemAmountText.text = "";
    }
}
}
