using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public Animator animator;
    public float speed = 2f;  // �������� �������� �����
    public Transform player;  // ����� (���� ��� �����)
    public float gravity = -9.81f;  // ���� ����������
    public float stopDistance = 2f;  // ���������� �� ������, �� ������� ����� �����������
    private CharacterController controller;  // ������ �� CharacterController
    private Vector3 velocity;  // �������� �������
    private Vector3 moveDirection;  // ����������� �������� �����

    void Start()
    {
        // �������� ��������� CharacterController
        controller = GetComponent<CharacterController>();

        // �������� ��������� Animator, ���� �� �� ��� ���������� �������
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        // ��������, ��������� �� ����� �� �����
        if (controller.isGrounded)
        {
            // ���� �� �����, �������� �������� �������
            velocity.y = -2f;  // ������� ������������� ��������, ����� ����� ������ �������� � �����
        }
        else
        {
            // ���� �� �� �����, ���������� ��������� ����������
            velocity.y += gravity * Time.deltaTime;
        }

        // ���� ����� (����) ����������
        if (player != null)
        {
            // ��������� ���������� �� ������
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // ���� ��������� ������ 2 ������, ����� �������� � ������
            if (distanceToPlayer > stopDistance)
            {
                // ������������ ����������� �������� ����� � ������
                Vector3 direction = (player.position - transform.position).normalized;
                moveDirection = direction * speed;  // ������ ����������� � ������ ��������

                // ������������� �������� ��� ���������
                animator.SetFloat("Speed", speed);

                // ������� ����� ����� � ������
                if (direction.x > 0)
                {
                    transform.localScale = new Vector3(1, 1, 1);  // ��������� ������
                }
                else if (direction.x < 0)
                {
                    transform.localScale = new Vector3(-1, 1, 1);  // ��������� �����
                }
            }
            else
            {
                // ���� ���������� ������ 2 ������, ����� ���������������
                moveDirection = Vector3.zero;  // ������������� ��������
                animator.SetFloat("Speed", 0);  // ������������� �������� �������� �� 0 (��������)
                Debug.Log("����� �����������");
            }
        }

        // ��������� �������� �� ����������� (moveDirection) � ������� (velocity)
        Vector3 move = moveDirection * Time.deltaTime;  // �������������� ��������
        controller.Move(move + velocity * Time.deltaTime);  // ��������� � �������������� ��������, � �������
    }
}
