using UnityEngine;

public class Platform : MonoBehaviour
{
    public float moovingLeft = 0f;//смещения
    public float moovingRight = 6f;
    public float borderLeft;//границы
    public float borderRight;
    public float speed;
    public bool mooveRight;//переключатель направления

    void Start()
    {
        borderLeft = transform.position.x - moovingLeft;
        borderRight = transform.position.x + moovingRight;
        speed = transform.gameObject.GetComponent<Platform>().speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (mooveRight)
        {
            if (transform.position.x < borderRight)
            {
                transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
            }
            else { mooveRight = !mooveRight; }
        }
        else
        {
            if (transform.position.x > borderLeft)
            {
                transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
            }
            else { mooveRight = !mooveRight; }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.transform.parent = transform;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.parent = null;
    }
}
