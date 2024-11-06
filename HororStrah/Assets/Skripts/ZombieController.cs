using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class ZombieController : MonoBehaviour
{
    [Header("Основные настройки")]
    public float walkSpeed = 4f;
    public float chaseSpeed = 7f;
    public float stopDistance = 2f;
    public Transform player;

    [Header("Настройки патрулирования")]
    public float detectionRange = 10f;
    public float wanderRadius = 10f;
    public float minWanderWaitTime = 1f;
    public float maxWanderWaitTime = 2f;

    [Header("Компоненты")]
    public Animator animator;
    public AudioSource scrimerAudio;
    public LayerMask obstacleLayer;

    private NavMeshAgent agent;
    private Vector3 wanderTarget;
    private Vector3 startPosition;
    private bool isWandering = false;
    private bool isChasing = false;
    private bool hasPlayedScreamer = false;
    private float waitTimer = 0f;

    void Start()
    {
        // Получаем и настраиваем NavMeshAgent
        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.speed = walkSpeed;
            agent.stoppingDistance = stopDistance;
        }
        else
        {
            Debug.LogError("NavMeshAgent не найден на объекте!");
            enabled = false;
            return;
        }

        // Проверяем аниматор
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        // Сохраняем начальную позицию
        startPosition = transform.position;
        SetNewWanderTarget();

        // Проверяем аудио
        if (scrimerAudio == null || scrimerAudio.clip == null)
        {
            Debug.LogWarning("Скример аудио не настроен!");
        }

        // Проверяем игрока
        if (player == null)
        {
            Debug.LogError("Не указан объект игрока!");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Проверяем видимость и расстояние до игрока
        if (IsPlayerVisible() && distanceToPlayer <= detectionRange)
        {
            isChasing = true;
            isWandering = false;
            ChasePlayer();
        }
        else
        {
            isChasing = false;
            Wander();
        }

        // Проверяем расстояние для скримера
        if (distanceToPlayer <= stopDistance && isChasing)
        {
            if (!hasPlayedScreamer && scrimerAudio != null)
            {
                scrimerAudio.Play();
                hasPlayedScreamer = true;
                Destroy(gameObject, scrimerAudio.clip.length);
            }
            agent.isStopped = true;
            return;
        }

        // Обновляем анимацию
        if (animator != null)
        {
            animator.SetFloat("Speed", agent.velocity.magnitude);
        }
    }

    private void ChasePlayer()
    {
        agent.isStopped = false;
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);

        // Поворот в сторону игрока
        Vector3 direction = (player.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                Quaternion.LookRotation(direction),
                                                Time.deltaTime * 5f);
        }
    }

    private void Wander()
    {
        agent.speed = walkSpeed;

        if (!isWandering)
        {
            SetNewWanderTarget();
            isWandering = true;
            waitTimer = Random.Range(minWanderWaitTime, maxWanderWaitTime);
        }

        if (waitTimer > 0)
        {
            waitTimer -= Time.deltaTime;
            agent.isStopped = true;
            return;
        }

        agent.isStopped = false;
        agent.SetDestination(wanderTarget);

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            isWandering = false;
        }
    }

    private void SetNewWanderTarget()
    {
        Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
        wanderTarget = startPosition + new Vector3(randomCircle.x, 0, randomCircle.y);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(wanderTarget, out hit, wanderRadius, NavMesh.AllAreas))
        {
            wanderTarget = hit.position;
        }
    }

    private bool IsPlayerVisible()
    {
        if (player == null) return false;

        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        RaycastHit hit;
        Vector3 startPos = transform.position + Vector3.up;
        Vector3 endPos = player.position + Vector3.up;

        if (Physics.Raycast(startPos, directionToPlayer.normalized, out hit, distanceToPlayer, obstacleLayer))
        {
            return false;
        }

        return true;
    }

    void OnDrawGizmosSelected()
    {
        // Визуализация радиуса обнаружения
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Визуализация радиуса патрулирования
        if (startPosition != Vector3.zero)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(startPosition, wanderRadius);
        }

        // Визуализация текущей цели
        if (isWandering)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, wanderTarget);
            Gizmos.DrawSphere(wanderTarget, 0.5f);
        }
    }
}