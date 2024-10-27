using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    public float moveSpeed;//скорость героя по горизонтали
    public float jumpForce; // Сила прыжка
    private Rigidbody2D rb; // Компонент Rigidbody2D
    private bool isGrounded; // Проверка на земле


    public bool keyDownW;//нажата ли W - вверх
    public bool keyDownA;
    public bool keyDownD;

    float moveInput;//ввод по горизонтали
    void Start()
    {
        moveSpeed = Constants.HERO_SPEED;
        jumpForce = Constants.HERO_JUMP_FORCE;
        rb = GetComponent<Rigidbody2D>();
        //moveInput = Input.GetAxis("Horizontal");
    }

    void Update()// видно выполняется только при прорисовке кадров, т.к. жудко тормозит.
    {

        /*        //сброс флагов нажатия, если отпустили кнопку// моя хуета
                CheckKeyUp(KeyCode.W, OnWKeyRelease);
                CheckKeyUp(KeyCode.Space, OnWKeyRelease);
                CheckKeyUp(KeyCode.UpArrow, OnWKeyRelease);
                CheckKeyUp(KeyCode.Keypad8, OnWKeyRelease);


                CheckKeyUp(KeyCode.A, OnAKeyRelease);
                CheckKeyUp(KeyCode.D, OnDKeyRelease);

                // проверка на нажатие кнопки
                CheckKeyDown(KeyCode.W, OnWKeyPress);
                CheckKeyDown(KeyCode.Space, OnWKeyPress);
                CheckKeyDown(KeyCode.UpArrow, OnWKeyPress);
                CheckKeyDown(KeyCode.Keypad8, OnWKeyPress);

                CheckKeyDown(KeyCode.A, OnAKeyPress);
                CheckKeyDown(KeyCode.D, OnDKeyPress);*/

        // Получаем ввод по горизонтали
        moveInput = Input.GetAxis("Horizontal");

        // Перемещение игрока
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Проверяем, нажата ли клавиша пробела и находится ли игрок на земле
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    /*    private void FixedUpdate()//через одинаковые промежутки времени выполняется (гладкое движение).
        {
            // действие на флаг нажатия кнопки и установка флагов
            if (keyDownW) { OnWKeyPress(); return; }

            if (keyDownA) { OnAKeyPress(); return; }
            if (keyDownD) { OnDKeyPress(); return; }
        }*/
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
    /*    void CheckKeyDown(KeyCode key, System.Action method)//проверка события нажатия кнопки
        {
            if (Input.GetKeyDown(key))
            {
                method();
            }
        }
        void CheckKeyUp(KeyCode key, System.Action method)//проверка события отпускания кнопки
        {
            if (Input.GetKeyUp(key))
            {
                method();
            }
        }
        // Добавьте здесь код, который нужно выполнить при нажатии клавиши W
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
            //Debug.Log("Клавиша W была отпущена");
            keyDownW = false;
        }

        public void OnAKeyRelease()
        {
            //Debug.Log("Клавиша A была отпущена");
            keyDownA = false;
        }
        public void OnDKeyRelease()
        {
            //Debug.Log("Клавиша D была отпущена");
            keyDownD = false;

        }*/
}
