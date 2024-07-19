using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SocialPlatforms.Impl;

public class HitLine : MonoBehaviour
{
    [SerializeField] private ParticleSystem hitps;
    [SerializeField] private Slider Health;
    [SerializeField] private Text SCORE;


    public float duration = 0.5f;  // Sarsýntý süresi
    public float strength = 1.0f;  // Sarsýntý þiddeti
    public int vibrato = 10;       // Titreþim sayýsý
    public float randomness = 90f; // Rastgelelik oraný
    private Vector3 initialPosition;

    public AudioSource hitline;
    public AudioSource gameoversound;
    public AudioClip gameOverSoundClip;
    public AudioSource backgroundmusic;
    private bool gameOverTriggered;





    public GameObject gameoverPanel;
    void Start()
    {
       gameOverTriggered = false;
    }

   

    // Update is called once per frame
    void Update()
    {
        if (!gameOverTriggered)
        {
            gameOverControl();
        }



    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("BlueBall") || collision.CompareTag("GreenBall") || collision.CompareTag("YellowBall") || collision.CompareTag("RedBall") || collision.CompareTag("PurpleBall") || collision.CompareTag("OrangeBall"))
        {
            Health.value -= 10;
            hitline.PlayOneShot(hitline.clip);
            Handheld.Vibrate();
            Vector3 spawnPosition = collision.transform.position;
            spawnPosition.y -= 0.5f; 
            ParticleSystem hitps2 = Instantiate(hitps, spawnPosition, Quaternion.identity);
            Vector3 newPosition = hitps2.transform.position;
            newPosition.z = 1;
            hitps2.transform.position = newPosition;
            hitps2.Play();
            Destroy(collision.gameObject);
            Destroy(hitps2.gameObject, 2f);

        }
    }

    private void gameOverControl()
    {
        if(Health.value <= 0)
        {

            gameOverTriggered = true;
            Time.timeScale = 0f;
            backgroundmusic.Stop();
            if (gameoversound != null && gameOverSoundClip != null)
            {
                gameoversound.PlayOneShot(gameOverSoundClip);
            }
            DestroyParticlesWithTag("ps");
            gameoverPanel.SetActive(true);
            SCORE.text = "SCORE : " + GameManager.CollectedPoints;
            updatehightscore();
            gameoversound.PlayOneShot(gameoversound.clip);

        }
    }


   private void updatehightscore()
    {
        if(PlayerPrefs.GetFloat("HighScore") == 0f || PlayerPrefs.GetInt("HighScore") == null)
        {
            PlayerPrefs.SetFloat("HighScore", GameManager.CollectedPoints);
            PlayerPrefs.Save();
        }
        else if (GameManager.CollectedPoints > PlayerPrefs.GetFloat("HighScore"))
        {
            PlayerPrefs.SetFloat("HighScore", GameManager.CollectedPoints);
            PlayerPrefs.Save();
        }
    }

    void DestroyParticlesWithTag(string tag)
    {
        GameObject[] particles = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject particle in particles)
        {
            Destroy(particle);
        }
    }

}
