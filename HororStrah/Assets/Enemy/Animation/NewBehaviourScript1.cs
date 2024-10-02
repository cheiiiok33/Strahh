using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public Animator animator;
    public float speed = 2f;  // Скорость движения зомби
    public Transform player;  // Игрок (цель для зомби)
    public float gravity = -9.81f;  // Сила гравитации
    public float stopDistance = 2f;  // Расстояние до игрока, на котором зомби остановится
    private CharacterController controller;  // Ссылка на CharacterController
    private Vector3 velocity;  // Скорость падения
    private Vector3 moveDirection;  // Направление движения зомби

    void Start()
    {
        // Получаем компонент CharacterController
        controller = GetComponent<CharacterController>();

        // Получаем компонент Animator, если он не был установлен вручную
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        // Проверка, находится ли зомби на земле
        if (controller.isGrounded)
        {
            // Если на земле, обнуляем скорость падения
            velocity.y = -2f;  // Немного отрицательная величина, чтобы зомби плотно прилегал к земле
        }
        else
        {
            // Если не на земле, продолжаем применять гравитацию
            velocity.y += gravity * Time.deltaTime;
        }

        // Если игрок (цель) существует
        if (player != null)
        {
            // Вычисляем расстояние до игрока
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Если дистанция больше 2 метров, зомби движется к игроку
            if (distanceToPlayer > stopDistance)
            {
                // Рассчитываем направление движения зомби к игроку
                Vector3 direction = (player.position - transform.position).normalized;
                moveDirection = direction * speed;  // Задаем направление с учетом скорости

                // Устанавливаем скорость для аниматора
                animator.SetFloat("Speed", speed);

                // Поворот зомби лицом к игроку
                if (direction.x > 0)
                {
                    transform.localScale = new Vector3(1, 1, 1);  // Повернуть вправо
                }
                else if (direction.x < 0)
                {
                    transform.localScale = new Vector3(-1, 1, 1);  // Повернуть влево
                }
            }
            else
            {
                // Если расстояние меньше 2 метров, зомби останавливается
                moveDirection = Vector3.zero;  // Останавливаем движение
                animator.SetFloat("Speed", 0);  // Устанавливаем скорость анимации на 0 (ожидание)
                Debug.Log("Зомби остановился");
            }
        }

        // Применяем движение по горизонтали (moveDirection) и падение (velocity)
        Vector3 move = moveDirection * Time.deltaTime;  // Горизонтальное движение
        controller.Move(move + velocity * Time.deltaTime);  // Применяем и горизонтальное движение, и падение
    }
}
