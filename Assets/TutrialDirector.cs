using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutrialDirector : MonoBehaviour
{
	public GameObject[] Tile;
	public bool tapp;
	int possibility;
	bool clear1,clear2,endrotation=false,nottile;
	public Text Text1,Text2,Text3,Text4,Text5,Task;
	public Button next;
	Text nextText;
	// Start is called before the first frame update
	void Start()
	{
		//int i;
		possibility = 100;
		TileContraller.Delaytime = 0.7f;
		clear1=false;
		clear2=false;
		Text1.enabled=false;
		Text2.enabled = false;
		Text3.enabled = false;
		Text4.enabled= false;
		Text5.enabled = false;
		next.interactable = false;
		Task.text = "タイルをタップしよう";
		nextText = next.GetComponentInChildren<Text>();

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
				next.interactable=true;
				Task.text = "クリア";
			}
	    }
	    else if((clear1==true)&(clear2==false)){
	    	Task.text = "色を合体させて色を進化させてみよう（色が変わるよ）";
			Text3.enabled = true;
			Text4.enabled = true;
			if ((Input.GetMouseButtonDown(0)) & (tapp == false))
			{
				TapAction();
			}
			if(TileContraller.maxLevel==2){
				clear2 =true;
				next.interactable=true;
				Task.text = "クリア";
			}
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
		if(clear1==false){
			int i,j;
			Tile = GameObject.FindGameObjectsWithTag("not level 0");
    	    for (i = 0; i < Tile.Length; i++)
			{
    	        Tile[i].gameObject.GetComponent<TileContraller>().level = 0;
			}
			Text1.enabled = false;
			Text2.enabled = false;
			clear1 = true;
			next.interactable=false;
			Tile = new GameObject[16];
			for (i = 1; i <= 4; i++)
			{
				for (j = 1; j <= 4; j++)
				{
					Tile[4 * (i - 1) + j - 1] = GameObject.Find(string.Format("Tile{0}-{1}", i, j));
					if ((i == 1) | (i == 4) | (j == 1) | (j == 4)) Tile[4 * (i - 1) + j - 1].gameObject.GetComponent<TileContraller>().level = 1;
				}
			}
		}else if(nextText.text=="次へ"){
			nextText.text="終了";
			Text3.enabled =false;
			Text4.enabled = false;
			Text5.enabled = true;
			Task.text = "チュートリアル完了";
		}else if(nextText.text=="終了"){
			SceneManager.LoadScene("TitleScene");
		}
	}

}


