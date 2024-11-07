using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemScriptableObject item;
    public int amount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TimerController timerController = FindObjectOfType<TimerController>();

            if (timerController != null)
            {
                timerController.AddTime(60f);

                Destroy(gameObject);
            }
        }
    }
}