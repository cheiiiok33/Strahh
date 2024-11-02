using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Note Item", menuName = "Inventory/Items/New Note Item")]
public class NoteItem : ItemScriptableObject
{
   private void OnEnable()
    {
        itemType = ItemType.Note;
    }
    
    public void SetupAsNote(string description)
    {
        itemType = ItemType.Note;
        itemDescription = description;
    }
}
