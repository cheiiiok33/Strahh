using UnityEngine;

[CreateAssetMenu(fileName = "Medkit Item", menuName = "Inventory/Items/New Medkit Item")]
public class MedkitItem : ItemScriptableObject
{
    public int healAmount = 1;

    private void OnEnable()
    {
        itemType = ItemType.Food; // Используем существующий тип Food для аптечки
        maximumAmount = 2; // Максимальное количество аптечек в слоте
    }
}