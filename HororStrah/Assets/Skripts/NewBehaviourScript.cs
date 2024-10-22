using UnityEngine;

public class LeverDoorController : MonoBehaviour
{
    // Массив рычагов
    public Lever[] levers;

    // Ссылка на дверь
    public GameObject door;

    
    private bool doorOpened = false;

    void Update()
    {
        
        if (doorOpened)
            return;

         
        if (AllLeversDown())
        {
            OpenDoor();
        }
    }

    // Метод для проверки состояния всех рычагов
    private bool AllLeversDown()
    {
        foreach (Lever lever in levers)
        {
            if (!lever.isLeverDown)
            {
                return false; // Если хотя бы один рычаг не опущен
            }
        }
        return true; // Все рычаги опущены
    }

    // Метод для открытия двери
    private void OpenDoor()
    {
        doorOpened = true;
        // Здесь можно добавить анимацию открытия двери или другие эффекты
        Debug.Log("Дверь открыта!");
        // Например, перемещаем дверь вверх
        door.transform.position += new Vector3(0, 5, 0); // Позиция может зависеть от сцены
    }
}

// Скрипт для рычага
public class Lever : MonoBehaviour
{
    public bool isLeverDown = false; // Состояние рычага

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            // Инвертируем состояние рычага при взаимодействии
            isLeverDown = !isLeverDown;
            Debug.Log("Рычаг " + (isLeverDown ? "опущен" : "поднят"));
            // Здесь можно добавить анимацию поворота рычага
        }
    }
}
