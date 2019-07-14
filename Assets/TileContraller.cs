using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TileContraller : MonoBehaviour
{
    public int Line;
    public int Raw;
    public static int n;
    public int level;
    public Vector3 Initialpos;
    public float Delaytime = 0.2f;
    public static List<Color> Colors = new List<Color>{new Color(1.0f,1.0f,1.0f,1.0f),new Color(1.0f,0,0,1.0f),new Color(0,1.0f,0,1.0f)};

    void Start()
    {
        //GetComponent<BoxCollider>().isTrigger = false;
        GetComponent<Renderer>().material.color = Color.white;
        Line = (int)(2.5f-transform.position.z);
        Raw = (int)(2.5f + transform.position.x);
        Initialpos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        level = 0;
        //n = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnAwake(int Line, int Raw)
    {
        //Debug.Log(this.Line);
        //Debug.Log(string.Format("Tile{0}-{1}", Line + 1, Raw));
        //Debug.Log();
        if (((((Line != 1)) & (Line != 4)) & ((Raw != 1) & (Raw != 4)))| ((Line != 4) & (Raw == 1)))
        {
            GameObject next = GameObject.Find(string.Format("Tile{0}-{1}", Line + 1, Raw));
            n = 0;
            next.GetComponent<TileContraller>().ChangeColor();
        }else if((Line == 1) & (Raw != 1))
        {
            GameObject next = GameObject.Find(string.Format("Tile{0}-{1}", Line, Raw - 1));
            n = 12;
            next.GetComponent<TileContraller>().ChangeColor();
        }else if ((Line != 1) & (Raw == 4))
        {
            GameObject next = GameObject.Find(string.Format("Tile{0}-{1}", Line-1, Raw));
            n = 8;
            next.GetComponent<TileContraller>().ChangeColor();
        }else if ((Line == 4) & (Raw != 4))
        {
            GameObject next = GameObject.Find(string.Format("Tile{0}-{1}", Line, Raw + 1));
            n = 4;
            next.GetComponent<TileContraller>().ChangeColor();
        }
    }

    public void ChangeColor()
    {
        if (n%2 == 0)
        {
            List<Vector3> Rotate = new List<Vector3> {new Vector3(1,0,0), new Vector3(0,0,1), new Vector3(0, 0, 1),
        new Vector3(-1, 0, 0),new Vector3(-1,0,0), new Vector3(0,0,-1),new Vector3(0,0,-1),new Vector3(1,0,0)};
           
            transform.Translate(0, 0.1f, 0);
            GetComponent<Collider>().isTrigger = false;
            transform.Translate(Rotate[n/2]);
            Invoke("DelayMethod", Delaytime);
            //Invoke("MoreDelay", Delaytime * 10);
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
        if(n >= 16) n -= 16;
        //Debug.Log(n);
        other.gameObject.GetComponent<TileContraller>().ChangeColor();
    }

    void DelayMethod()
    {
        //level += 1;
        //Debug.Log("Delay");
        transform.Translate(0, -0.1f, 0);
        GetComponent<Renderer>().material.color = Colors[level];
        Line = (int)(2.5f - transform.position.z);
        Raw = (int)(2.5f + transform.position.x);
        if ((Line < 1) | (4 < Line) | (Raw < 1) | (4 < Raw)) Adjust();
        this.name = string.Format("Tile{0}-{1}", Line, Raw);
        //transform.position = Initialpos;
        GetComponent<Collider>().isTrigger = true;
        //n++;
        //yield return new WaitForSeconds(1.0f);
        //transform.Translate(pos);
    }

    void Adjust()
    {
        if (((Line == 0) & (Raw != 2)) | ((Line == 2) & (Raw == 5))){
            Line = 1;
            Raw -= 2;
            transform.position = new Vector3(Raw - 2.5f, 0, 2.5f - Line);
        }else if (((Line != 3) & (Raw == 0)) | ((Line == 0) & (Raw == 2)))
        {
            Line += 2;
            Raw = 1;
            transform.position = new Vector3(Raw - 2.5f, 0, 2.5f - Line);
        }else if (((Line == 5) & (Raw != 3)) | ((Line == 3) & (Raw == 0)))
        {
            Line = 4;
            Raw += 2;
            transform.position = new Vector3(Raw - 2.5f, 0, 2.5f - Line);
        }else if (((Line != 2) & (Raw == 5)) | ((Line == 5) & (Raw == 3)))
        {
            Line -= 2;
            Raw = 4;
            transform.position = new Vector3(Raw - 2.5f, 0, 2.5f - Line);
        }
    }

    /*void MoreDelay()
    {
        GetComponent<Collider>().isTrigger = true;
    }*/
}


