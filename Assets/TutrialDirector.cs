﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutrialDirector : MonoBehaviour
{
	public GameObject[] Tile;
	public bool tapp;
	int possibility;
	bool clear1,clear21,clear22,clear23,clear24,clear31,clear41,endrotation=false,nottile;
	public Text Text1,Text2,Text23,Text3,Text4,Text5,Task;
	public Button next;
	public Sprite nextimage,endimage;
    public GameObject yajirusi2;
    public GameObject yajirusi1,yajirusi3;
	// Start is called before the first frame update
	void Start()
	{
		//int i;
		possibility = 100;
		TileContraller.Delaytime = 0.6f;
		clear1=false;
        clear21 = false;
        clear22 = false;
        clear23 = false;
        clear24 = false;
		clear31=false;
        clear41 = false;
		Text1.enabled=false;
		Text2.enabled = false;
        Text23.enabled = false;
		Text3.enabled = false;
		Text4.enabled= false;
		Text5.enabled = false;
		next.interactable = false;
		Task.text = "黄色いタイルを\nタップしよう";
		next.GetComponent<Image>().sprite=nextimage;
        Tile[6].gameObject.GetComponent<TileContraller>().level = 1;
        TileContraller.maxLevel = 100;


    }

	// Update is called once per frame
	void Update()
	{
        if (clear1 == false)
        {

            if ((Input.GetMouseButtonDown(0)) & (tapp == false))
            {
                TapAction(1);
                if (nottile != true)
                {
                    Text1.enabled = true;
                    Text1.GetComponent<Animator>().SetTrigger("isText1");
                }
            }
            if (endrotation == true)
            {
                Text2.enabled = true;
                Text2.GetComponent<Animator>().SetTrigger("isText2");
                endrotation = false;
                next.interactable = true;
                Task.text = "クリア";
            }
        }
        else if ((clear1 == true) & (clear21 == false))
        {
            Task.text = "端にある黄色いタイルをタップしよう";
            //Text23.enabled = true;
            if ((Input.GetMouseButtonDown(0)) & (tapp == false))
            {
                /*yajirusi.gameObject.SetActive(true);
                yajirusi.GetComponent<Animator>().SetTrigger("isLeft");*/
                TapAction(2);
            }
            if (endrotation == true)
            {
                Text23.enabled = true;
                endrotation = false;
                next.interactable = true;
                clear21 = true;
                //Task.text = "クリア";


            }
        }
        else if ((clear23 == true)&(clear22==false))
        {
            if ((Input.GetMouseButtonDown(0)) & (tapp == false))
            {
                /*yajirusi.gameObject.SetActive(true);
                yajirusi.GetComponent<Animator>().SetTrigger("isLeft");*/
                TapAction(3);
            }
            if (endrotation == true)
            {
                //Text23.enabled = true;
                endrotation = false;
                next.interactable = true;
                clear24 = true;
                Task.text = "クリア";
            }
        }
        else if ((clear1 == true) & (clear22 == true) & (clear31 == false))
        {
            Task.text = "色を合体させて色を進化させてみよう（色が変わるよ）";
            Text3.enabled = true;
            Text3.GetComponent<Animator>().SetTrigger("isText3");
            Text4.enabled = true;
            Text4.GetComponent<Animator>().SetTrigger("isText4");
            if ((Input.GetMouseButtonDown(0)) & (tapp == false))
            {
                TapAction(4);
            }
            if (TileContraller.TutrialClear == true)
            {
                clear31 = true;
                next.interactable = true;
                Task.text = "クリア";
            }
        }

	}

	private void Generate()
	{
		Tile = GameObject.FindGameObjectsWithTag("level 0");
        //Debug.Log(Tile.Length);
        //tilelen = Tile.Length;
        //int number = Random.Range(0, Tile.Length);
        int number = Random.Range(0, Tile.Length);
        int x = Random.Range(0, 100);

        //Debug.Log(x);
        if (x < possibility)
        {
            //Debug.Log(number);
            Tile[number].gameObject.GetComponent<TileContraller>().level = 1;
            //Debug.Log(Tile[number].gameObject.name);
            //Debug.Log(Tile[number].gameObject.GetComponent<TileContraller>().level);
            Tile[number].gameObject.GetComponent<Renderer>().material.color = TileContraller.Colors[1];

        }
        tapp = false;
		endrotation = true;
		//Tile = GameObject.FindGameObjectsWithTag("level 0");
	}

	void TapAction(int p){
	    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			tapp = true;
			if (Physics.Raycast(ray, out hit, Mathf.Infinity))
			{
                if(hit.collider.gameObject.CompareTag("not level 0")) {
                switch (p)
                {
                    case 1:
                        yajirusi3.gameObject.SetActive(true);
                        yajirusi3.GetComponent<Animator>().SetTrigger("isYajirusi3");
                        break;
                    case 2:
                        yajirusi2.gameObject.SetActive(true);
                        yajirusi2.GetComponent<Animator>().SetTrigger("isLeft");
                        break;
                    case 3:
                        yajirusi1.gameObject.SetActive(true);
                        yajirusi1.GetComponent<Animator>().SetTrigger("isYajirusi1");
                        break;
                }
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
        if (clear1 == false) {
            yajirusi3.gameObject.SetActive(false);
            int k;
            Tile = GameObject.FindGameObjectsWithTag("not level 0");
            for (k = 0; k < Tile.Length; k++)
            {
                Tile[k].gameObject.GetComponent<TileContraller>().level = 0;
            }
            Text1.enabled = false;
            Text2.enabled = false;
            clear1 = true;
            next.interactable = false;
            Tile = new GameObject[16];
            Tile[7] = GameObject.Find(string.Format("Tile2-4"));
            //Tile[4] = GameObject.Find(string.Format("Tile2-1"));
            Tile[7].gameObject.GetComponent<TileContraller>().level = 1;
            //Tile[4].gameObject.GetComponent<TileContraller>().level = 1;
        }
        if (clear21 == true)
        {
            yajirusi2.gameObject.SetActive(false);
            int u;
            Tile = GameObject.FindGameObjectsWithTag("not level 0");
            for (u = 0; u < Tile.Length; u++)
            {
                Tile[u].gameObject.GetComponent<TileContraller>().level = 0;
            }
            clear23 = true;
            next.interactable = false;
            Tile = new GameObject[16];
            Tile[12] = GameObject.Find(string.Format("Tile4-1"));
            Tile[12].gameObject.GetComponent<TileContraller>().level = 1;
        }
        if ((clear1 == true) & (clear24 == true))
        {
            yajirusi1.gameObject.SetActive(false);
            //Task.text = "色を合体させて色を進化させてみよう（色が変わるよ）";
            int i, j;
            Tile = GameObject.FindGameObjectsWithTag("not level 0");
            for (i = 0; i < Tile.Length; i++)
            {
                Tile[i].gameObject.GetComponent<TileContraller>().level = 0;
            }
            //Text1.enabled = false;
            Text23.enabled = false;
            clear22 = true;
            next.interactable = false;
            Tile = new GameObject[16];
            for (i = 1; i <= 4; i++)
            {
                for (j = 1; j <= 4; j++)
                {
                    Tile[4 * (i - 1) + j - 1] = GameObject.Find(string.Format("Tile{0}-{1}", i, j));
                    if ((i == 1) | (i == 4) | (j == 1) | (j == 4)) Tile[4 * (i - 1) + j - 1].gameObject.GetComponent<TileContraller>().level = 1;
                }
            }
        }
        if (clear41 == true)
        {
            SceneManager.LoadScene("TitleScene");
        }
        if (clear31==true){
			next.GetComponent<Image>().sprite=endimage;
			Text3.enabled =false;
			Text4.enabled = false;
			Text5.enabled = true;
            Text5.GetComponent<Animator>().SetTrigger("isText5");
			Task.text = "チュートリアル完了";
            clear41 = true;
            next.interactable = true;
        }
        
    }

}


