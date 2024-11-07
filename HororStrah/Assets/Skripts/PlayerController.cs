using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Передвижение")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float gravity = -9.81f;

    [Header("Здоровье")]
    public int maxHealth = 2;
    public int currentHealth;

    [Header("UI")]
    public GameObject gameOverScreen;
    public Image[] healthIcons; // Массив иконок здоровья
    public Color fullHeartColor = Color.white; // Цвет активного сердца
    public Color emptyHeartColor = new Color(0.5f, 0.5f, 0.5f, 1f); // Серый цвет для неактивного сердца

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private Animator animator;
    private bool isDead = false;
    private bool isDeathSequenceStarted = false;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }

        // Проверяем наличие иконок здоровья
        if (healthIcons == null || healthIcons.Length == 0)
        {
            Debug.LogError("Не назначены иконки здоровья!");
        }

        UpdateHealthDisplay();
    }

    private void Update()
    {
        if (isDead) return;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float speed = isRunning ? runSpeed : walkSpeed;

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (animator != null)
        {
            animator.SetFloat("DirectionX", x);
            animator.SetFloat("DirectionY", z);
            animator.SetFloat("Speed", speed);
        }
    }

    public void TakeDamage()
    {
        currentHealth--;
        UpdateHealthDisplay();

        if (currentHealth <= 0 && !isDead && !isDeathSequenceStarted)
        {
            isDeathSequenceStarted = true;
        }
    }

    private void UpdateHealthDisplay()
    {
        if (healthIcons == null) return;

        // Обновляем каждую иконку здоровья
        for (int i = 0; i < healthIcons.Length; i++)
        {
            if (healthIcons[i] != null)
            {
                // Если индекс меньше текущего здоровья - показываем белое сердце
                // иначе - серое
                healthIcons[i].color = i < currentHealth ? fullHeartColor : emptyHeartColor;
            }
        }
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;

        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }
        else
        {
            Debug.LogError("Экран проигрыша не назначен!");
        }

        enabled = false;

        if (animator != null)
        {
            // animator.SetTrigger("Die");
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}