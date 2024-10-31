using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public GameObject purpose;//цель
    void Start()
    {
        purpose = GameObject.Find("Hero");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(purpose.transform.position.x, purpose.transform.position.y, -50f);
    }
}
