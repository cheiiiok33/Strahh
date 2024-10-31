using UnityEngine;

public class LeverController : MonoBehaviour
{
    [SerializeField] private Animator leverAnimator;
    private bool isActivated = false;
    private bool playerInRange = false;

    public bool IsActivated { get { return isActivated; } }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log($"Кнопка E нажата. PlayerInRange: {playerInRange}, IsActivated: {isActivated}");
        }

        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !isActivated)
        {
            Debug.Log("Пытаемся активировать рычаг");
            ActivateLever();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Что-то вошло в триггер: {other.gameObject.name} с тегом: {other.tag}");

        if (other.CompareTag("Player"))
        {
            Debug.Log("Игрок вошел в зону рычага");
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Игрок вышел из зоны рычага");
            playerInRange = false;
        }
    }

    // Только ОДИН метод ActivateLever должен быть в классе
    private void ActivateLever()
    {
        Debug.Log("Активация рычага");
        isActivated = true;
        leverAnimator.SetBool("IsActivated", true);
        Debug.Log("Рычаг активирован");
    }
}