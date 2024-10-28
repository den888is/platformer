using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb; // ��������� Rigidbody2D
    private GameObject keysIndicator;//��������� �� ��������� ������

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
        hitColliderNull = true;
        hit2ColliderNull = true;
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

    void Move()
    {
        // �������� ���� �� �����������
        moveInput = Input.GetAxis("Horizontal");

        // �������� �� ������������ � �������
        if (!IsTouchingWall(moveInput))//��� �������������� �����
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
        RaycastHit2D hit2 = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y - 0.9376f, 0f), Vector2.down, Constants.BEAM_LENGTH_VERTICAL);//
        RaycastHit2D hit22 = Physics2D.Raycast(new Vector3(transform.position.x - 0.6875f, transform.position.y - 0.9376f, 0f), Vector2.down, Constants.BEAM_LENGTH_VERTICAL);//2 ��� 
        RaycastHit2D hit222 = Physics2D.Raycast(new Vector3(transform.position.x + 0.6875f, transform.position.y - 0.9376f, 0f), Vector2.down, Constants.BEAM_LENGTH_VERTICAL);//3� ���
        Debug.Log("AboveWall() ");
        if (hit2.collider != null || hit22.collider != null || hit222.collider != null)
        {
            isGrounded = true;//������ �� �����
            // ����� �� ������� �����������
            hit2ColliderNull = false;
        }
        else
        {
            isGrounded = false;
            hit2ColliderNull = true;
        }
    }

    private bool IsTouchingWall(float moveInput)
    {
        Debug.Log("IsTouchingWall");
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

            RaycastHit2D hit = Physics2D.Raycast(new Vector3(transform.position.x + (right * Constants.BEAM_DISPLACEMENT), transform.position.y, 0f), direction, Constants.BEAM_LENGTH_HORIZONT);
            // ���� ���� ������������ �� ������, ���������� true
            if (hit.collider != null) //������ ���-�� � ����������� ���� �� ����������� ����
            {
                hitColliderNull = false;
                //�������� �� ������� �������������� ��� ��������� ��������
                /*                if (HasBoxCollider2D(hit.collider.gameObject))
                                {
                                    if (hit.collider.gameObject.GetComponent<BoxCollider2D>().isTrigger == true)
                                    { return false; } //���� ��������� �������� �������� - ���������� false
                                    else
                                        return true;
                                }
                                else return true;//���� ��������� �� �� ���� - �����*/
                return IsTriggerBoxCollider2D(hit.collider.gameObject);
            }
            else
            {
                Debug.Log("else 1");
                hitColliderNull = true;
                //������ ������ ��� ������
                hit = Physics2D.Raycast(new Vector3(transform.position.x + (right * Constants.BEAM_DISPLACEMENT), transform.position.y + 0.5001f, 0f), direction, Constants.BEAM_LENGTH_HORIZONT);
                if (hit.collider != null) //������ ����� ����
                {
                    hitColliderNull = false;
                    /* if (hit.collider.gameObject.GetComponent<BoxCollider2D>().isTrigger == true) { return false; }
                     else
                         return true;*/
                    return IsTriggerBoxCollider2D(hit.collider.gameObject);
                }
                else
                {
                    Debug.Log("else 2");
                    hitColliderNull = true;
                    //������ ������ ��� �����
                    hit = Physics2D.Raycast(new Vector3(transform.position.x + (right * Constants.BEAM_DISPLACEMENT), transform.position.y - 0.9376f, 0f), direction, Constants.BEAM_LENGTH_HORIZONT);
                    if (hit.collider != null) //������ ����� ����
                    {
                        hitColliderNull = false;
                        /*                        if (hit.collider.gameObject.GetComponent<BoxCollider2D>().isTrigger == true) { return false; }
                                                else
                                                    return true;*/
                        return IsTriggerBoxCollider2D(hit.collider.gameObject);
                    }
                    else
                    {
                        hitColliderNull = true;
                    }
                }
            }

            return !hitColliderNull;//���������� ���� �� �����
        }
        return false;
    }
    //��������������� ����� ��� ���������� ���������� � IsTouchingWall
    bool IsTriggerBoxCollider2D(GameObject gO)
    {
        Debug.Log("IsTriggerBoxCollider2D");
        //�������� �� ������� �������������� ��� ��������� ��������
        if (HasBoxCollider2D(gO))
        {
            Debug.Log("HasBoxCollider2D(gO)");
            if (gO.GetComponent<BoxCollider2D>().isTrigger == true)
            { return false; } //���� ��������� �������� �������� - ���������� false (�.�. ����� ���)
            else
                return true;
        }
        else return true;//���� ��������� �� �� ���� - ����� 
    }
    //��������� ������� BoxCollider2D �� �������
    bool HasBoxCollider2D(GameObject g)
    {
        if (g.GetComponent<BoxCollider2D>() != null)
        {
            return true;
        }
        else return false;
    }
    void Jump()
    {
        // ��������� ���� ������
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }
    private void OnCollisionStay2D(Collision2D collision)
    {

    }
    private void OnCollisionExit2D(Collision2D collision)
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ��������� ������������ � ������
        if (collision.gameObject.CompareTag("Key"))
        {
            Debug.Log("Key");
            if (keysIndicator != null)
            {
                Debug.Log("keysIndicator != null");
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
