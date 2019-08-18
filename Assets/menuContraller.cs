using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menuContraller : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
		gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SceneReload()
	{
		SceneManager.LoadScene("GameScene");
	}

	public void Titleload()
	{
		SceneManager.LoadScene("TitleScene");
	}
}
