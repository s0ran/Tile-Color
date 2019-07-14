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
    public static float Delaytime=0.05f;
    public static float height = 0.21f;
    public static List<Color> Colors = new List<Color>{new Color(1.0f,1.0f,1.0f,1.0f),new Color(1.0f,0,0,1.0f),new Color(0,1.0f,0,1.0f)};

    void Start()
    {
        GetComponent<Renderer>().material.color = Color.white;
        Line = (int)(2.5f　-　transform.position.z);
        Raw = (int)(2.5f + transform.position.x);
        level = 0;
        Debug.Log("Start");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnAwake(int Line, int Raw)
    { 
        if (((((Line != 1)) & (Line != 4)) & ((Raw != 1) & (Raw != 4)))| ((Line != 4) & (Raw == 1)))
        {
            GameObject next = GameObject.Find(string.Format("Tile{0}-{1}", Line + 1, Raw));
            n = 0;
            next.GetComponent<TileContraller>().Rotation();
        }else if((Line == 1) & (Raw != 1))
        {
            GameObject next = GameObject.Find(string.Format("Tile{0}-{1}", Line, Raw - 1));
            n = 12;
            next.GetComponent<TileContraller>().Rotation();
        }else if ((Line != 1) & (Raw == 4))
        {
            GameObject next = GameObject.Find(string.Format("Tile{0}-{1}", Line-1, Raw));
            n = 8;
            next.GetComponent<TileContraller>().Rotation();
        }else if ((Line == 4) & (Raw != 4))
        {
            GameObject next = GameObject.Find(string.Format("Tile{0}-{1}", Line, Raw + 1));
            n = 4;
            next.GetComponent<TileContraller>().Rotation();
        }
    }

    public void Rotation()
    {
        if (n%2 == 0)
        {
            List<Vector3> Rotate = new List<Vector3> {new Vector3(1,0,0), new Vector3(0,0,1), new Vector3(0, 0, 1),
        new Vector3(-1, 0, 0),new Vector3(-1,0,0), new Vector3(0,0,-1),new Vector3(0,0,-1),new Vector3(1,0,0)};
           
            transform.Translate(0, height, 0);
            GetComponent<Collider>().isTrigger = false;

            int NextLine = (int)(2.5f - transform.position.z - Rotate[n / 2].z);
            int NextRaw = (int)(2.5f + transform.position.x + Rotate[n / 2].x);

            if ((NextLine>=1)&(4>=NextLine)&(NextRaw>=1)&(4>=NextRaw)) {
                transform.Translate(Rotate[n / 2]);
                Invoke("MiddleContraller", Delaytime);
            }
            else
            {
                Line = NextLine;
                Raw = NextRaw;
                Invoke("EdgeContraller", Delaytime);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        n++;
        if(n >= 16) n -= 16;
        other.gameObject.GetComponent<TileContraller>().Rotation();
    }

    void MiddleContraller()
    {
        //level += 1;
        transform.Translate(0, -height, 0);
        GetComponent<Renderer>().material.color = Colors[level];
        Line = (int)(2.5f - transform.position.z);
        Raw = (int)(2.5f + transform.position.x);
        this.name = string.Format("Tile{0}-{1}", Line, Raw);
        Invoke("TriggerOn", Delaytime*0.1f);
    }

    void EdgeContraller()
    {
        level = 0;
        GetComponent<Renderer>().material.color = Colors[level];
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
        this.name = string.Format("Tile{0}-{1}", Line, Raw);
        Invoke("TriggerOn", Delaytime * 0.1f);
    }

    void TriggerOn()
    {
        GetComponent<Collider>().isTrigger = true;
    }
}


