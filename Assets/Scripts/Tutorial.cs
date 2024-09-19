using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tutorial : MonoBehaviour
{
   
    

    [SerializeField] GameObject p1, p2, p3,p4, genel;
    private bool trigger = true;

    [SerializeField] AudioSource tutorialSound;

    private void Awake()
    {
        if(PlayerPrefs.GetInt("TutorialPanel",0) == 1)
        {
            Destroy(genel);
            Destroy(gameObject);
        }
    }
    void Start()
    {
        

    }


    void Update()
    {

       if(trigger) Time.timeScale = 0f;
        
        
    }


   

    public void Button1()
    {
            if(MainMenu.IsSoundOn) tutorialSound.Play();

            p1.SetActive(false);
            p2.SetActive(true);
        

       
    }




    public void Button2()
    {
        if (MainMenu.IsSoundOn) tutorialSound.Play();
        p2.SetActive(false);
            p3.SetActive(true);
        
        
    }
    public void Button3()
    {

        if (MainMenu.IsSoundOn) tutorialSound.Play();
        p3.SetActive(false);
        p4.SetActive(true);
        trigger = false;
    }
    public void Button4()
    {

        if (MainMenu.IsSoundOn) tutorialSound.Play();
        PlayerPrefs.SetInt("TutorialPanel", 1);
        Time.timeScale = 1f;
        
        Destroy(genel);
            
        
        

    }
}
