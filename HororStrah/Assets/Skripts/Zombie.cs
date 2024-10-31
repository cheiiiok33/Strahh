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

    public LayerMask obstacleLayer;  // Слой для проверки препятствий

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

            // Проверка на наличие препятствий между зомби и игроком
            if (!IsPlayerVisible())
            {
                // Если есть препятствие, зомби останавливается и переходит в состояние Idle
                StopMoving();
                return;
            }

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
                Vector3 lookDirection = (player.position - transform.position).normalized;
                lookDirection.y = 0;  // Обнуляем ось Y, чтобы зомби не наклонялся вверх или вниз

                // Если направление движения не равно нулю
                if (lookDirection != Vector3.zero)
                {
                    // Рассчитываем желаемое вращение
                    Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

                    // Плавно поворачиваем зомби к игроку
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f); // 5f - скорость поворота
                }
            }
        }

        // Применяем движение зомби
        controller.Move((moveDirection + velocity) * Time.deltaTime);
    }

    // Проверка на наличие препятствий между зомби и игроком
    private bool IsPlayerVisible()
    {
        // Проверяем линию от зомби до игрока на наличие препятствий
        if (Physics.Linecast(transform.position, player.position, obstacleLayer))
        {
            return false;  // Если есть препятствие, игрок не виден
        }

        return true;  // Если препятствий нет, игрок виден
    }

    // Метод для остановки движения и перехода в Idle
    private void StopMoving()
    {
        isMoving = false;
        moveDirection = Vector3.zero;
        animator.SetFloat("Speed", 0);  // Устанавливаем анимацию Idle
        Debug.Log("Зомби остановился из-за препятствия.");
    }
}
