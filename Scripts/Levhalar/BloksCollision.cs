using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BloksCollision : MonoBehaviour
{
    [SerializeField]
    private GameObject[] balls = new GameObject[6];

    [SerializeField] 
     public ParticleSystem[] efektler = new ParticleSystem[6];

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BlueBall") || collision.CompareTag("GreenBall") || collision.CompareTag("YellowBall") || collision.CompareTag("RedBall") || collision.CompareTag("PurpleBall") || collision.CompareTag("OrangeBall") || collision.CompareTag("Ball"))
        {           
            if (int.TryParse(this.gameObject.tag, out int index))
            {
                if (index == 0) {efektler[0].Play(); }
                else if (index == 1) { efektler[3].Play();}
                else if (index == 2) { efektler[4].Play();}
                else if (index == 3) { efektler[5].Play();}
                else if (index == 4) { efektler[1].Play();}
                else if (index == 5) { efektler[2].Play();}
               GameManager.aktifball = balls[index];
            }
           
            Destroy(collision.gameObject);
            
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
