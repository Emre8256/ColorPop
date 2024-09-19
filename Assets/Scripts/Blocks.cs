using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Blocks : MonoBehaviour
{
    [SerializeField] private GameObject[] balls = new GameObject[4];
    [SerializeField] private ParticleSystem[] efektler = new ParticleSystem[4];
    [SerializeField] private GameObject[] Blockss = new GameObject[4];

    [SerializeField] private Sprite[] LongBlocks = new Sprite [4];
    [SerializeField] private Sprite[] ShortBlocks = new Sprite [4];

    // 0 = blue 1=red 2=green 3=yellow

    [SerializeField] AudioSource BlockSound;
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BlueBall" || collision.gameObject.tag =="GreenBall" || collision.gameObject.tag == "YellowBall" || collision.gameObject.tag =="RedBall" || collision.gameObject.tag == "Ball")
        {
           
            if (int.TryParse(this.gameObject.tag, out int index))
            {
                if (index == 0 & GameManager.aktifball != balls[0])
                {
                    efektler[0].Play();
                    if(MainMenu.IsSoundOn) BlockSound.Play();
                    this.gameObject.GetComponent<Image>().sprite = ShortBlocks[0];

                    Blockss[1].GetComponent<Image>().sprite = LongBlocks[1];
                    Blockss[2].GetComponent<Image>().sprite = LongBlocks[2];
                    Blockss[3].GetComponent<Image>().sprite = LongBlocks[3];
                    
                }
                else if (index == 1 & GameManager.aktifball != balls[1])
                {
                    efektler[1].Play();
                    if (MainMenu.IsSoundOn) BlockSound.Play();
                    this.gameObject.GetComponent<Image>().sprite = ShortBlocks[1];

                    Blockss[0].GetComponent<Image>().sprite = LongBlocks[0];
                    Blockss[2].GetComponent<Image>().sprite = LongBlocks[2];
                    Blockss[3].GetComponent<Image>().sprite = LongBlocks[3];
                }
                else if (index == 2 & GameManager.aktifball != balls[2])
                {
                    efektler[2].Play();
                    if (MainMenu.IsSoundOn) BlockSound.Play();
                    this.gameObject.GetComponent<Image>().sprite = ShortBlocks[2];

                    Blockss[1].GetComponent<Image>().sprite = LongBlocks[1];
                    Blockss[0].GetComponent<Image>().sprite = LongBlocks[0];
                    Blockss[3].GetComponent<Image>().sprite = LongBlocks[3];
                }
                else if (index == 3 & GameManager.aktifball != balls[3])
                {
                    efektler[3].Play();
                    if (MainMenu.IsSoundOn) BlockSound.Play();
                    this.gameObject.GetComponent<Image>().sprite = ShortBlocks[3];

                    Blockss[1].GetComponent<Image>().sprite = LongBlocks[1];
                    Blockss[2].GetComponent<Image>().sprite = LongBlocks[2];
                    Blockss[0].GetComponent<Image>().sprite = LongBlocks[0];
                }
                
                GameManager.aktifball = balls[index];
            }

            
            collision.gameObject.SetActive(false);
            
        }
    }

    
}
