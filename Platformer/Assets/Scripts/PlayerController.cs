using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb; // ��������� Rigidbody2D
    private GameObject keysIndicator;//��������� �� ��������� ������

    private bool pressedJump;//���������� ������� ������ ������
    public float jumpForce; // ���� ������
    public float moveSpeed; // �������� �����������

    public bool isGrounded; // �������� �� ����� (��� �����������)
    public float moveInput, moveInput2;//������������ �������� (����������)
                                       //��� ����������
    public bool hitColliderNull;//���������� ������������ ����
    public bool hit2ColliderNull;//���������� ������������ ���� 2

    float right;//���������� ��� ����������� ��������;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpForce = Constants.JUMP_FORCE;
        moveSpeed = Constants.MOOVE_SPEED;
        keysIndicator = GameObject.Find("KeysIndicator");
    }

    void Update()
    {
        Move(); // ������� ����������� � ��������� �����

        // ���������, ������ �� ������� ������� � ��������� �� ����� �� �����
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Keypad8))
        {
            if (isGrounded)
                Jump();
        }

        AboveWall();
    }
    /*    void CheckKeyDown(KeyCode key, System.Action method)//�������� ������� ������� ������
        {
            if (Input.GetKeyDown(key))
            {
                method();
            }
        }
        bool CheckKeyDown(KeyCode key)//�������� ������� ������� ������
        {
            if (Input.GetKeyDown(key))
            {
                return true;
            }
            return false;
        }
    */
    void Move()
    {
        // �������� ���� �� �����������
        moveInput = Input.GetAxis("Horizontal");

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
    //������ ������� �� ����� ������
    void AboveWall()
    {
        RaycastHit2D hit2 = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y - 1, 0f), Vector2.down, 0.5f);//1.625f
        Debug.Log("AboveWall() ");
        if (hit2.collider != null)
        {
            // Debug.Log("hit2.collider name: " + (hit2.collider.gameObject.name));
            // ����� �� ������� �����������
            hit2ColliderNull = false;
            // ������ ������ ��� �����������
            moveInput2 = Input.GetAxis("Vertical");
            if (moveInput2 <= 0)
            {
                isGrounded = true;
                if (hit2.collider.CompareTag("Wall"))
                {
                    Debug.Log("hit2.collider.CompareTag(Wall)");
                }
            }
            else
            {
                isGrounded = false;
            }
        }
        else
        {
            hit2ColliderNull = true;
        }
    }

    private bool IsTouchingWall(float moveInput)
    {
        // �������� �� ������������ � �������
        if (moveInput != 0)
        {
            // ���������� ����������� ��������
            Vector2 direction = new Vector2(moveInput, 0);//������ ��� ������� ������ ��������
            if (direction.x > 0) { right = 1f; }
            else
            {
                right = -1f;
            }
            RaycastHit2D hit = Physics2D.Raycast(new Vector3(transform.position.x + (right * Constants.BEAM_DISPLACEMENT), transform.position.y, 0f), direction, Constants.BEAM_LENGTH);
            // ���� ���� ������������ �� ������, ���������� true
            if (hit.collider != null)
            {
                hitColliderNull = false;
            }
            else
            { hitColliderNull = true; }
            return hit.collider != null && hit.collider.CompareTag("Wall");//Wall
        }
        return false;
    }

    void Jump()
    {
        // ��������� ���� ������
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isGrounded = false; // ����� ������ � �������
        pressedJump = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("OnCollisionEnter2D");
        // ��������� ������������ � ������
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // ����� �� �����
            Debug.Log("OnCollisionEnter2D -  collision.gameObject.CompareTag(Ground): " + collision.gameObject.CompareTag("Ground"));
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("OnCollisionStay2D");
        // ��������� ������������ � ������
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // ����� �� �����
        }
        // ��������� ������������ � ������
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Wallll");
            isGrounded = true; // ����� �� ����� ?
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ��������� ������������ � ������
        if (collision.gameObject.CompareTag("Key"))
        {
            if (keysIndicator != null)
            {
                keysIndicator.GetComponent<KeysIndicator>().KeyCountPlus();
                Destroy(collision.gameObject);
            }
            else { Debug.LogError("null - keyIndicator!!!"); }
        }
        // ��������� ������������ � ������
        if (collision.gameObject.CompareTag("Door"))
        {
            Debug.Log("Door");
            if (keysIndicator.GetComponent<KeysIndicator>().keyCount == 5)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
