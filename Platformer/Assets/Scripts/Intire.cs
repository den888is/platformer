using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intire : MonoBehaviour
{
    public bool intireCollider; //внутри коллайдера
    private GameObject collisionObject;//ссылка на объект коллизии

    bool intire = false;
    Vector2[] vectors;
    GameObject[] objects = new GameObject[4];
    GameObject obj;//метка подоби€

    int ignoreLayer;
    int layerMask;

    private void Start()
    {
        vectors = new Vector2[4] { Vector2.down, Vector2.left, Vector2.up, Vector2.right };
        ignoreLayer = LayerMask.NameToLayer("Hero");
        layerMask = ~(1 << ignoreLayer);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Ќаходимс€ в коллизи€ с твердью");

            collisionObject = collision.gameObject;

            if (IntireOtherCollider()) //есл застр€ли - то
            {
                intireCollider = true;
                Debug.Log(transform.position);
                Debug.Log("point is inside collider");
                if (gameObject.GetComponent<Rigidbody2D>().velocity.y < Constants.MAX_SPEED_VECTOR_VELOCITY_NO_DAMAGE)
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y + 2f);
                    Debug.Log("2");
                }
                else
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y + 4f);//спрыгнули с коллизии
                    Debug.Log("4");
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("¬ышли из коллизи€ с твердью");
            if (collision.gameObject.Equals(collisionObject)) { intireCollider = false; collisionObject = null; }
        }
    }
    //внутри другого коллайдера
    public bool IntireOtherCollider()
    {

        //обнулить данные
        intire = false;
        obj = null;
        for (int i = 0; i < 4; i++)
        {
            objects[i] = null;
        }


        for (int i = 0; i < 4; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, vectors[i], 50f, layerMask);
            if (hit.collider != null && (hit.collider.isTrigger == false))
            {
                objects[i] = hit.collider.gameObject;
                Debug.Log("ѕопали в: " + hit.collider.gameObject);
            }
            else
            {
                Debug.Log("hit.collider == null || (hit.collider.isTrigger == true)");
                return false;
            }
        }

        //проверка на равенство границ
        obj = objects[0];
        if (obj != null)
        {
            for (int i = 1; i < 4; i++)
            {
                if (obj.Equals(objects[i])) { intire = true; }
                else { return false; }
            }
        }
        else { intire = false; }
        return intire;
    }
}

