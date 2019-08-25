using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class GameDirector : MonoBehaviour
{
	public GameObject[] Tile;
	bool tapp,lenchange;
	int possibility=100,tilelen,highscore,noad;
	float passtime,speed;
	public bool gameover;
	public static int score;
	public int adfrequency=1;
	public GameObject ScoreText,textGameOver,textResultScore,textResultLevel,levelPrefab,blackPrefab;
	private string key = "HIGH SCORE";
	public Button Restart;
	private AudioSource audioSource;

	void Start()
	{
		if((PlayerPrefs.GetFloat("speed",0.04f))>0.06f) PlayerPrefs.SetFloat("speed",0.04f);
		//Advertisement.Initialize("3263089",false);
		audioSource = GetComponent<AudioSource>();
		textGameOver.SetActive(false);
		possibility = 100;
		score = 0;
		Generate();//タイルを生む
		Generate();
		highscore = PlayerPrefs.GetInt(key,0);
		leveldesign(1);
		TileContraller.maxLevel=1;
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
				hit.collider.gameObject.GetComponent<TileContraller>().OnAwake
					(hit.collider.gameObject.GetComponent<TileContraller>().Line,
					hit.collider.gameObject.GetComponent<TileContraller>().Raw);
				if(TileContraller.Delaytime<0.04)Invoke("Generate", TileContraller.Delaytime*13);
				else if(TileContraller.Delaytime==0.04)Invoke("Generate", TileContraller.Delaytime*8);
				else Invoke("Generate",TileContraller.Delaytime*10);
			}
			else//タイル以外をタップした場合
			{
				tapp = false;
			}
		}

		if(lenchange){
			if((16-tilelen<=TileContraller.maxLevel)&(TileContraller.maxLevel<8)){
				possibility=100;
			}else if (tilelen <= 5)
			{
				possibility = 70;
			}
			else if ((6 <= tilelen) & (tilelen <= 10))
			{
				possibility = 50;
			}
			lenchange=false;
		}


		if ((tapp == true)&(gameover==false))//タップ無効状態が0.8秒以上続いたときの対策
		{
			passtime += Time.deltaTime;
			if(passtime >= 0.8f)
			{
				tapp = false;
				passtime = 0;
			}
		}

		if (((tilelen == 0)&(gameover == false))|(TileContraller.maxLevel>=15))
		{
			//Restart.enabled=false;
			noad = PlayerPrefs.GetInt("AD", 0);
			tapp=true;
			textGameOver.SetActive(true);
			if(TileContraller.maxLevel>=15) textGameOver.GetComponent<Text>().text="Clear";
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
			if(noad>=adfrequency){
				//Invoke("ShowAd",2.5f);
				PlayerPrefs.SetInt("AD",0);
			}else{
				noad++;
				Restart.enabled=true;
				PlayerPrefs.SetInt("AD",noad);
			}
		}
	}

	void Generate()
	{
		Tile = GameObject.FindGameObjectsWithTag("level 0");//白色のタイル
		tilelen = Tile.Length;//配列の長さ
		int number = Random.Range(0, tilelen);
		int x = Random.Range(0, 100);
		if (x<possibility) {
			Tile[number].gameObject.GetComponent<TileContraller>().level = 1;
			Tile[number].gameObject.GetComponent<TileContraller>().LevelUp=true;
			tilelen--;
			lenchange=true;
		}
		tapp = false;
	}

	public void leveldesign(int level){
		GameObject block,black,block2;
		block = Instantiate(levelPrefab);
		black = Instantiate(blackPrefab);
		if(level<10){
			block.transform.position = new Vector3(-2.5f+level/2.0f, 0 ,-2.8f);
			black.transform.position = new Vector3(-2.5f+level/2.0f, 0 ,-2.8f);
		}
		else if(level<19){
			block.transform.position = new Vector3(-7.0f+level/2.0f, 0 ,-3.4f);
			black.transform.position = new Vector3(-7.0f+level/2.0f, 0 ,-3.4f);
		}else{
			block.transform.position = new Vector3(-11.5f+level/2.0f, 0 ,-4.0f);
			black.transform.position = new Vector3(-11.5f+level/2.0f, 0 ,-4.0f);
		}
		if(level<=7) block.GetComponent<Renderer>().material.color= TileContraller.Colors[level];
		else if(level<11){
			block2=Instantiate(levelPrefab);
			block2.transform.position=new Vector3(block.transform.position.x,0.01f,block.transform.position.z);
			block2.transform.localScale=new Vector3(0.28f,0.01f,0.28f);
			if(level<9){
				block.GetComponent<Renderer>().material.color = TileContraller.Colors[7];
				block2.GetComponent<Renderer>().material.color=TileContraller.Colors[level-8];
			}else{
				block.GetComponent<Renderer>().material.color = TileContraller.Colors[7];
				block2.GetComponent<Renderer>().material.color= TileContraller.Colors[level*2-15];
			}
		}else if(level==11){
			block.GetComponent<Renderer>().material.color= Color.black;
		}else if((level>=12)&(level<=15)){
			block2=Instantiate(levelPrefab);
			block.GetComponent<Renderer>().material.color= Color.black;
			block2.transform.position=new Vector3(block.transform.position.x,0.01f,block.transform.position.z);
			switch(level){
				case 12:
					block2.GetComponent<Renderer>().material.color=TileContraller.Colors[0];
					break;
				case 13:
					block2.GetComponent<Renderer>().material.color=TileContraller.Colors[4];
					break;
				case 14:
					block2.GetComponent<Renderer>().material.color=TileContraller.Colors[6];
					break;
				case 15:
					block2.GetComponent<Renderer>().material.color=TileContraller.Colors[7];
					break;
				}
			block2.transform.localScale=new Vector3(0.28f,0.01f,0.28f);
		}
	}

	/*void ShowAd(){
		if(Advertisement.IsReady(){
			Advertisement.Show();
		}
		Restart.enabled=true;
	}*/
}
