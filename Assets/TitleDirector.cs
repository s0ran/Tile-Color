using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class TitleDirector : MonoBehaviour
{
    public Text HighScore;
    private int highscore;
    private string key = "HIGH SCORE";

    public GameObject soundOptionCanvas;
    public GameObject SettingButton;
    public GameObject StartButton;
    public GameObject YarikataButton;
    public AudioMixer audioMixer;
    // Start is called before the first frame update
    void Start()
    {
        highscore = PlayerPrefs.GetInt(key,0);
        HighScore.text = "HIGH SCORE: " + highscore.ToString();
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
        //GameObject soundOptionCanvas = GameObject.Find("OptionCanvas");
        soundOptionCanvas.SetActive(true);
        SettingButton.SetActive(false);
        StartButton.SetActive(false);
        YarikataButton.SetActive(false);
    }

    public void CloseDown()
    {
        soundOptionCanvas.SetActive(false);
        SettingButton.SetActive(true);
        StartButton.SetActive(true);
        YarikataButton.SetActive(true);
    }

    public void SetBGM(float volume)
    {
        audioMixer.SetFloat("BGMVol", volume);
    }

    public void SetSE(float volume)
    {
        audioMixer.SetFloat("SEVol",volume);
    }
}
