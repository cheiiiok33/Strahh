using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Axe Item", menuName = "Inventory/Items/New Axe Item")]
public class AxeItem : ItemScriptableObject
{
    
    void OnEnable()
    {
        itemType = ItemType.Axe;
    }

    public void SetupAsNote(string description)
    {
        itemType = ItemType.Axe;
        itemDescription = description;
    }
}
