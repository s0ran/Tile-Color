using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
	public GameObject[] Tile;
	public bool tapp;
	int possibility,tilelen;
	float passtime;
	public bool gameover;
	public static int score;
	public GameObject ScoreText;
    public GameObject textGameOver;
    //private Transform _camTransform;
    //private float _positionStep = 2.0f;
    // Start is called before the first frame update
    void Start()
	{
		textGameOver.SetActive(false);
		possibility = 100;
		score = 0;
		Generate();
		//Tile = GameObject.FindGameObjectsWithTag("level 0");
		Generate();
        //textGameOver.GetComponent<Animator>().SetBool("toCamera", false);
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


		if (tapp == true)
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
            /*Debug.Log("GameOver");
			gameover = true;
			SceneManager.LoadScene("GameOverScene");*/
            textGameOver.GetComponent<Animator>().SetTrigger("isGameOver");
           // textGameOver.GetComponent<Animator>().SetBool("toCamera",true);
            GameObject camera = GameObject.Find("Main Camera");
            //if(textGameOver.GetComponent<Animator>().GetBool("toCamera"))
            camera.GetComponent<Animator>().SetTrigger("isGameOverCamera");
            /*Vector3 campos = _camTransform.position;
            campos += _camTransform.right * Time.deltaTime * _positionStep;
            */
            /* GameObject camera = GameObject.Find("Main Camera");
             camera.gameObject.transform.position = Vector3.Lerp(camera.transform.position, new Vector3(4.5f, 7.0f, 0), Time.deltaTime * 2);*/
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
}
