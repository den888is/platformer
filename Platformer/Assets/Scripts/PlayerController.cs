using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 5f; // Сила прыжка
    public float moveSpeed = 5f; // Скорость перемещения
    private Rigidbody2D rb; // Компонент Rigidbody2D
    public bool isGrounded; // Проверка на земле

    float right;//переменная для направления движения;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move(); // Вынесли перемещение в отдельный метод
        // Проверяем, нажата ли клавиша пробела и находится ли игрок на земле
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Debug.Log("Jump");
            Jump();
        }
    }
    void Move()
    {
        // Получаем ввод по горизонтали
        float moveInput = Input.GetAxis("Horizontal");

        // Проверка на столкновение с стенами
        if (!IsTouchingWall(moveInput))
        {
            // Перемещение игрока
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }
        else//иначе не двигаемся по горизонтали*
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
    }

    private bool IsTouchingWall(float moveInput)
    {
        // Проверка на столкновение с стенами
        if (moveInput != 0)
        {
            // Определяем направление движения
            Vector2 direction = new Vector2(moveInput, 0);//растет при нажатии кнопки движения
            //Debug.Log("direction.x: " + direction.x);

            if (direction.x > 0) { right = 1f; }
            else
            {
                right = -1f;
            }
            Debug.Log("Hero position: " + transform.position);
            RaycastHit2D hit = Physics2D.Raycast(new Vector3(transform.position.x + (right * 0.6876f), transform.position.y, 0f), direction, 0.125f); ; // 0.5f - расстояние до стены

            // Если есть столкновение со стеной, возвращаем true
            if (hit.collider != null)
            {
                Debug.Log("hit.collider name: " + (hit.collider.gameObject.name));
                Debug.Log("hit.collider.CompareTag(Wall): " + hit.collider.CompareTag("Wall"));
            }
            return hit.collider != null && hit.collider.CompareTag("Wall");//Wall
        }
        return false;
    }

    void Jump()
    {
        // Применяем силу прыжка
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isGrounded = false; // Игрок теперь в воздухе
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Проверяем столкновение с землей
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // Игрок на земле
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        // Проверяем столкновение с землей
        if (collision.gameObject.CompareTag("Ground"))
        {

            isGrounded = true; // Игрок на земле
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        // Проверяем столкновение с землей
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false; // Игрок на земле
        }
    }
}
