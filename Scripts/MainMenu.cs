using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Text HightScoretxt;
   
   
    void Start()
    {
        Time.timeScale = 1f;
        HightScoretxt.text = "HIGHT SCORE : " + PlayerPrefs.GetFloat("HighScore", 0f);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    public void exit()
    {
        Application.Quit();
    }

    
}
