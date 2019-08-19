using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleDirector : MonoBehaviour
{
    public Text HighScore;
    private int highscore;
    private string key = "HIGH SCORE";

    public GameObject soundOptionCanvas;
    public GameObject SettingButton;
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
    }

    public void CloseDown()
    {
        soundOptionCanvas.SetActive(false);
        SettingButton.SetActive(true);
    }
}
