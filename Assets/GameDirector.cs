using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    public GameObject[] Tile;
    public bool tapp;
    int possibility;
    float passtime;
    // Start is called before the first frame update
    void Start()
    {
        possibility = 100;
        Generate();
        Generate();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetMouseButtonDown(0))&(tapp==false))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            tapp = true;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                hit.collider.gameObject.GetComponent<TileContraller>().OnAwake
                (hit.collider.gameObject.GetComponent<TileContraller>().Line,
                    hit.collider.gameObject.GetComponent<TileContraller>().Raw);
                Invoke("Generate", TileContraller.Delaytime * 9);
            }
            else
            {
                tapp = false;
            }
            
        }

        if (Tile.Length <= 5)
        {
            possibility = 10;
        }
        else if((6<=Tile.Length)&(Tile.Length <= 10))
        {
            possibility = 70;
        }


        if (tapp == true)
        {
            passtime += Time.deltaTime;
            if(passtime >= 1.0f)
            {
                tapp = false;
                passtime = 0;
            }
        }
    }

    void Generate()
    {
        Tile = GameObject.FindGameObjectsWithTag("level 0");
        int number = Random.Range(0, Tile.Length);
        int x = Random.Range(0, 100);
        if (x<possibility) {
            Tile[number].gameObject.GetComponent<TileContraller>().level = 1;
        }
        tapp = false;
    }
}
