using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class TitleDirector : MonoBehaviour
{
	public Text HighScore;
	private string key = "HIGH SCORE";
	public GameObject SettingButton,StartButton,YarikataButton,Panel;
	public AudioMixer audioMixer;
	public Button Slow,Standard,High;
	public Slider BGM,SE;
	// Start is called before the first frame update
	void Start()
	{
		int highscore;
		highscore = PlayerPrefs.GetInt(key,0);
		HighScore.text = "HIGH SCORE: " + highscore.ToString();
		BGM.value = PlayerPrefs.GetFloat("BGMVol",0);
		SE.value = PlayerPrefs.GetFloat("SEVol",0);
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void InvokeStart()
	{
		Invoke("StartButtonDown", 1);
	}

	public void InvokeYarikata()
	{
		Invoke("YarikataDown", 1);
	}

	public void StartButtonDown(){
		SceneManager.LoadScene("GameScene");
	}

	public void YarikataDown()
	{
		SceneManager.LoadScene("TutrialScene");
	}

	public void SettingDown()
	{
		Panel.SetActive(true);
		SettingButton.SetActive(false);
		StartButton.SetActive(false);
		YarikataButton.SetActive(false);
		float speed = PlayerPrefs.GetFloat("speed",0.04f);
		BGM.value = PlayerPrefs.GetFloat("BGMVol",0);
		SE.value = PlayerPrefs.GetFloat("SEVol",0);
		if(speed==0.04f) Standard.Select();
		else if(speed == 0.03f) High.Select();
		else Slow.Select();
	}

	public void CloseDown()
	{
		Panel.SetActive(false);
		SettingButton.SetActive(true);
		StartButton.SetActive(true);
		YarikataButton.SetActive(true);
	}

	public void SetBGM(float volume)
	{
		audioMixer.SetFloat("BGMVol", volume);
		PlayerPrefs.SetFloat("BGMVol",volume);
		PlayerPrefs.Save();
	}

	public void SetSE(float volume)
	{
		audioMixer.SetFloat("SEVol",volume);
		PlayerPrefs.SetFloat("SEVol",volume);
		PlayerPrefs.Save();
	}

	public void SlowButtonDown(){
		PlayerPrefs.SetFloat("speed",0.06f);
		PlayerPrefs.Save();
	}
	public void StandardButtonDown(){
		PlayerPrefs.SetFloat("speed",0.04f);
		PlayerPrefs.Save();
	}
	public void HighButtonDown(){
		PlayerPrefs.SetFloat("speed",0.03f);
		PlayerPrefs.Save();
	}
}
