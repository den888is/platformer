using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    public float moveSpeed;//�������� ����� �� �����������
    public float jumpForce; // ���� ������
    private Rigidbody2D rb; // ��������� Rigidbody2D
    private bool isGrounded; // �������� �� �����


    public bool keyDownW;//������ �� W - �����
    public bool keyDownA;
    public bool keyDownD;

    float moveInput;//���� �� �����������
    void Start()
    {
        moveSpeed = Constants.HERO_SPEED;
        jumpForce = Constants.HERO_JUMP_FORCE;
        rb = GetComponent<Rigidbody2D>();
        //moveInput = Input.GetAxis("Horizontal");
    }

    void Update()// ����� ����������� ������ ��� ���������� ������, �.�. ����� ��������.
    {

        /*        //����� ������ �������, ���� ��������� ������// ��� �����
                CheckKeyUp(KeyCode.W, OnWKeyRelease);
                CheckKeyUp(KeyCode.Space, OnWKeyRelease);
                CheckKeyUp(KeyCode.UpArrow, OnWKeyRelease);
                CheckKeyUp(KeyCode.Keypad8, OnWKeyRelease);


                CheckKeyUp(KeyCode.A, OnAKeyRelease);
                CheckKeyUp(KeyCode.D, OnDKeyRelease);

                // �������� �� ������� ������
                CheckKeyDown(KeyCode.W, OnWKeyPress);
                CheckKeyDown(KeyCode.Space, OnWKeyPress);
                CheckKeyDown(KeyCode.UpArrow, OnWKeyPress);
                CheckKeyDown(KeyCode.Keypad8, OnWKeyPress);

                CheckKeyDown(KeyCode.A, OnAKeyPress);
                CheckKeyDown(KeyCode.D, OnDKeyPress);*/

        // �������� ���� �� �����������
        moveInput = Input.GetAxis("Horizontal");

        // ����������� ������
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // ���������, ������ �� ������� ������� � ��������� �� ����� �� �����
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    /*    private void FixedUpdate()//����� ���������� ���������� ������� ����������� (������� ��������).
        {
            // �������� �� ���� ������� ������ � ��������� ������
            if (keyDownW) { OnWKeyPress(); return; }

            if (keyDownA) { OnAKeyPress(); return; }
            if (keyDownD) { OnDKeyPress(); return; }
        }*/
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
    /*    void CheckKeyDown(KeyCode key, System.Action method)//�������� ������� ������� ������
        {
            if (Input.GetKeyDown(key))
            {
                method();
            }
        }
        void CheckKeyUp(KeyCode key, System.Action method)//�������� ������� ���������� ������
        {
            if (Input.GetKeyUp(key))
            {
                method();
            }
        }
        // �������� ����� ���, ������� ����� ��������� ��� ������� ������� W
        public void OnWKeyPress()
        {

            if (isGrounded)
            {
                Jump();
            }
            keyDownW = true;
        }
        public void OnAKeyPress()
        {

            // transform.Translate(Vector3.left * speed * Time.fixedDeltaTime);
            rb.velocity = new Vector2(rb.velocity.x * -1, rb.velocity.y);
            keyDownA = true;
            keyDownD = false;
        }
        public void OnDKeyPress()
        {
            transform.Translate(Vector3.right * moveSpeed * Time.fixedDeltaTime);
            keyDownD = true;
            keyDownA = false;

        }
        public void OnWKeyRelease()
        {
            //Debug.Log("������� W ���� ��������");
            keyDownW = false;
        }

        public void OnAKeyRelease()
        {
            //Debug.Log("������� A ���� ��������");
            keyDownA = false;
        }
        public void OnDKeyRelease()
        {
            //Debug.Log("������� D ���� ��������");
            keyDownD = false;

        }*/
}
