using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circle : MonoBehaviour
{
    public GameObject gObject;

    private float speed;

    // Use this for initialization
    void Start()
    {
        speed = 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v = new Vector3(0, 0, 1);
        transform.RotateAround(gObject.transform.position, v, speed);
    }
}
