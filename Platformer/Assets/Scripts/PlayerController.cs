using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb; // Компонент Rigidbody2D
    private GameObject keysIndicator;//указывает на индикатор ключей

    private bool pressedJump;//переменная нажатой кнопки прыжка
    public float jumpForce; // Сила прыжка
    public float moveSpeed; // Скорость перемещения

    public bool isGrounded; // Проверка на земле (для наглядности)
    public float moveInput, moveInput2;//коэффициеэнт движения (переменная)
                                       //доп переменные
    public bool hitColliderNull;//переменная столкновения луча
    public bool hit2ColliderNull;//переменная столкновения луча 2

    float right;//переменная для направления движения;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpForce = Constants.JUMP_FORCE;
        moveSpeed = Constants.MOOVE_SPEED;
        keysIndicator = GameObject.Find("KeysIndicator");
    }

    void Update()
    {
        Move(); // Вынесли перемещение в отдельный метод

        // Проверяем, нажата ли клавиша пробела и находится ли игрок на земле
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Keypad8))
        {
            if (isGrounded)
                Jump();
        }

        AboveWall();
    }
    /*    void CheckKeyDown(KeyCode key, System.Action method)//проверка события нажатия кнопки
        {
            if (Input.GetKeyDown(key))
            {
                method();
            }
        }
        bool CheckKeyDown(KeyCode key)//проверка события нажатия кнопки
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
        // Получаем ввод по горизонтали
        moveInput = Input.GetAxis("Horizontal");

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
    //логика падения на стену сверху
    void AboveWall()
    {
        RaycastHit2D hit2 = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y - 1, 0f), Vector2.down, 0.5f);//1.625f
        Debug.Log("AboveWall() ");
        if (hit2.collider != null)
        {
            // Debug.Log("hit2.collider name: " + (hit2.collider.gameObject.name));
            // Игрок на жесткой поверхности
            hit2ColliderNull = false;
            // Логика прыжка или приземления
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
        // Проверка на столкновение с стенами
        if (moveInput != 0)
        {
            // Определяем направление движения
            Vector2 direction = new Vector2(moveInput, 0);//растет при нажатии кнопки движения
            if (direction.x > 0) { right = 1f; }
            else
            {
                right = -1f;
            }
            RaycastHit2D hit = Physics2D.Raycast(new Vector3(transform.position.x + (right * Constants.BEAM_DISPLACEMENT), transform.position.y, 0f), direction, Constants.BEAM_LENGTH);
            // Если есть столкновение со стеной, возвращаем true
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
        // Применяем силу прыжка
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isGrounded = false; // Игрок теперь в воздухе
        pressedJump = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("OnCollisionEnter2D");
        // Проверяем столкновение с землей
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // Игрок на земле
            Debug.Log("OnCollisionEnter2D -  collision.gameObject.CompareTag(Ground): " + collision.gameObject.CompareTag("Ground"));
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("OnCollisionStay2D");
        // Проверяем столкновение с землей
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // Игрок на земле
        }
        // Проверяем столкновение с землей
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Wallll");
            isGrounded = true; // Игрок на земле ?
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Проверяем столкновение с ключом
        if (collision.gameObject.CompareTag("Key"))
        {
            if (keysIndicator != null)
            {
                keysIndicator.GetComponent<KeysIndicator>().KeyCountPlus();
                Destroy(collision.gameObject);
            }
            else { Debug.LogError("null - keyIndicator!!!"); }
        }
        // Проверяем столкновение с дверью
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
