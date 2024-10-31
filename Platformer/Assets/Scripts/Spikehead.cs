using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikehead : MonoBehaviour
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
}
