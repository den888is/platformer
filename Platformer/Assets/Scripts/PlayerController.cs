using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 5f; // ���� ������
    public float moveSpeed = 5f; // �������� �����������
    private Rigidbody2D rb; // ��������� Rigidbody2D
    public bool isGrounded; // �������� �� �����

    float right;//���������� ��� ����������� ��������;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move(); // ������� ����������� � ��������� �����
        // ���������, ������ �� ������� ������� � ��������� �� ����� �� �����
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Debug.Log("Jump");
            Jump();
        }
    }
    void Move()
    {
        // �������� ���� �� �����������
        float moveInput = Input.GetAxis("Horizontal");

        // �������� �� ������������ � �������
        if (!IsTouchingWall(moveInput))
        {
            // ����������� ������
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }
        else//����� �� ��������� �� �����������*
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
    }

    private bool IsTouchingWall(float moveInput)
    {
        // �������� �� ������������ � �������
        if (moveInput != 0)
        {
            // ���������� ����������� ��������
            Vector2 direction = new Vector2(moveInput, 0);//������ ��� ������� ������ ��������
            //Debug.Log("direction.x: " + direction.x);

            if (direction.x > 0) { right = 1f; }
            else
            {
                right = -1f;
            }
            Debug.Log("Hero position: " + transform.position);
            RaycastHit2D hit = Physics2D.Raycast(new Vector3(transform.position.x + (right * 0.6876f), transform.position.y, 0f), direction, 0.125f); ; // 0.5f - ���������� �� �����

            // ���� ���� ������������ �� ������, ���������� true
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
        // ��������� ���� ������
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isGrounded = false; // ����� ������ � �������
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ��������� ������������ � ������
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // ����� �� �����
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        // ��������� ������������ � ������
        if (collision.gameObject.CompareTag("Ground"))
        {

            isGrounded = true; // ����� �� �����
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        // ��������� ������������ � ������
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false; // ����� �� �����
        }
    }
}
