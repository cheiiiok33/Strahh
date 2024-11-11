using UnityEngine;

[CreateAssetMenu(fileName = "Note Item", menuName = "Inventory/Items/New Note Item")]
public class NoteItem : ItemScriptableObject
{
    [Header("Time Settings")]
    [SerializeField] private float timeBonusAmount = 60f; // Ёто добавит поле в инспектор

    private void OnEnable()
    {
        itemType = ItemType.Note;
    }

    public void SetupAsNote(string description)
    {
        itemType = ItemType.Note;
        itemDescription = description;

        TimerController timerController = GameObject.FindObjectOfType<TimerController>();
        if (timerController != null)
        {
            timerController.AddTime(timeBonusAmount); // »спользуем установленное значение
            Debug.Log($"ƒобавлено {timeBonusAmount} секунд от записки {itemName}");
        }
        else
        {
            Debug.LogWarning("TimerController не найден в сцене!");
        }
    }
}