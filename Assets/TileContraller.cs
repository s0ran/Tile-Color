using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TileContraller : MonoBehaviour
{
    public int Line;
    public int Raw;
    public static int n;
    public Vector3 Initialpos;
    public float Delaytime = 0.1f;

    void Start()
    {
        //GetComponent<BoxCollider>().isTrigger = false;
        GetComponent<Renderer>().material.color = Color.white;
        Line = (int)(2.5f-transform.position.z);
        Raw = (int)(2.5f + transform.position.x);
        Initialpos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        //n = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Awake(int Line, int Raw)
    {
        //Debug.Log(this.Line);
        //Debug.Log(string.Format("Tile{0}-{1}", Line + 1, Raw));
        //Debug.Log();
        if (((Line != 1) & (Line != 4)) & ((Raw != 1) & (Raw != 4)))
        {
            GameObject next = GameObject.Find(string.Format("Tile{0}-{1}", Line + 1, Raw));
            n = 0;
            next.GetComponent<TileContraller>().ChangeColor(n);
        }
    }

    public void ChangeColor(int i)
    {
        if (n%2 == 0)
        {
            List<Vector3> Rotate = new List<Vector3> {new Vector3(1,0,0), new Vector3(0,0,1), new Vector3(0, 0, 1),
        new Vector3(-1, 0, 0),new Vector3(-1,0,0), new Vector3(0,0,-1),new Vector3(0,0,-1),new Vector3(1,0,0)};
           
            transform.Translate(0, 0.1f, 0);
            GetComponent<Collider>().isTrigger = false;
            transform.Translate(Rotate[i/2]);
            Invoke("DelayMethod", Delaytime);
            Invoke("MoreDelay", Delaytime * 10);
            //Debug.Log(this.Raw);
            //GetComponent<Renderer>().material.color = Color.red;
            //DelayMethod(Initialpos);
            //n++;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit");
        n++;
        Debug.Log(n);
        other.gameObject.GetComponent<TileContraller>().ChangeColor(n);
    }

    void DelayMethod()
    {
        Debug.Log("Delay");
        transform.Translate(0, -0.1f, 0);
        GetComponent<Renderer>().material.color = Color.red;

        transform.position = Initialpos;

        //n++;
        //yield return new WaitForSeconds(1.0f);
        //transform.Translate(pos);
    }

    void MoreDelay()
    {
        GetComponent<Collider>().isTrigger = true;
    }
}


