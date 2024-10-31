using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb; // Компонент Rigidbody2D
    private GameObject keysIndicator;//указывает на индикатор ключей
    private GameObject healthBar;//здоровье
    private RaycastHit2D hit2, hit22, hit222;//переменные луча
    private float speedVelocityY;//переменная значения вектора по Y (во избежание переназначения)
    private float maxVelY;//максимальное значение velocity.y до падения
    private bool wasDamage;// получил ли урон от падения
    private Animator animator;

    public float jumpForce; // Сила прыжка
    public float moveSpeed; // Скорость перемещения

    public bool isGrounded; // Проверка на земле (для наглядности)
    public float moveInput, moveInput2;//коэффициеэнт движения (переменная)
                                       //доп переменные
    public bool horizontalHitCollider;//переменная столкновения горизонтального луча коллайдером (любым)
    public bool verticalDownHitCollider;//переменная столкновения  вертикального луча 2 с коллайдером (любым)

    float right;//переменная для направления движения;
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
        Move(); // Вынесли перемещение по горизонтали в отдельный метод

        // Прыжок
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Keypad8))
        {
            if (isGrounded)
                Jump();
        }

        AboveWall();// На поверхности
    }
    //перемещение игрока по горизонтали
    void Move()
    {
        // Получаем ввод по горизонтали
        moveInput = Input.GetAxis("Horizontal");
        //поворот
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
        // Проверка на столкновение с стенами
        if (!IsTouchingWall(moveInput))//нет присоединенной стены
        {
            // Перемещение игрока
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
            if (moveInput != 0)
                animator.SetBool("goHorizontal", true);
            // animator.SetBool("jump", false);

        }
        else//иначе не двигаемся по горизонтали* (есть помеха по горизонтали)
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
        GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);//вернуть цвет
        yield return null;
    }

    //логика падения на поверхность сверху
    //назначает isGrounded
    void AboveWall()
    {
        hit2 = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y - 0.9376f, 0f), Vector2.down, Constants.BEAM_LENGTH_VERTICAL);//
        hit22 = Physics2D.Raycast(new Vector3(transform.position.x - 0.6875f, transform.position.y - 0.9376f, 0f), Vector2.down, Constants.BEAM_LENGTH_VERTICAL);//2 луч 
        hit222 = Physics2D.Raycast(new Vector3(transform.position.x + 0.6875f, transform.position.y - 0.9376f, 0f), Vector2.down, Constants.BEAM_LENGTH_VERTICAL);//3й луч
        Debug.Log("AboveWall() ");


        speedVelocityY = rb.velocity.y;//текущее значение

        //столкнулись с каким-то коллайдером
        if (hit2.collider != null || hit22.collider != null || hit222.collider != null)
        {

            Debug.Log("colliders != null");

            if (speedVelocityY <= Constants.MAX_SPEED_VECTOR_VELOCITY_NO_DAMAGE)
            {//выводим только нужные значения велосити
                Debug.Log("speedVelocityY: " + speedVelocityY);
                Debug.Log("maxVelY: " + maxVelY);
            }
            verticalDownHitCollider = true;//было столкновение с коллайдером
            if (!IsTriggerBoxCollider2D(AimObject(hit2, hit22, hit222)))//не тригер
            {
                Debug.Log("коллайдер, но не триггер бокс коллайдер ");
                // Если упал на бокс коллайдер
                if (HasBoxCollider2D(AimObject(hit2, hit22, hit222)))
                {
                    Debug.Log("BoxCollider!");
                    ReactionItemWhithBoxCollider2D(AimObject(hit2, hit22, hit222));
                }
                else
                {
                    Debug.Log("Коллайдер поверхности");

                }
                isGrounded = true; //значит на твердой поверхности 
                //если упал с высоты на Скорости - получи урон
                wasDamage = healthBar.GetComponent<HealthBar>().CheckFallDamage(Mathf.RoundToInt(maxVelY));//чтобы сохраненное значение обработать
                if (wasDamage)
                {
                    maxVelY = 0; //Обнулили максимальный вектор
                }
                animator.SetBool("onGround", true);
                animator.SetBool("jump", false);
                animator.SetBool("fall", false);
            }
            else
            {
                Debug.Log("тригер БоксКоллайдер");
                isGrounded = false;
                verticalDownHitCollider = false;
                animator.SetBool("fall", true);
                animator.SetBool("onGround", false);
                animator.SetBool("jump", false);
            }
        }
        else
        {
            //свободный полет
            isGrounded = false;
            if (speedVelocityY < maxVelY) { maxVelY = speedVelocityY; }//запоминаем максимальную скорость
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
    //вспомогательный метод для SearchingItemWhithBoxCollider2D() 
    //метод возвращающий объект пересекающий один из лучей 
    GameObject AimObject(RaycastHit2D hit2, RaycastHit2D hit22, RaycastHit2D hit222)
    {
        if (hit2.collider != null) { return hit2.collider.gameObject; }
        else
        if (hit22.collider != null) { return hit22.collider.gameObject; }
        else
        if (hit222.collider != null) return hit222.collider.gameObject;
        return null;//возвращаем null если нет объекта
    }
    //столкнулись со стеной или нет?
    bool IsTouchingWall(float moveInput)
    {
        Debug.Log("IsTouchingWall");
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

            RaycastHit2D hit = Physics2D.Raycast(new Vector3(transform.position.x + (right * Constants.BEAM_DISPLACEMENT), transform.position.y, 0f), direction, Constants.BEAM_LENGTH_HORIZONT);
            // Если есть столкновение со стеной, возвращаем true
            if (hit.collider != null) //значит что-то с коллайдером есть по направлению луча
            {
                horizontalHitCollider = true; //метка -"нет коллайдера" приобрела значение есть коллайдер
                                              //проверка на наличие боксколлайдера для одиночных объектов
                return !IsTriggerBoxCollider2D(hit.collider.gameObject);
            }
            else //если нет объекта с коллайдером по центру
            {
                Debug.Log("else 1");
                horizontalHitCollider = false;//нет коллайдера столкнувшегося с лучом
                                              //кидаем второй луч сверху
                hit = Physics2D.Raycast(new Vector3(transform.position.x + (right * Constants.BEAM_DISPLACEMENT), transform.position.y + 0.5001f, 0f), direction, Constants.BEAM_LENGTH_HORIZONT);
                if (hit.collider != null) //значит коллайдер есть вверху
                {
                    horizontalHitCollider = true;
                    return !IsTriggerBoxCollider2D(hit.collider.gameObject);
                }
                else
                {
                    Debug.Log("else 2");
                    horizontalHitCollider = false;
                    //кидаем третий луч снизу
                    hit = Physics2D.Raycast(new Vector3(transform.position.x + (right * Constants.BEAM_DISPLACEMENT), transform.position.y - 0.9376f, 0f), direction, Constants.BEAM_LENGTH_HORIZONT);
                    if (hit.collider != null) //значит стена есть
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

            return horizontalHitCollider;//после того как проверили на движение вбок, которое может равняться 0 при движении.
        }
        return false; //если нет движения по горизонтали - значит не столкнулись
    }
    //вспомогательный метод для одинаковых вычислений в IsTouchingWall
    //возвращает true если луч пересек триггер бокс коллайдера
    bool IsTriggerBoxCollider2D(GameObject gO)
    {

        //проверка на наличие боксколлайдера для одиночных объектов
        if (HasBoxCollider2D(gO))
        {
            Debug.Log("HasBoxCollider2D(gO)");
            if (gO.GetComponent<BoxCollider2D>().isTrigger == true)
            {
                Debug.Log("IsTriggerCollider2D");
                //ReactionItemWhithTriggerCollider2D(gO);//если коллайдер является тригером - 
                return true;//т.е. бокс коллайдер является тригером
            }
            else
            {
                Debug.Log("IsBoxCollider2D");
                ReactionItemWhithBoxCollider2D(gO);
                return false;//иначе бокс коллайдер не тригер
            }
        }
        else
            return false;//если нет бокс коллайдера
    }
    //реакция на объект с боксколлайдером (только жесткие!!)
    void ReactionItemWhithBoxCollider2D(GameObject gO)
    {
        if (gO != null)
        {
            if (gO.CompareTag("Enemy"))
            {
                healthBar.GetComponent<HealthBar>().MinusHealth(gO.GetComponent<Enemy>().Damage());  //получить урон от врага
            }
        }
    }
    //реакция на объект с триггерколлайдером
    void ReactionItemWhithTriggerCollider2D(GameObject gO)
    {
    }

    //проверяет наличие BoxCollider2D на объекте
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
        // Применяем силу прыжка
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        animator.SetBool("onGround", false);
        animator.SetBool("jump", true);
        animator.SetBool("fall", false);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Проверяем столкновение с ключом
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
        // Проверяем столкновение с дверью
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
