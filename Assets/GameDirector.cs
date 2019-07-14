using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    public GameObject[] Tile;
    // Start is called before the first frame update
    void Start()
    {
        Generate();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit, Mathf.Infinity))
            {
                hit.collider.gameObject.GetComponent<TileContraller>().OnAwake
                (hit.collider.gameObject.GetComponent<TileContraller>().Line,
                    hit.collider.gameObject.GetComponent<TileContraller>().Raw);
            }
            Invoke("Generate",TileContraller.Delaytime*9);
        }
    }

    void Generate()
    {
        Tile = GameObject.FindGameObjectsWithTag("level 0");
        int number = Random.Range(0, Tile.Length);
        Tile[number].gameObject.GetComponent<TileContraller>().level = 1;
    }
}
