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

        // ��������� �������� �����
        controller.Move((moveDirection + velocity) * Time.deltaTime);
    }
}
