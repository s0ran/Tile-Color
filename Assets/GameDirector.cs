using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    public GameObject[] Tile;
    bool tapp;
    int possibility;
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
            if(Physics.Raycast(ray,out hit, Mathf.Infinity))
            {
                hit.collider.gameObject.GetComponent<TileContraller>().OnAwake
                (hit.collider.gameObject.GetComponent<TileContraller>().Line,
                    hit.collider.gameObject.GetComponent<TileContraller>().Raw);
                Invoke("Generate", TileContraller.Delaytime * 9);
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
    }

    void Generate()
    {
        tapp = false;
        Tile = GameObject.FindGameObjectsWithTag("level 0");
        int number = Random.Range(0, Tile.Length);
        int x = Random.Range(0, 100);
        if (x<possibility) {
            Tile[number].gameObject.GetComponent<TileContraller>().level = 1;
        }
    }
}
