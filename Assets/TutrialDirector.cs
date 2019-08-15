using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutrialDirector : MonoBehaviour
{
	public GameObject[] Tile;
	public bool tapp;
	int possibility;
	bool clear1,clear2,clear3;
	// Start is called before the first frame update
	void Start()
	{
		possibility = 100;
		TileContraller.Delaytime = 1.0f;
		clear1=false;
		clear2=false;
		clear3=false;

	}

	// Update is called once per frame
	void Update()
	{
        if (clear1==false){
    		if ((Input.GetMouseButtonDown(0))&(tapp==false))
	    	{
		    	TapAction();
		    	clear1=true;
	    	}
	    }
	    else if((clear1==true)&(clear2==false)){
	        clear2=true;
	    }else if((clear1==true)&(clear2==true)&(clear3==false)){
	        clear3=true;
	    }

	}

	private void Generate()
	{
		Tile = GameObject.FindGameObjectsWithTag("level 0");
		//Debug.Log(Tile.Length);
		//tilelen = Tile.Length;
		int number = Random.Range(0, Tile.Length);
		int x = Random.Range(0, 100);
		if (x<possibility) {

			Tile[number].gameObject.GetComponent<TileContraller>().level = 1;
			Tile[number].gameObject.GetComponent<Renderer>().material.color= TileContraller.Colors[1];
			//tilelen--;
		}
		tapp = false;
		//Tile = GameObject.FindGameObjectsWithTag("level 0");
	}

	void TapAction(){
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
			//Debug.Log(score);
	}

}


