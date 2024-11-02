using UnityEngine;

public class LeverController : MonoBehaviour
{
    [SerializeField] private Animator leverAnimator;
    [SerializeField] private float interactionDistance = 2f; // Расстояние взаимодействия
    private bool isActivated = false;
    private Camera mainCamera;

    public bool IsActivated { get { return isActivated; } }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isActivated)
        {
            // Создаем луч из центра камеры
            Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
            RaycastHit hit;

            // Проверяем, попал ли луч в объект и находится ли он в пределах дистанции взаимодействия
            if (Physics.Raycast(ray, out hit, interactionDistance))
            {
                // Проверяем, попали ли мы в этот рычаг
                if (hit.collider.gameObject == gameObject)
                {
                    Debug.Log("Пытаемся активировать рычаг");
                    ActivateLever();
                }
            }
        }
    }

    private void ActivateLever()
    {
        Debug.Log("Активация рычага");
        isActivated = true;
        leverAnimator.SetBool("IsActivated", true);
        Debug.Log("Рычаг активирован");
    }
}