﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
	public GameObject[] Tile;
	public bool tapp;
	int possibility=100,tilelen,highscore;
	float passtime;
	public bool gameover;
	public static int score;
	public GameObject ScoreText,textGameOver,textResultScore,textResultLevel,levelPrefab,blackPrefab;
    private string key = "HIGH SCORE";

    //public AudioClip tileMove;

    private AudioSource audioSource;
    //private Transform _camTransform;
    //private float _positionStep = 2.0f;
    // Start is called before the first frame update
    void Start()
	{
        audioSource = GetComponent<AudioSource>();
		textGameOver.SetActive(false);
		possibility = 100;
		score = 0;
		Generate();//タイルを生む
		//Tile = GameObject.FindGa meObjectsWithTag("level 0");
		Generate();
        //textGameOver.GetComponent<Animator>().SetBool("toCamera", false);
        highscore = PlayerPrefs.GetInt(key,0);
        leveldesign(1);
    }


	// Update is called once per frame
	void Update()
	{
		ScoreText.GetComponent<Text>().text = "Score:  "+score;

		if ((Input.GetMouseButtonDown(0))&(tapp==false))
		{
			tapp = true;
            passtime = 0;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, Mathf.Infinity))//タップしたobjectの情報をhitに格納
			{
                //audioSource.PlayOneShot(tileMove);
                hit.collider.gameObject.GetComponent<TileContraller>().OnAwake//タップしたタイルのコンポーネントを取得
				(hit.collider.gameObject.GetComponent<TileContraller>().Line,
					hit.collider.gameObject.GetComponent<TileContraller>().Raw);
				Invoke("Generate", TileContraller.Delaytime * 10);
			}
			else//タイル以外をタップした場合
			{
				tapp = false;
			}
			//Debug.Log(score);
		}

		if (Tile.Length <= 5)
		{
			possibility = 70;
		}
		else if ((6 <= Tile.Length) & (Tile.Length <= 10))
		{
			possibility = 50;
		}


		if ((tapp == true)&(gameover==false))//タップ無効状態が1秒以上続いたときの対策
		{
			passtime += Time.deltaTime;
			if(passtime >= 1.0f)
			{
				tapp = false;
				passtime = 0;
			}
		}

		//Tile = GameObject.FindGameObjectsWithTag("level 0");
		if ((tilelen == 0)&(gameover == false))
		{
			tapp=true;
            textGameOver.SetActive(true);
			gameover = true;
            textGameOver.GetComponent<Animator>().SetTrigger("isGameOver");
            textResultScore.GetComponent<Text>().text = "Score:  " + score;
            GameObject camera = GameObject.Find("Main Camera");
            textResultLevel.GetComponent<Text>().text = "Level:  " + TileContraller.maxLevel;
            camera.GetComponent<Animator>().SetTrigger("isGameOverCamera");
            if(score>highscore){
            	PlayerPrefs.SetInt(key, score);
            	PlayerPrefs.Save();
            }
        }
	}

	void Generate()
	{
		Tile = GameObject.FindGameObjectsWithTag("level 0");//白色のタイル

		tilelen = Tile.Length;//配列の長さ
		int number = Random.Range(0, Tile.Length);
		int x = Random.Range(0, 100);
		Debug.Log(x);//デバッグ時に表示
		if (x<possibility) {
			Debug.Log(number);
			Tile[number].gameObject.GetComponent<TileContraller>().level = 1;//level番目のタイルのレベルを上げる
			Debug.Log(Tile[number].gameObject.name);
			Debug.Log(Tile[number].gameObject.GetComponent<TileContraller>().level);
			Tile[number].gameObject.GetComponent<Renderer>().material.color= TileContraller.Colors[1];//黄色にする
            tilelen--;
		}
		tapp = false;
		//Tile = GameObject.FindGameObjectsWithTag("level 0");
	}

	public void MenuButtonDown()
	{
		GameObject menu = GameObject.Find("menu");
		menu.transform.GetChild(1).gameObject.SetActive(true);
	}

	public void leveldesign(int level){
		//Debug.Log("into");
		GameObject block,black;
		block = Instantiate(levelPrefab);
		black =Instantiate(blackPrefab);
		if(level<=10){
			block.transform.position = new Vector3(-2.5f+level/2.0f, 0 ,-2.8f);
			black.transform.position = new Vector3(-2.5f+level/2.0f, 0 ,-2.8f);
		}
		else {
			block.transform.position = new Vector3(-7.5f+level/2.0f, 0 ,-3.4f);
			black.transform.position = new Vector3(-7.5f+level/2.0f, 0 ,-3.4f);
		}
		block.GetComponent<Renderer>().material.color= TileContraller.Colors[level];
	}
}
