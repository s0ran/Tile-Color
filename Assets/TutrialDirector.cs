using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutrialDirector : MonoBehaviour
{
	public GameObject[] Tile;
	public bool tapp;
	int possibility;
	bool clear1,clear2,clear3,endrotation=false,nottile;
	public Text Text1,Text2,Text3;
	// Start is called before the first frame update
	void Start()
	{
		//int i;
		possibility = 100;
		TileContraller.Delaytime = 0.7f;
		clear1=false;
		clear2=false;
		clear3=false;
		Text1.enabled=false;
		Text2.enabled = false;
		Text3.enabled = false;
		/*for (i = 0; i < Sentence.Length;i++)
		{
			Sentence[i].SetActive(false);
		}*/

	}

	// Update is called once per frame
	void Update()
	{
        if (clear1==false){
    		if ((Input.GetMouseButtonDown(0))&(tapp==false))
	    	{
		    	TapAction();
				if(nottile!=true) Text1.enabled=true;				
	    	}
			if (endrotation == true)
			{
				Text2.enabled = true;
				endrotation = false;
			}
	    }
	    else if((clear1==true)&(clear2==false)){
			Text3.enabled = true;
			if ((Input.GetMouseButtonDown(0)) & (tapp == false))
			{
				TapAction();
			}
			if(TileContraller.maxLevel==2) clear2 =true;
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
		endrotation = true;
		//Tile = GameObject.FindGameObjectsWithTag("level 0");
	}

	void TapAction(){
	    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			tapp = true;
			if (Physics.Raycast(ray, out hit, Mathf.Infinity))
			{
			nottile = false;
			hit.collider.gameObject.GetComponent<TileContraller>().OnAwake
				(hit.collider.gameObject.GetComponent<TileContraller>().Line,
					hit.collider.gameObject.GetComponent<TileContraller>().Raw);
				Invoke("Generate", TileContraller.Delaytime * 9);
			}
			else
			{
				tapp = false;
			    nottile = true;
			}
			//Debug.Log(score);
	}

    public void NextButtonDown()
	{
		int i,j;
		Tile = GameObject.FindGameObjectsWithTag("not level 0");
        for (i = 0; i < Tile.Length; i++)
		{
            Tile[i].gameObject.GetComponent<TileContraller>().level = 0;
		}
		Text1.enabled = false;
		Text2.enabled = false;
		clear1 = true;
		Tile = new GameObject[16];
		for (i = 1; i <= 4; i++)
		{
			for (j = 1; j <= 4; j++)
			{
				Tile[4 * (i - 1) + j - 1] = GameObject.Find(string.Format("Tile{0}-{1}", i, j));
				if ((i == 1) | (i == 4) | (j == 1) | (j == 4)) Tile[4 * (i - 1) + j - 1].gameObject.GetComponent<TileContraller>().level = 1;
			}
		}
		//Tile[0].gameObject.GetComponent<TileContraller>().level = 1;
		//Tile[0].gameObject.GetComponent<Renderer>().material.color = TileContraller.Colors[1];
		//Tile[1].gameObject.GetComponent<TileContraller>().level = 1;
		//Tile[1].gameObject.GetComponent<Renderer>().material.color = TileContraller.Colors[1];
	}

}


