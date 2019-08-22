using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TileContraller : MonoBehaviour
{
	public int Line,Raw,level;
	public static int maxLevel = 1,n,m;
	public static float Delaytime;
	public float height = 0.21f;
	public static List<Color> Colors = new List<Color>{new Color(1.0f,1.0f,1.0f,1.0f),new Color(1.0f, 0.5f, 0.20f, 1.0f),new Color(1.0f, 0.35f,0, 1.0f),new Color(0, 0.62f, 0.28f, 1.0f),
		new Color(0,0.42f,1.0f,1.0f),new Color(0.35f, 0.14f,1.0f, 1.0f),
		new Color(1.0f,0.22f,0.42f,1.0f),new Color(1.0f, 0,0, 1.0f)};
	int edge;//{0:真ん中の回転　1:端の回転 2:角の回転}
	 List<Vector3> Rotate = new List<Vector3> {new Vector3(1,0,0), new Vector3(0,0,1), new Vector3(0, 0, 1),
		new Vector3(-1, 0, 0),new Vector3(-1,0,0), new Vector3(0,0,-1),new Vector3(0,0,-1),new Vector3(1,0,0)};
	public static Vector2Int start;
	public AudioClip tileUp;
	public AudioClip tileMove;
	private AudioSource audioSource;
	public GameObject levelPrefab;
	GameObject Second;
	bool StageUp;
	public bool LevelUp;

	void Start()
	{
		GetComponent<Renderer>().material.color = Color.white;
		audioSource = GetComponent<AudioSource>();
		Line = (int)(2.5f - transform.position.z);
		Raw = (int)(2.5f + transform.position.x);
		//level = Line*4+Raw-5;//色見たいときに level0 コメントアウトして
		level = 0;
		GameObject Director = GameObject.Find("GameDirector");
		Delaytime = PlayerPrefs.GetFloat("speed",0.04f);
		StageUp=false;
		LevelUp=false;
	}
	void Update()
	{
		if (level == 0) tag = "level 0";
		else tag = "not level 0";

		if(LevelUp){
			if(level<=7) {
				Destroy(Second);
				StageUp=false;
				GetComponent<Renderer>().material.color = Colors[level];
			}
			else if(level<12) {
				if(StageUp==false) {
					Second=Instantiate(levelPrefab);
					Second.transform.localScale=new Vector3(0.7f,0.01f,0.7f);
					StageUp=true;
				}
				Second.transform.position=new Vector3(transform.position.x,0.1f,transform.position.z);
				if(level!=11){
					GetComponent<Renderer>().material.color = Colors[(level-8)*2+2];
					Second.GetComponent<Renderer>().material.color=Colors[(level-8)*2+1];
				}else{
					GetComponent<Renderer>().material.color = Colors[(level-8)*2+1];
					Second.GetComponent<Renderer>().material.color=Colors[0];
				}
			}else if(level==12){
				Destroy(Second);
				StageUp=false;
				GetComponent<Renderer>().material.color = Color.black;
			}else if((level>=13)&(level<=21)){
				if(StageUp==false) {
					Second=Instantiate(levelPrefab);
					Second.transform.localScale=new Vector3(0.7f,0.01f,0.7f);
					StageUp=true;
				}
				GetComponent<Renderer>().material.color = Color.black;
				Second.transform.position=new Vector3(transform.position.x,0.1f,transform.position.z);
				Second.GetComponent<Renderer>().material.color=Colors[level-14];
			}
		}
		LevelUp=false;
	}

	public void OnAwake(int Line, int Raw)//タップされたタイルから動くタイルへ
	{
		start = new Vector2Int(Line, Raw);
		GameObject next;
		edge = 1;
		if ((Line == 1 | Line == 4) & (Raw == 1 | Raw == 4)) edge = 2;
		if ((((Line != 1)) & (Line != 4)) & ((Raw != 1) & (Raw != 4)))
		{
			edge = 0;
			start.x += 1;
			n = 0;
		}
		else if((Line != 4) & (Raw == 1)){
			start.x += 1;
			n = 0;
		}
		else if((Line == 1) & (Raw != 1))
		{
			start.y -= 1;
			n = 12;
		}
		else if ((Line != 1) & (Raw == 4))
		{
			start.x -= 1;
			n = 8;
		}
		else if ((Line == 4) & (Raw != 4))
		{
			start.y += 1;
			n = 4;
		}

		m = ColorController(start.x, start.y)+1;

		//next=null;
		next = GameObject.Find(string.Format("Tile{0}-{1}",start.x , start.y));//名前がTile{start.x}-{start.y}のタイルをサーチ
		if(next!=null) next.GetComponent<TileContraller>().Rotation();

	}

	public void Rotation()
	{
		if ((n%2 == 0)&(m>0))
		{
			m--;
			audioSource.PlayOneShot(tileMove);
			transform.Translate(0, height, 0);
			GetComponent<Collider>().isTrigger = false;

			int NextLine = (int)(2.5f - transform.position.z - Rotate[n / 2].z);
			int NextRaw = (int)(2.5f + transform.position.x + Rotate[n / 2].x);

			if ((NextLine>=1)&(4>=NextLine)&(NextRaw>=1)&(4>=NextRaw)&(m!=0)) {
				transform.Translate(Rotate[n / 2]);
				LevelUp=true;
				Invoke("MiddleContraller", Delaytime);
			}
			else
			{
				Invoke("EdgeContraller", Delaytime);
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		n++;
		if(n >= 16) n -= 16;
		other.gameObject.GetComponent<TileContraller>().Rotation();
	}

	void MiddleContraller()
	{
		transform.Translate(0, -height, 0);
		Line = (int)(2.5f - transform.position.z);
		Raw = (int)(2.5f + transform.position.x);
		this.name = string.Format("Tile{0}-{1}", Line, Raw);
		Invoke("TriggerOn", Delaytime*0.1f);
	}

	void EdgeContraller()
	{
		int FormerLine = Line, FormerRaw = Raw;
		Line = start.x;
		Raw = start.y;

		transform.position = new Vector3(Raw - 2.5f, 0, 2.5f - Line);

		this.name = string.Format("Tile{0}-{1}", Line, Raw);
		if((level != 0)&((FormerLine != Line) | (FormerRaw != Raw)) )
		{
			GameObject Former = GameObject.Find(string.Format("Tile{0}-{1}", FormerLine, FormerRaw));
			if (Former.gameObject.GetComponent<TileContraller>().level == 0)
			{
				Former.gameObject.GetComponent<TileContraller>().level = level;
				Former.gameObject.GetComponent<TileContraller>().LevelUp=true;
			}
			else if(Former.gameObject.GetComponent<TileContraller>().level == level)
			{
				audioSource.PlayOneShot(tileUp);
				Former.gameObject.GetComponent<TileContraller>().level++;
				GameDirector.score += (int)Math.Pow(2, level);
				Former.gameObject.GetComponent<TileContraller>().LevelUp=true;
				if (maxLevel < level+1) {
					GameObject Director = GameObject.Find("GameDirector");
					maxLevel = level+1;
					Director.GetComponent<GameDirector>().leveldesign(maxLevel);
				}
			}
			if(level>8) {
				Destroy(Second);
				StageUp=false;
			}
			level = 0;
			LevelUp=true;
		}
		Invoke("TriggerOn", Delaytime * 0.1f);
	}

	void TriggerOn()
	{
		GetComponent<Collider>().isTrigger = true;
	}

	int ColorController(int a, int b)
	{
		int l = n / 2, i, k;
		GameObject surround;
		int[] surroundlevel = new int[5];

		if (edge == 1)
		{
			k = 5;
		}
		else if (edge == 2)
		{
			k = 3;
		}
		else return 10;

		surround = GameObject.Find(string.Format("Tile{0}-{1}", a, b));
		surroundlevel[0] = surround.GetComponent<TileContraller>().level;

		for (i = 1; i < k; i++)
		{
			if (l + i - 1 >= 8) l -= 8;
			a -= (int)Rotate[l + i - 1].z;
			b += (int)Rotate[l + i - 1].x;
			surround = GameObject.Find(string.Format("Tile{0}-{1}", a, b));
			surroundlevel[i] = surround.GetComponent<TileContraller>().level;
		}

		if (surroundlevel[k - 1] == 0) return k - 1;
		for (i = k - 1; i != 0; i--)
		{
			if ((surroundlevel[i-1] == 0) | (surroundlevel[i] == surroundlevel[i - 1]))
			{
				return i;
			}
		}
		return 0;
	}



}