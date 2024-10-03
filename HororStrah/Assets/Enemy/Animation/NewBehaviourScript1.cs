using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public Animator animator;
    public float speed = 2f;  // Скорость движения зомби
    public Transform player;  // Игрок (цель для зомби)
    public float gravity = -9.81f;  // Сила гравитации
    public float stopDistance = 2f;  // Расстояние до игрока, на котором зомби исчезнет
    private CharacterController controller;  // Ссылка на CharacterController
    private Vector3 velocity;  // Скорость падения
    private Vector3 moveDirection;  // Направление движения зомби
    private bool isMoving;  // Флаг, движется ли зомби
    public AudioSource scrimerAudio;  // Компонент для воспроизведения звука
    private bool hasPlayedScreamer = false;  // Флаг, чтобы скример сработал один раз

    void Start()
    {
        // Получаем компонент CharacterController
        controller = GetComponent<CharacterController>();

        // Получаем компонент Animator, если он не был установлен вручную
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        // Убеждаемся, что скример не воспроизводится в начале
        if (scrimerAudio == null || scrimerAudio.clip == null)
        {
            Debug.LogError("Не установлен AudioSource или аудиоклип для скримера!");
        }
        else
        {
            scrimerAudio.Stop();  // Останавливаем, если вдруг запустился по ошибке
        }
    }

    void Update()
    {
        // Применение гравитации
        if (controller.isGrounded)
        {
            velocity.y = -2f; // Чтобы зомби был "приклеен" к земле
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Если расстояние до игрока меньше stopDistance, зомби исчезает
            if (distanceToPlayer <= stopDistance)
            {
                if (!hasPlayedScreamer)
                {
                    // Воспроизводим скример перед удалением зомби
                    scrimerAudio.Play();
                    hasPlayedScreamer = true;  // Устанавливаем флаг, чтобы звук не повторялся

                    // Удаляем зомби с задержкой, равной длине аудиоклипа
                    Destroy(gameObject, scrimerAudio.clip.length);
                }

                return;  // Завершаем выполнение метода
            }

            // Если дистанция больше stopDistance, зомби продолжает двигаться
            Vector3 direction = (player.position - transform.position).normalized;
            moveDirection = direction * speed;
            isMoving = true;

            // Управление анимацией движения
            animator.SetFloat("Speed", isMoving ? speed : 0);

            // Поворот зомби лицом к игроку
            if (isMoving)
            {
                Vector3 lookDirection = player.position - transform.position;
                if (lookDirection.x > 0)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                else if (lookDirection.x < 0)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
            }
        }

        // Применяем движение зомби
        controller.Move((moveDirection + velocity) * Time.deltaTime);
    }
}
