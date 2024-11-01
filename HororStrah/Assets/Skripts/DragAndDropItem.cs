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
    public void OnDrag(PointerEventData eventData)
    {
        // Если слот пустой, то мы не выполняем то что ниже return;
        if (oldSlot.isEmpty)
            return;
        GetComponent<RectTransform>().position += new Vector3(eventData.delta.x, eventData.delta.y);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (oldSlot.isEmpty)
            return;

        if (oldSlot.item != null && oldSlot.item.itemType == ItemType.Note && eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Clicking on note: " + oldSlot.item.itemName);
            if (UIManager.instance != null)
            {
                Debug.Log("Note text: " + oldSlot.item.itemDescription);
                UIManager.instance.DisplayNoteText(oldSlot.item.itemDescription);
                return; // Прерываем выполнение, чтобы не начать drag
            }
        }

        // Если это не записка или это правый клик, выполняем drag
        if (oldSlot.item.itemType == ItemType.Note && eventData.button != PointerEventData.InputButton.Right)
        {
            return;
        }
        
            GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0.75f);
            GetComponentInChildren<Image>().raycastTarget = false;
            transform.SetParent(transform.parent.parent);
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (oldSlot.isEmpty)
            return;
        // Делаем картинку опять не прозрачной
        GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1f);
        // И чтобы мышка опять могла ее засечь
        GetComponentInChildren<Image>().raycastTarget = true;

        //Поставить DraggableObject обратно в свой старый слот
        transform.SetParent(oldSlot.transform);
        transform.position = oldSlot.transform.position;
        //Если мышка отпущена над объектом по имени UIPanel, то...
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            if (eventData.pointerCurrentRaycast.gameObject.name == "UIPanel")
            {
                if (player != null)
                {
                    GameObject itemObject = Instantiate(oldSlot.item.itemPrefab, player.position + Vector3.up + player.forward, Quaternion.identity);
                    // Устанавливаем количество объектов такое какое было в слоте
                    Item itemComponent = itemObject.GetComponent<Item>();

                    if (itemComponent != null)
                    {
                        itemComponent.item = oldSlot.item;
                        itemComponent.amount = oldSlot.amount;
                    }
                    // убираем значения InventorySlot
                    NullifySlotData();
                }
            }
        }
        else if (eventData.pointerCurrentRaycast.gameObject.transform.parent != null &&
                      eventData.pointerCurrentRaycast.gameObject.transform.parent.parent != null &&
                      eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<InventorySlot>() != null)
        {
            // Перемещаем данные из одного слота в другой
            ExchangeSlotData(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<InventorySlot>());
        }

        void NullifySlotData()
        {
            // убираем значения InventorySlot
            oldSlot.item = null;
            oldSlot.amount = 0;
            oldSlot.isEmpty = true;
            oldSlot.iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            oldSlot.iconGO.GetComponent<Image>().sprite = null;
            oldSlot.itemAmountText.text = "";
        }
        void ExchangeSlotData(InventorySlot newSlot)
        {
            // Временно храним данные newSlot в отдельных переменных
            ItemScriptableObject item = newSlot.item;
            int amount = newSlot.amount;
            bool isEmpty = newSlot.isEmpty;
            GameObject iconGO = newSlot.iconGO;
            TMP_Text itemAmountText = newSlot.itemAmountText;

            // Заменяем значения newSlot на значения oldSlot
            newSlot.item = oldSlot.item;
            newSlot.amount = oldSlot.amount;
            if (oldSlot.isEmpty == false)
            {
                newSlot.SetIcon(oldSlot.iconGO.GetComponent<Image>().sprite);
                newSlot.itemAmountText.text = oldSlot.amount.ToString();
            }
            else
            {
                newSlot.iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                newSlot.iconGO.GetComponent<Image>().sprite = null;
                newSlot.itemAmountText.text = "";
            }

            newSlot.isEmpty = oldSlot.isEmpty;

            // Заменяем значения oldSlot на значения newSlot сохраненные в переменных
            oldSlot.item = item;
            oldSlot.amount = amount;
            if (isEmpty == false)
            {
                oldSlot.SetIcon(iconGO.GetComponent<Image>().sprite);
                oldSlot.itemAmountText.text = amount.ToString();
            }
            else
            {
                oldSlot.iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                oldSlot.iconGO.GetComponent<Image>().sprite = null;
                oldSlot.itemAmountText.text = "";
            }

            oldSlot.isEmpty = isEmpty;
        }
    }
}
