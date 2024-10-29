using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb; // Компонент Rigidbody2D
    private GameObject keysIndicator;//указывает на индикатор ключей
    private GameObject healthBar;//здоровье

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
        hitColliderNull = true;
        hit2ColliderNull = true;
        healthBar = GameObject.Find("HealthBar");
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

    void Move()
    {
        // Получаем ввод по горизонтали
        moveInput = Input.GetAxis("Horizontal");

        // Проверка на столкновение с стенами
        if (!IsTouchingWall(moveInput))//нет присоединенной стены
        {
            // Перемещение игрока
            rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        }
        else//иначе не двигаемся по горизонтали*
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
    }

    public void BlinkingRed(float immunityTime)
    {
        StartCoroutine(StartBlinkTimer(immunityTime));
    }
    private IEnumerator StartBlinkTimer(float time)
    {
        float timeRed = time;
        Debug.Log("Start blinktimer");
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
        Debug.Log("End blinktimer");
        GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);//назначить красный
        yield return null;
    }

    //логика падения на стену сверху
    void AboveWall()
    {
        RaycastHit2D hit2 = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y - 0.9376f, 0f), Vector2.down, Constants.BEAM_LENGTH_VERTICAL);//
        RaycastHit2D hit22 = Physics2D.Raycast(new Vector3(transform.position.x - 0.6875f, transform.position.y - 0.9376f, 0f), Vector2.down, Constants.BEAM_LENGTH_VERTICAL);//2 луч 
        RaycastHit2D hit222 = Physics2D.Raycast(new Vector3(transform.position.x + 0.6875f, transform.position.y - 0.9376f, 0f), Vector2.down, Constants.BEAM_LENGTH_VERTICAL);//3й луч
        Debug.Log("AboveWall() ");
        if (hit2.collider != null || hit22.collider != null || hit222.collider != null)
        {
            isGrounded = true;//значит на земле
            // Игрок на жесткой поверхности
            hit2ColliderNull = false;


            SearchingItemWhithBoxCollider2D(AimObject(hit2, hit22, hit222));
        }
        else
        {
            isGrounded = false;
            hit2ColliderNull = true;
        }
    }
    //вспомогательный метод для SearchingItemWhithBoxCollider2D() 
    //метод определяющий объект
    private GameObject AimObject(RaycastHit2D hit2, RaycastHit2D hit22, RaycastHit2D hit222)
    {
        if (hit2.collider != null) return hit2.collider.gameObject;
        if (hit22.collider != null) return hit22.collider.gameObject;
        if (hit222.collider != null) return hit222.collider.gameObject;
        return null;//возвращаем null если нет объекта
    }

    private bool IsTouchingWall(float moveInput)
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
                hitColliderNull = false;
                //проверка на наличие боксколлайдера для одиночных объектов
                return IsTriggerBoxCollider2D(hit.collider.gameObject);
            }
            else
            {
                Debug.Log("else 1");
                hitColliderNull = true;
                //кидаем второй луч сверху
                hit = Physics2D.Raycast(new Vector3(transform.position.x + (right * Constants.BEAM_DISPLACEMENT), transform.position.y + 0.5001f, 0f), direction, Constants.BEAM_LENGTH_HORIZONT);
                if (hit.collider != null) //значит стена есть
                {
                    hitColliderNull = false;
                    return IsTriggerBoxCollider2D(hit.collider.gameObject);
                }
                else
                {
                    Debug.Log("else 2");
                    hitColliderNull = true;
                    //кидаем третий луч снизу
                    hit = Physics2D.Raycast(new Vector3(transform.position.x + (right * Constants.BEAM_DISPLACEMENT), transform.position.y - 0.9376f, 0f), direction, Constants.BEAM_LENGTH_HORIZONT);
                    if (hit.collider != null) //значит стена есть
                    {
                        hitColliderNull = false;
                        return IsTriggerBoxCollider2D(hit.collider.gameObject);
                    }
                    else
                    {
                        hitColliderNull = true;
                    }
                }
            }

            return !hitColliderNull;//возвращаем есть ли стена
        }
        return false;
    }
    //вспомогательный метод для одинаковых вычислений в IsTouchingWall
    bool IsTriggerBoxCollider2D(GameObject gO)
    {
        Debug.Log("IsTriggerBoxCollider2D");
        //проверка на наличие боксколлайдера для одиночных объектов
        if (HasBoxCollider2D(gO))
        {
            Debug.Log("HasBoxCollider2D(gO)");
            if (gO.GetComponent<BoxCollider2D>().isTrigger == true)
            { return false; } //если коллайдер является тригером - возвращаем false (т.е. стены нет)
            else
            {
                SearchingItemWhithBoxCollider2D(gO);//определяем что за предмет
                return true;
            }
        }
        else return true;//если коллайдер но не бокс - стена 
    }
    //получаем урон от врага, если это он
    private void SearchingItemWhithBoxCollider2D(GameObject gO)
    {
        if (gO.CompareTag("Enemy"))
        {
            healthBar.GetComponent<HealthBar>().MinusHealth(gO.GetComponent<Enemy>().Damage());  //получить урон от врага
            //сделаться неуязвимым временно
        }
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
    }
}
