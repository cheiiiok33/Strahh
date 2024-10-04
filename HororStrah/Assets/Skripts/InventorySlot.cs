using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public ItemSkriptableObject item;
    public int amount;
    public bool isEmpty = true;
    public GameObject iconGO;
    public TMP_Text itemAmountText;

    private void Awake()
    {
        iconGO = transform.GetChild(0).gameObject;
        itemAmountText = transform.GetChild(1).GetComponent<TMP_Text>();
    }

    public void SetIcon(Sprite icon)
    {
        iconGO.GetComponent<Image>().color = new Color(1, 1, 1);
        iconGO.GetComponent<Image>().sprite = icon;
        iconGO.GetComponent<Image>().sprite = icon;
    }
}
