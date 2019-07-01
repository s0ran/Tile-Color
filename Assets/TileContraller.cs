using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TileContraller : MonoBehaviour
{
    public int Line;
    public int Raw;

    void Start()
    {
        GetComponent<Renderer>().material.color = Color.white;
        Line = (int)(2.5f-transform.position.z);
        Raw = (int)(2.5f+transform.position.x);
        Debug.Log(this.Line);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Awake(int Line, int Raw)
    {
        //Debug.Log(this.Line);
        //Debug.Log(string.Format("Tile{0}-{1}", Line + 1, Raw));
        GameObject next = GameObject.Find(string.Format("Tile{0}-{1}", Line+1, Raw));
        next.GetComponent<TileContraller>().ChangeColor();
    }

    public void ChangeColor()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }
}


