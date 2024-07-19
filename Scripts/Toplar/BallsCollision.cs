using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallsCollision : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] Explosions = new ParticleSystem[6];

    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        GameManager gameManager = FindObjectOfType<GameManager>();
        if (this.gameObject.tag == "BlueBall" && collision.CompareTag("BlueBall"))
        {
            Destroy(this.gameObject);
            Destroy(collision.gameObject);
            GameManager.CollectedPoints += 0.5f;
            gameManager.playbubble();
            ParticleSystem ex =   Instantiate(Explosions[0], transform.position, Quaternion.identity);           
            ex.Play();
            Destroy(ex.gameObject, 3f);
        }

        else if (this.gameObject.tag == "YellowBall" && collision.CompareTag("YellowBall"))
        {
            Destroy(this.gameObject);
            Destroy(collision.gameObject);
            GameManager.CollectedPoints += 0.5f;
            gameManager.playbubble();
            ParticleSystem ex = Instantiate(Explosions[1], transform.position, Quaternion.identity);
            ex.Play();
            Destroy(ex.gameObject, 3f);
        }

        else if (this.gameObject.tag == "RedBall" && collision.CompareTag("RedBall"))
        {
            Destroy(this.gameObject);
            Destroy(collision.gameObject);
            GameManager.CollectedPoints += 0.5f;
            gameManager.playbubble();
            ParticleSystem ex = Instantiate(Explosions[2], transform.position, Quaternion.identity);           
            ex.Play();
            Destroy(ex.gameObject, 3f);
        }

        else if (this.gameObject.tag == "OrangeBall" && collision.CompareTag("OrangeBall"))
        {
            Destroy(this.gameObject);
            Destroy(collision.gameObject);
            GameManager.CollectedPoints += 0.5f;
            gameManager.playbubble();
            ParticleSystem ex = Instantiate(Explosions[3], transform.position, Quaternion.identity);
            ex.Play();
            Destroy(ex.gameObject, 3f);
        }

        else if (this.gameObject.tag == "PurpleBall" && collision.CompareTag("PurpleBall"))
        {
            Destroy(this.gameObject);
            Destroy(collision.gameObject);
            GameManager.CollectedPoints +=0.5f;
            gameManager.playbubble();
            ParticleSystem ex = Instantiate(Explosions[4], transform.position, Quaternion.identity);
            ex.Play();
            Destroy(ex.gameObject, 3f);
        }

        else if (this.gameObject.tag == "GreenBall" && collision.CompareTag("GreenBall"))
        {
            Destroy(this.gameObject);
            Destroy(collision.gameObject);
            GameManager.CollectedPoints +=0.5f;
            gameManager.playbubble();
            ParticleSystem ex = Instantiate(Explosions[5], transform.position, Quaternion.identity);
            ex.Play();
            Destroy(ex.gameObject, 3f);
        }

        else if (collision.CompareTag("Finish"))
        {
            Destroy(this.gameObject);
        }
    }

   
}
