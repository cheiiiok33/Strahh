using UnityEngine;
using System.Collections;

public class ZombieController : MonoBehaviour
{
    public Animator animator;
    public float walkSpeed = 4f;
    public float chaseSpeed = 7f;
    public Transform player;
    public float gravity = -9.81f;
    public float stopDistance = 2f;
    private CharacterController controller;
    private Vector3 velocity;
    private Vector3 moveDirection;
    private bool isMoving;
    public AudioSource scrimerAudio;
    private bool hasPlayedScreamer = false;
    public LayerMask obstacleLayer;

    public float detectionRange = 10f;
    public float wanderRadius = 10f;
    public float minWanderWaitTime = 1f;
    public float maxWanderWaitTime = 2f;

    private Vector3 wanderTarget;
    private bool isWandering = false;
    private bool isChasing = false;
    private float waitTimer = 0f;
    private Vector3 startPosition;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        startPosition = transform.position;
        SetNewWanderTarget();

        if (scrimerAudio == null || scrimerAudio.clip == null)
        {
            Debug.LogWarning("Скример аудио не настроен!");
        }
    }

    void Update()
    {
        if (controller.isGrounded)
        {
            velocity.y = -2f;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (IsPlayerVisible() && distanceToPlayer <= detectionRange)
            {
                isChasing = true;
                isWandering = false;
                ChasePlayer();
                Debug.Log("Преследую игрока");
            }
            else
            {
                isChasing = false;
                Wander();
                Debug.Log("Патрулирую");
            }

            if (distanceToPlayer <= stopDistance && isChasing)
            {
                if (!hasPlayedScreamer && scrimerAudio != null)
                {
                    scrimerAudio.Play();
                    hasPlayedScreamer = true;
                    Destroy(gameObject, scrimerAudio.clip.length);
                }
                return;
            }
        }

        controller.Move((moveDirection + velocity) * Time.deltaTime);
        animator.SetFloat("Speed", moveDirection.magnitude);

        // Отладочная информация
        Debug.Log($"Позиция: {transform.position}, Цель: {wanderTarget}, Скорость: {moveDirection.magnitude}");
    }

    private void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        moveDirection = direction * chaseSpeed;

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                Quaternion.LookRotation(direction),
                                                Time.deltaTime * 5f);
        }
    }

    private void Wander()
    {
        if (!isWandering)
        {
            SetNewWanderTarget();
            isWandering = true;
            waitTimer = Random.Range(minWanderWaitTime, maxWanderWaitTime);
            Debug.Log($"Новая точка патрулирования: {wanderTarget}");
        }

        if (waitTimer > 0)
        {
            waitTimer -= Time.deltaTime;
            moveDirection = Vector3.zero;
            return;
        }

        Vector3 directionToTarget = wanderTarget - transform.position;
        directionToTarget.y = 0;

        if (directionToTarget.magnitude > 0.5f)
        {
            moveDirection = directionToTarget.normalized * walkSpeed;

            if (moveDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation,
                                                    Quaternion.LookRotation(moveDirection),
                                                    Time.deltaTime * 5f);
            }
        }
        else
        {
            isWandering = false;
        }
    }

    private void SetNewWanderTarget()
    {
        Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
        wanderTarget = startPosition + new Vector3(randomCircle.x, 0, randomCircle.y);

        // Визуализация точки патрулирования
        Debug.DrawLine(transform.position, wanderTarget, Color.yellow, 2f);
    }

    private bool IsPlayerVisible()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        RaycastHit hit;
        Vector3 startPos = transform.position + Vector3.up;
        Vector3 endPos = player.position + Vector3.up;

        if (Physics.Raycast(startPos, directionToPlayer.normalized, out hit, distanceToPlayer, obstacleLayer))
        {
            Debug.DrawLine(startPos, hit.point, Color.red, 0.1f);
            return false;
        }

        Debug.DrawLine(startPos, endPos, Color.green, 0.1f);
        return true;
    }

    void OnDrawGizmosSelected()
    {
        // Визуализация радиусов в редакторе
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        if (startPosition != Vector3.zero)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(startPosition, wanderRadius);
        }

        if (isWandering)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, wanderTarget);
            Gizmos.DrawSphere(wanderTarget, 0.5f);
        }
    }
}