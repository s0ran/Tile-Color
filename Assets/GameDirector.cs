using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
	public GameObject[] Tile;
	public bool tapp;
	int possibility,tilelen,highscore;
	float passtime;
	public bool gameover;
	public static int score;
	public GameObject ScoreText,textGameOver,textResultScore,textResultLevel,levelPrefab;
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
		Generate();
		//Tile = GameObject.FindGa meObjectsWithTag("level 0");
		Generate();
        //textGameOver.GetComponent<Animator>().SetBool("toCamera", false);
        highscore = PlayerPrefs.GetInt(key,0);
    }


	// Update is called once per frame
	void Update()
	{
		ScoreText.GetComponent<Text>().text = "Score:  "+score;

		if ((Input.GetMouseButtonDown(0))&(tapp==false))
		{

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			tapp = true;
			if (Physics.Raycast(ray, out hit, Mathf.Infinity))
			{
                //audioSource.PlayOneShot(tileMove);
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

		if (Tile.Length <= 5)
		{
			possibility = 70;
		}
		else if ((6 <= Tile.Length) & (Tile.Length <= 10))
		{
			possibility = 50;
		}


		if ((tapp == true)&(gameover==false))
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
            // textGameOver.GetComponent<Animator>().SetBool("toCamera",true);
            //GameObject camera = GameObject.Find("Main Camera");
            //if(textGameOver.GetComponent<Animator>().GetBool("toCamera"))
            camera.GetComponent<Animator>().SetTrigger("isGameOverCamera");
            if(score>highscore){
            	PlayerPrefs.SetInt(key, score);
            	PlayerPrefs.Save();
            }
        }
	}

	void Generate()
	{
		Tile = GameObject.FindGameObjectsWithTag("level 0");
		//Debug.Log(Tile.Length);
		tilelen = Tile.Length;
		int number = Random.Range(0, Tile.Length);
		int x = Random.Range(0, 100);
		if (x<possibility) {

			Tile[number].gameObject.GetComponent<TileContraller>().level = 1;
			Tile[number].gameObject.GetComponent<Renderer>().material.color= TileContraller.Colors[1];
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
		GameObject block;
		block = Instantiate(levelPrefab);
		block.transform.position = new Vector3(-3.0f+level, 0 ,-3.0f);
		block.GetComponent<Renderer>().material.color= TileContraller.Colors[level];
	}
}
