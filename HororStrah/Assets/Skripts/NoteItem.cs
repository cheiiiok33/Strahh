using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Food Item", menuName = "Inventory/Items/New Note Item")]
public class NoteItem : ItemSkriptableObject
{
   private void Start()
    {
        itemType = ItemType.Note;
    }
}
