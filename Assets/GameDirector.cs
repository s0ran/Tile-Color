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
	float passtime;
	public bool gameover;
	public static int score;
	public int adfrequency=1;
	public GameObject ScoreText,textGameOver,textResultScore,textResultLevel,levelPrefab,blackPrefab;
	private string key = "HIGH SCORE";
	public Button Restart;
	private AudioSource audioSource;
	// Start is called before the first frame update
	void Start()
	{
		audioSource = GetComponent<AudioSource>();
		textGameOver.SetActive(false);
		possibility = 100;
		score = 0;
		Generate();//タイルを生む
		Generate();
		//Debug.Log("start");
		highscore = PlayerPrefs.GetInt(key,0);
		leveldesign(1);
		//tilelen=0;
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
				//audioSource.PlayOneShot(tileMove);
				hit.collider.gameObject.GetComponent<TileContraller>().OnAwake//タップしたタイルのコンポーネントを取得
				(hit.collider.gameObject.GetComponent<TileContraller>().Line,
					hit.collider.gameObject.GetComponent<TileContraller>().Raw);
				Invoke("Generate", TileContraller.Delaytime*12);
			}
			else//タイル以外をタップした場合
			{
				tapp = false;
			}
			//ScoreText.GetComponent<Text>().text = "Score:  "+score;
			//Debug.Log(score);
		}

		if(lenchange){
			if (tilelen <= 5)
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

		if (((tilelen == 0)&(gameover == false))|(TileContraller.maxLevel>=21))
		{
			Restart.enabled=false;
			noad = PlayerPrefs.GetInt("AD", 0);
			tapp=true;
			textGameOver.SetActive(true);
			if(TileContraller.maxLevel>=21) textGameOver.GetComponent<Text>().text="Clear";
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
		else if(level<12){
			block2=Instantiate(levelPrefab);
			block2.transform.position=new Vector3(block.transform.position.x,block.transform.position.y+0.01f,block.transform.position.z);
			block2.transform.localScale=new Vector3(0.28f,0.01f,0.28f);
			if(level!=11){
				block.GetComponent<Renderer>().material.color = TileContraller.Colors[(level-8)*2+2];
				block2.GetComponent<Renderer>().material.color=TileContraller.Colors[(level-8)*2+1];
			}else{
				block.GetComponent<Renderer>().material.color = TileContraller.Colors[(level-8)*2+1];
				block2.GetComponent<Renderer>().material.color=Color.white;
			}
		}else if(level==12){
			block.GetComponent<Renderer>().material.color= Color.black;
		}else if((level>=13)&(level<=21)){
			block2=Instantiate(levelPrefab);
			block.GetComponent<Renderer>().material.color= Color.black;
			block2.transform.position=new Vector3(block.transform.position.x,block.transform.position.y+0.01f,block.transform.position.z);
			block2.GetComponent<Renderer>().material.color=TileContraller.Colors[level-14];
			block2.transform.localScale=new Vector3(0.28f,0.01f,0.28f);
		}
	}

	/*oid ShowAd(){
		if(Advertisement.IsReady()){
			Advertisement.Show();
		}
		Restart.enabled=true;
	}*/
}
