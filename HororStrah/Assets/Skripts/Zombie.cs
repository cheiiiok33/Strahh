using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public Animator animator;
    public float speed = 2f;  // �������� �������� �����
    public Transform player;  // ����� (���� ��� �����)
    public float gravity = -9.81f;  // ���� ����������
    public float stopDistance = 2f;  // ���������� �� ������, �� ������� ����� ��������
    private CharacterController controller;  // ������ �� CharacterController
    private Vector3 velocity;  // �������� �������
    private Vector3 moveDirection;  // ����������� �������� �����
    private bool isMoving;  // ����, �������� �� �����
    public AudioSource scrimerAudio;  // ��������� ��� ��������������� �����
    private bool hasPlayedScreamer = false;  // ����, ����� ������� �������� ���� ���

    public LayerMask obstacleLayer;  // ���� ��� �������� �����������

    void Start()
    {
        // �������� ��������� CharacterController
        controller = GetComponent<CharacterController>();

        // �������� ��������� Animator, ���� �� �� ��� ���������� �������
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        // ����������, ��� ������� �� ��������������� � ������
        if (scrimerAudio == null || scrimerAudio.clip == null)
        {
            Debug.LogError("�� ���������� AudioSource ��� ��������� ��� ��������!");
        }
        else
        {
            scrimerAudio.Stop();  // �������������, ���� ����� ���������� �� ������
        }
    }

    void Update()
    {
        // ���������� ����������
        if (controller.isGrounded)
        {
            velocity.y = -2f; // ����� ����� ��� "��������" � �����
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // �������� �� ������� ����������� ����� ����� � �������
            if (!IsPlayerVisible())
            {
                // ���� ���� �����������, ����� ��������������� � ��������� � ��������� Idle
                StopMoving();
                return;
            }

            // ���� ���������� �� ������ ������ stopDistance, ����� ��������
            if (distanceToPlayer <= stopDistance)
            {
                if (!hasPlayedScreamer)
                {
                    // ������������� ������� ����� ��������� �����
                    scrimerAudio.Play();
                    hasPlayedScreamer = true;  // ������������� ����, ����� ���� �� ����������

                    // ������� ����� � ���������, ������ ����� ����������
                    Destroy(gameObject, scrimerAudio.clip.length);
                }

                return;  // ��������� ���������� ������
            }

            // ���� ��������� ������ stopDistance, ����� ���������� ���������
            Vector3 direction = (player.position - transform.position).normalized;
            moveDirection = direction * speed;
            isMoving = true;

            // ���������� ��������� ��������
            animator.SetFloat("Speed", isMoving ? speed : 0);

            // ������� ����� ����� � ������
            if (isMoving)
            {
                Vector3 lookDirection = (player.position - transform.position).normalized;
                lookDirection.y = 0;  // �������� ��� Y, ����� ����� �� ���������� ����� ��� ����

                // ���� ����������� �������� �� ����� ����
                if (lookDirection != Vector3.zero)
                {
                    // ������������ �������� ��������
                    Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

                    // ������ ������������ ����� � ������
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f); // 5f - �������� ��������
                }
            }
        }

        // ��������� �������� �����
        controller.Move((moveDirection + velocity) * Time.deltaTime);
    }

    // �������� �� ������� ����������� ����� ����� � �������
    private bool IsPlayerVisible()
    {
        // ��������� ����� �� ����� �� ������ �� ������� �����������
        if (Physics.Linecast(transform.position, player.position, obstacleLayer))
        {
            return false;  // ���� ���� �����������, ����� �� �����
        }

        return true;  // ���� ����������� ���, ����� �����
    }

    // ����� ��� ��������� �������� � �������� � Idle
    private void StopMoving()
    {
        isMoving = false;
        moveDirection = Vector3.zero;
        animator.SetFloat("Speed", 0);  // ������������� �������� Idle
        Debug.Log("����� ����������� ��-�� �����������.");
    }
}
