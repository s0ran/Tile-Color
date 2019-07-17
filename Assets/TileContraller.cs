using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TileContraller : MonoBehaviour
{
    public int Line;
    public int Raw;
    public static int n,m;
    public int level;
    public static float Delaytime = 0.05f;
    public float height = 0.21f;
    public static List<Color> Colors = new List<Color>{new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f, 0.9f, 0.2f, 1.0f),new Color(0, 1.0f,1.0f, 1.0f), new Color(0, 1.0f, 0, 1.0f),
        new Color(0, 0.3f, 0, 1.0f),new Color(0,0,1.0f,1.0f),new Color(1.0f,0,0,1.0f),new Color(0.5f, 0,1.0f, 0.5f),new Color(1.0f, 0,1.0f, 1.0f)};
    int edge;//{0:端の回転　1:真ん中の回転}
    List<Vector3> Rotate = new List<Vector3> {new Vector3(1,0,0), new Vector3(0,0,1), new Vector3(0, 0, 1),
        new Vector3(-1, 0, 0),new Vector3(-1,0,0), new Vector3(0,0,-1),new Vector3(0,0,-1),new Vector3(1,0,0)};
    public static Vector2Int start;

    void Start()
    {
        GetComponent<Renderer>().material.color = Color.white;
        Line = (int)(2.5f　-　transform.position.z);
        Raw = (int)(2.5f + transform.position.x);
        level = 0;
    }
    void Update()
    {
        GetComponent<Renderer>().material.color = Colors[level];
        if (level == 0) tag = "level 0";
        else tag = "not level 0";
    }

    public void OnAwake(int Line, int Raw)
    {
        start = new Vector2Int(Line, Raw);
        GameObject next;
        edge = 1;
        if ((Line == 1 | Line == 4) & (Raw == 1 | Raw == 4)) edge = 2;
        if ((((Line != 1)) & (Line != 4)) & ((Raw != 1) & (Raw != 4)))
        {
            edge = 0;
            start.x += 1;
            n = 0;
        }
        else if((Line != 4) & (Raw == 1)){
            start.x += 1;
            n = 0;
        }
        else if((Line == 1) & (Raw != 1))
        {
            start.y -= 1;
            n = 12;
        }
        else if ((Line != 1) & (Raw == 4))
        {
            start.x -= 1;
            n = 8;
        }
        else if ((Line == 4) & (Raw != 4))
        {
            start.y += 1;
            n = 4;
        }

        //if (edge != 0)
        //{
            //Debug.Log(ColorController(Line + start.x, Raw + start.y));
        m = ColorController(start.x, start.y)+1;
        //Debug.Log(m);
        //}
        next = GameObject.Find(string.Format("Tile{0}-{1}",start.x , start.y)) ;
        next.GetComponent<TileContraller>().Rotation();
    }

    public void Rotation()
    {
        if ((n%2 == 0)&(m>0))
        {
            m--;
            transform.Translate(0, height, 0);
            GetComponent<Collider>().isTrigger = false;

            int NextLine = (int)(2.5f - transform.position.z - Rotate[n / 2].z);
            int NextRaw = (int)(2.5f + transform.position.x + Rotate[n / 2].x);

            if ((NextLine>=1)&(4>=NextLine)&(NextRaw>=1)&(4>=NextRaw)&(m!=0)) {
                transform.Translate(Rotate[n / 2]);
                Invoke("MiddleContraller", Delaytime);
            }
            else
            {
                //Line = NextLine;
                //Raw = NextRaw;
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
        //transform.Translate(Rotate[n / 2]);
        transform.Translate(0, -height, 0);
        //GetComponent<Renderer>().material.color = Colors[level];
        Line = (int)(2.5f - transform.position.z);
        Raw = (int)(2.5f + transform.position.x);
        this.name = string.Format("Tile{0}-{1}", Line, Raw);
        Invoke("TriggerOn", Delaytime*0.1f);
    }

    void EdgeContraller()
    {
        int FormerLine = Line;
        int FormerRaw = Raw;

        /*if (((Line == 0) & (Raw != 2)) | ((Line == 2) & (Raw == 5))){
            Line = 1;
            Raw -= 2;
        }else if (((Line != 3) & (Raw == 0)) | ((Line == 0) & (Raw == 2)))
        {
            Line += 2;
            Raw = 1;
        }else if (((Line == 5) & (Raw != 3)) | ((Line == 3) & (Raw == 0)))
        {
            Line = 4;
            Raw += 2;
        }else if (((Line != 2) & (Raw == 5)) | ((Line == 5) & (Raw == 3)))
        {
            Line -= 2;
            Raw = 4;
        }*/
        Line = start.x;
        Raw = start.y;

        transform.position = new Vector3(Raw - 2.5f, 0, 2.5f - Line);
        
        this.name = string.Format("Tile{0}-{1}", Line, Raw);
        if((level != 0)&((FormerLine != Line) | (FormerRaw != Raw)) )
        {
            /*if (FormerLine == 0) FormerLine++;
            else if (FormerLine == 5) FormerLine--;
            else if (FormerRaw == 0) FormerRaw++;
            else FormerRaw--;*/
            GameObject Former = GameObject.Find(string.Format("Tile{0}-{1}", FormerLine, FormerRaw));
            if (Former.gameObject.GetComponent<TileContraller>().level == 0)
            {
                Former.gameObject.GetComponent<TileContraller>().level = level;
            }
            else if(Former.gameObject.GetComponent<TileContraller>().level == level)
            {
                Former.gameObject.GetComponent<TileContraller>().level++;
            }
            level = 0;
        }
        Invoke("TriggerOn", Delaytime * 0.1f);
    }

    void TriggerOn()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    int ColorController(int a, int b)
    {
        int l = n / 2, i, k;
        GameObject surround;
        int[] surroundlevel = new int[5];

        if (edge == 1)
        {
            k = 5;
        }
        else if (edge == 2)
        {
            k = 3;
        }
        else return 10;
        //Debug.Log(k);

        surround = GameObject.Find(string.Format("Tile{0}-{1}", a, b));
        surroundlevel[0] = surround.GetComponent<TileContraller>().level;
        //Debug.Log(string.Format("Tile{0}-{1}", a, b));

        for (i = 1; i < k; i++)
        {
            if (l + i - 1 >= 8) l -= 8;
            a -= (int)Rotate[l + i - 1].z;
            b += (int)Rotate[l + i - 1].x;
            surround = GameObject.Find(string.Format("Tile{0}-{1}", a, b));
            surroundlevel[i] = surround.GetComponent<TileContraller>().level;
            //Debug.Log(surroundlevel[i]);
        }

        if (surroundlevel[k - 1] == 0) return k - 1;
        for (i = k - 1; i != 0; i--)
        {
            if ((surroundlevel[i-1] == 0) | (surroundlevel[i] == surroundlevel[i - 1]))
            {
                return i;
            }
        }
        return 0;
        
    }

}