using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
        }
    }
}
