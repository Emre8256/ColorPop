using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class HitLine : MonoBehaviour
{
    [SerializeField] private ParticleSystem hitps;
    [SerializeField] private Slider Health;
    [SerializeField] private Text SCORE;
    [SerializeField] private GameObject DamagePanel;   
    public static bool gameOverTriggered ;

    [SerializeField] AudioSource Hit;

    void Start()
    {
       
    }

    
    void Update()
    {
        if (!gameOverTriggered)
        {
            gameOverControl();
        }

        if (Balls.isHealthTaken)
        {
            Health.value += 20;
            Balls.isHealthTaken = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BlueBall" || collision.gameObject.tag == "GreenBall" || collision.gameObject.tag == "YellowBall" || collision.gameObject.tag == "RedBall")
        {
            Health.value -= 10;
            if(MainMenu.IsSoundOn) Hit.Play();
            if(MainMenu.IsVibrateOn) Handheld.Vibrate();  
            Vector3 spawnPosition = collision.transform.position;
            spawnPosition.y -= 0.2f;
            ParticleSystem hitps2 = Instantiate(hitps, spawnPosition, Quaternion.identity);
            Vector3 newPosition = hitps2.transform.position;
            newPosition.z = 1;
            hitps2.transform.position = newPosition;
            hitps2.Play();
            Destroy(collision.gameObject);
            StartCoroutine(OpenAndCloseDamagePanel());
        }

        else if (collision.gameObject.tag == "Clock" || collision.gameObject.tag == "Bomb" || collision.gameObject.tag == "HealthPackage" || collision.gameObject.tag == "ShieldPackage")
        {
            collision.gameObject.SetActive(false);
            Destroy(collision.gameObject,2f);
        }
    }

    IEnumerator OpenAndCloseDamagePanel()
    {
        DamagePanel.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        DamagePanel.SetActive(false);
    }

    [SerializeField] GameObject GameOverPnl;
    [SerializeField] GameObject AnimateText;
    [SerializeField] Text EndScoreTextFade;
    [SerializeField] Text EndScoreText;
    [SerializeField] ParticleSystem CoinParticle;
    [SerializeField] private Button EndPlayAgain;
    [SerializeField] private Button EndMainMenu;

    public void GameOverPanel()
    {
        DestroyObjects();
        gameOverTriggered = true;
        int point = (int)GameManager.CollectedPoints;
        var main = CoinParticle.main;
        main.maxParticles = point;
        CanvasGroup canvasGroup1 = EndPlayAgain.GetComponent<CanvasGroup>();
        CanvasGroup canvasGroup2 = EndMainMenu.GetComponent<CanvasGroup>();

        GameOverPnl.SetActive(true);
        DOVirtual.DelayedCall(1.50f, () => AnimateText.SetActive(true));
        DOVirtual.DelayedCall(2.5f, () => EndScoreTextFade.DOFade(1f, 1.5f));

        if(point == 0)
        {
            DOVirtual.DelayedCall(4f, () => DOTween.To(() => 0, x => EndScoreText.text = x.ToString(), point, 0.5f));
            DOVirtual.DelayedCall(5f, () =>
            {
                if (canvasGroup1 != null)
                {
                    canvasGroup1.DOFade(1f, 1.5f);
                    EndPlayAgain.interactable = true;
                }

                if (canvasGroup2 != null)
                {
                    canvasGroup2.DOFade(1f, 1.5f);
                    EndMainMenu.interactable = true;
                }
            });
        }

        if(point > 0 && point < 11) { 
        DOVirtual.DelayedCall(4f, () => DOTween.To(() => 0, x => EndScoreText.text = x.ToString(), point, 1f));
            DOVirtual.DelayedCall(6f, () =>
            {
                DOTween.To(() => point, x => EndScoreText.text = x.ToString(), 0, 1f);
                CoinParticle.Play();
            });

            DOVirtual.DelayedCall(7f, () => CoinParticle.Stop(false, ParticleSystemStopBehavior.StopEmitting));
            DOVirtual.DelayedCall(8f, () =>
            {
                if (canvasGroup1 != null)
                {
                    canvasGroup1.DOFade(1f, 1.5f);
                    EndPlayAgain.interactable = true;
                }

                if (canvasGroup2 != null)
                {
                    canvasGroup2.DOFade(1f, 1.5f);
                    EndMainMenu.interactable = true;
                }
            });

        }

        if (point > 10)
        {
            DOVirtual.DelayedCall(4f, () => DOTween.To(() => 0, x => EndScoreText.text = x.ToString(), point, 2f));
            DOVirtual.DelayedCall(7f, () =>
            {
                DOTween.To(() => point, x => EndScoreText.text = x.ToString(), 0, 2f);
                CoinParticle.Play();
            });

            DOVirtual.DelayedCall(9f, () => CoinParticle.Stop(false, ParticleSystemStopBehavior.StopEmitting));
            DOVirtual.DelayedCall(10f, () =>
            {
                if (canvasGroup1 != null)
                {
                    canvasGroup1.DOFade(1f, 1.5f);
                    EndPlayAgain.interactable = true;
                }

                if (canvasGroup2 != null)
                {
                    canvasGroup2.DOFade(1f, 1.5f);
                    EndMainMenu.interactable = true;
                }
            });
        }

        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins",0) + point);

        if (point > PlayerPrefs.GetInt("Score",0))
        PlayerPrefs.SetInt("Score", point);
        
    }

    private void gameOverControl()
    {
        if (Health.value <= 0)
        {

           
            
            //backgroundmusic.Stop();
            
            //DestroyParticlesWithTag("ps");
            
            GameOverPanel();
            //updatehightscore();
            //gameoversound.PlayOneShot(gameoversound.clip);

        }
    }

    void DestroyObjects()
    {
        
        string[] tags = { "ps", "Finish", "RedBall", "BlueBall", "YellowBall", "GreenBall","Ball" , "BombAnim" , "Shield" , "Clock" , "Bomb" , "HealthPackage", "ShieldPackage"};

        
        foreach (string tag in tags)
        {
            
            GameObject[] objectsToDestroy = GameObject.FindGameObjectsWithTag(tag);

            
            foreach (GameObject obj in objectsToDestroy)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                    Destroy(obj, 2f);
                }
                
            }
        }



    }

    public void RestartGame()
    {
        DOTween.KillAll();
        Health.value = 100;
        HitLine.gameOverTriggered = false;
        SceneManager.LoadScene("Game");
    }

}
