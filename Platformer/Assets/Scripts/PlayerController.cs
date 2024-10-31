using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb; // ��������� Rigidbody2D
    private GameObject keysIndicator;//��������� �� ��������� ������
    private GameObject healthBar;//��������
    private RaycastHit2D hit2, hit22, hit222;//���������� ����
    private float speedVelocityY;//���������� �������� ������� �� Y (�� ��������� ��������������)
    private float maxVelY;//������������ �������� velocity.y �� �������
    private bool wasDamage;// ������� �� ���� �� �������
    private Animator animator;

    public float jumpForce; // ���� ������
    public float moveSpeed; // �������� �����������

    public bool isGrounded; // �������� �� ����� (��� �����������)
    public float moveInput, moveInput2;//������������ �������� (����������)
                                       //��� ����������
    public bool horizontalHitCollider;//���������� ������������ ��������������� ���� ����������� (�����)
    public bool verticalDownHitCollider;//���������� ������������  ������������� ���� 2 � ����������� (�����)

    float right;//���������� ��� ����������� ��������;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpForce = Constants.JUMP_FORCE;
        moveSpeed = Constants.MOOVE_SPEED;
        keysIndicator = GameObject.Find("KeysIndicator");
        horizontalHitCollider = false;
        verticalDownHitCollider = false;
        healthBar = GameObject.Find("HealthBar");
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move(); // ������� ����������� �� ����������� � ��������� �����

        // ������
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Keypad8))
        {
            if (isGrounded)
                Jump();
        }

        AboveWall();// �� �����������
    }
    //����������� ������ �� �����������
    void Move()
    {
        // �������� ���� �� �����������
        moveInput = Input.GetAxis("Horizontal");
        //�������
        if (moveInput > 0)
        {
            Quaternion rot = transform.rotation;
            rot.y = 0;
            transform.rotation = rot;
        }

        if (moveInput < 0)
        {
            Quaternion rot = transform.rotation;
            rot.y = 180;
            transform.rotation = rot;
        }
        if (moveInput == 0)
        {
            animator.SetBool("goHorizontal", false);
        }
        // �������� �� ������������ � �������
        if (!IsTouchingWall(moveInput))//��� �������������� �����
        {
            // ����������� ������
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
            if (moveInput != 0)
                animator.SetBool("goHorizontal", true);
            // animator.SetBool("jump", false);

        }
        else//����� �� ��������� �� �����������* (���� ������ �� �����������)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
            animator.SetBool("goHorizontal", false);
            animator.SetBool("fall", true);
        }
    }

    public void BlinkingRed(float immunityTime)
    {
        StartCoroutine(StartBlinkTimer(immunityTime));
    }
    private IEnumerator StartBlinkTimer(float time)
    {
        float timeRed = time;
        byte g = 0;
        byte increment = 1;
        int sign = 1;


        while (timeRed > 0)
        {
            timeRed -= Time.deltaTime;
            GetComponent<SpriteRenderer>().color = new Color32(255, g, g, 255);
            g = (byte)(g + increment * sign);
            if (g > 254) { sign = -1; }
            if (g < 1) { sign = 1; }
            yield return null;
        }
        GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);//������� ����
        yield return null;
    }

    //������ ������� �� ����������� ������
    //��������� isGrounded
    void AboveWall()
    {
        hit2 = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y - 0.9376f, 0f), Vector2.down, Constants.BEAM_LENGTH_VERTICAL);//
        hit22 = Physics2D.Raycast(new Vector3(transform.position.x - 0.6875f, transform.position.y - 0.9376f, 0f), Vector2.down, Constants.BEAM_LENGTH_VERTICAL);//2 ��� 
        hit222 = Physics2D.Raycast(new Vector3(transform.position.x + 0.6875f, transform.position.y - 0.9376f, 0f), Vector2.down, Constants.BEAM_LENGTH_VERTICAL);//3� ���
        Debug.Log("AboveWall() ");


        speedVelocityY = rb.velocity.y;//������� ��������

        //����������� � �����-�� �����������
        if (hit2.collider != null || hit22.collider != null || hit222.collider != null)
        {

            Debug.Log("colliders != null");

            if (speedVelocityY <= Constants.MAX_SPEED_VECTOR_VELOCITY_NO_DAMAGE)
            {//������� ������ ������ �������� ��������
                Debug.Log("speedVelocityY: " + speedVelocityY);
                Debug.Log("maxVelY: " + maxVelY);
            }
            verticalDownHitCollider = true;//���� ������������ � �����������
            if (!IsTriggerBoxCollider2D(AimObject(hit2, hit22, hit222)))//�� ������
            {
                Debug.Log("���������, �� �� ������� ���� ��������� ");
                // ���� ���� �� ���� ���������
                if (HasBoxCollider2D(AimObject(hit2, hit22, hit222)))
                {
                    Debug.Log("BoxCollider!");
                    ReactionItemWhithBoxCollider2D(AimObject(hit2, hit22, hit222));
                }
                else
                {
                    Debug.Log("��������� �����������");

                }
                isGrounded = true; //������ �� ������� ����������� 
                //���� ���� � ������ �� �������� - ������ ����
                wasDamage = healthBar.GetComponent<HealthBar>().CheckFallDamage(Mathf.RoundToInt(maxVelY));//����� ����������� �������� ����������
                if (wasDamage)
                {
                    maxVelY = 0; //�������� ������������ ������
                }
                animator.SetBool("onGround", true);
                animator.SetBool("jump", false);
                animator.SetBool("fall", false);
            }
            else
            {
                Debug.Log("������ �������������");
                isGrounded = false;
                verticalDownHitCollider = false;
                animator.SetBool("fall", true);
                animator.SetBool("onGround", false);
                animator.SetBool("jump", false);
            }
        }
        else
        {
            //��������� �����
            isGrounded = false;
            if (speedVelocityY < maxVelY) { maxVelY = speedVelocityY; }//���������� ������������ ��������
            animator.SetBool("onGround", false);
            if (speedVelocityY < 0)
            {
                animator.SetBool("fall", true);
                animator.SetBool("jump", false);
                animator.SetBool("onGround", false);
            }
            else { animator.SetBool("fall", false); animator.SetBool("jump", true); animator.SetBool("onGround", false); };
            //if (speedVelocityY > 0) { animator.SetBool("fall", false); } else { animator.SetBool("fall", true); };
        }
    }
    //��������������� ����� ��� SearchingItemWhithBoxCollider2D() 
    //����� ������������ ������ ������������ ���� �� ����� 
    GameObject AimObject(RaycastHit2D hit2, RaycastHit2D hit22, RaycastHit2D hit222)
    {
        if (hit2.collider != null) { return hit2.collider.gameObject; }
        else
        if (hit22.collider != null) { return hit22.collider.gameObject; }
        else
        if (hit222.collider != null) return hit222.collider.gameObject;
        return null;//���������� null ���� ��� �������
    }
    //����������� �� ������ ��� ���?
    bool IsTouchingWall(float moveInput)
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
                horizontalHitCollider = true; //����� -"��� ����������" ��������� �������� ���� ���������
                                              //�������� �� ������� �������������� ��� ��������� ��������
                return !IsTriggerBoxCollider2D(hit.collider.gameObject);
            }
            else //���� ��� ������� � ����������� �� ������
            {
                Debug.Log("else 1");
                horizontalHitCollider = false;//��� ���������� �������������� � �����
                                              //������ ������ ��� ������
                hit = Physics2D.Raycast(new Vector3(transform.position.x + (right * Constants.BEAM_DISPLACEMENT), transform.position.y + 0.5001f, 0f), direction, Constants.BEAM_LENGTH_HORIZONT);
                if (hit.collider != null) //������ ��������� ���� ������
                {
                    horizontalHitCollider = true;
                    return !IsTriggerBoxCollider2D(hit.collider.gameObject);
                }
                else
                {
                    Debug.Log("else 2");
                    horizontalHitCollider = false;
                    //������ ������ ��� �����
                    hit = Physics2D.Raycast(new Vector3(transform.position.x + (right * Constants.BEAM_DISPLACEMENT), transform.position.y - 0.9376f, 0f), direction, Constants.BEAM_LENGTH_HORIZONT);
                    if (hit.collider != null) //������ ����� ����
                    {
                        horizontalHitCollider = false;
                        return !IsTriggerBoxCollider2D(hit.collider.gameObject);
                    }
                    else
                    {
                        horizontalHitCollider = false;
                    }
                }
            }

            return horizontalHitCollider;//����� ���� ��� ��������� �� �������� ����, ������� ����� ��������� 0 ��� ��������.
        }
        return false; //���� ��� �������� �� ����������� - ������ �� �����������
    }
    //��������������� ����� ��� ���������� ���������� � IsTouchingWall
    //���������� true ���� ��� ������� ������� ���� ����������
    bool IsTriggerBoxCollider2D(GameObject gO)
    {

        //�������� �� ������� �������������� ��� ��������� ��������
        if (HasBoxCollider2D(gO))
        {
            Debug.Log("HasBoxCollider2D(gO)");
            if (gO.GetComponent<BoxCollider2D>().isTrigger == true)
            {
                Debug.Log("IsTriggerCollider2D");
                //ReactionItemWhithTriggerCollider2D(gO);//���� ��������� �������� �������� - 
                return true;//�.�. ���� ��������� �������� ��������
            }
            else
            {
                Debug.Log("IsBoxCollider2D");
                ReactionItemWhithBoxCollider2D(gO);
                return false;//����� ���� ��������� �� ������
            }
        }
        else
            return false;//���� ��� ���� ����������
    }
    //������� �� ������ � ��������������� (������ �������!!)
    void ReactionItemWhithBoxCollider2D(GameObject gO)
    {
        if (gO != null)
        {
            if (gO.CompareTag("Enemy"))
            {
                healthBar.GetComponent<HealthBar>().MinusHealth(gO.GetComponent<Enemy>().Damage());  //�������� ���� �� �����
            }
        }
    }
    //������� �� ������ � ������������������
    void ReactionItemWhithTriggerCollider2D(GameObject gO)
    {
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
        animator.SetBool("onGround", false);
        animator.SetBool("jump", true);
        animator.SetBool("fall", false);
    }
    void OnTriggerEnter2D(Collider2D collision)
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
        if (collision.gameObject.CompareTag("Kit"))
        {
            Debug.Log("Kit");
            healthBar.GetComponent<HealthBar>().PlusKitHealth(collision.gameObject);
        }
    }
}
