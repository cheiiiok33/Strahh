using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public ItemScriptableObject item;
    public int amount;
    public bool isEmpty = true;
    public GameObject iconGO;
    public TMP_Text itemAmountText;

    private void Awake()
    {
        iconGO = transform.GetChild(0).GetChild(0).gameObject;
        itemAmountText = transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>();
    }

    public void SetIcon(Sprite icon)
    {
        iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        iconGO.GetComponent<Image>().sprite = icon;
    }

    public void UseItem()
    {
        if (item != null && item is MedkitItem)
        {
            MedkitItem medkit = item as MedkitItem;
            if (medkit != null)
            {
                PlayerController playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.Heal(medkit.healAmount);
                    amount--;
                    if (amount <= 0)
                    {
                        ClearSlot();
                    }
                    else
                    {
                        itemAmountText.text = amount.ToString();
                    }
                }
            }
        }
    }

    private void ClearSlot()
    {
        item = null;
        amount = 0;
        isEmpty = true;
        iconGO.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        iconGO.GetComponent<Image>().sprite = null;
        itemAmountText.text = "";
    }
}