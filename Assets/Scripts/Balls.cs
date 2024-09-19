using DG.Tweening;
using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Balls : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] Explosions = new ParticleSystem[4];
    public static bool isShieldOn= false;
    public static bool isHealthTaken = false;
    public static bool isBombTaken = false;
    public static bool isClockTaken = false;
    public static bool HealthSound = false;
    void Start()
    {
        
    } 

                                                        //finish ve sesler 
    void Update()
    {
        
    }

    public static bool pop = false;
    public static bool clocksound = false;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (IsObjectInCameraView(collision.gameObject))
        {
            if (gameObject.tag == "BlueBall" && collision.gameObject.tag == "BlueBall")
            {
                TrailRenderer trail = GetComponent<TrailRenderer>();
                TrailRenderer trail1 = collision.GetComponent<TrailRenderer>();

                if ((trail.isVisible && !trail1.isVisible) || (!trail.isVisible && trail1.isVisible))
                {
                    if (MainMenu.IsSoundOn)
                        pop = true;
                    this.gameObject.SetActive(false);
                    if (this.gameObject != null) Destroy(this.gameObject, 2f);
                    collision.gameObject.SetActive(false);
                    if (collision.gameObject != null) Destroy(collision.gameObject, 2f);
                    GameManager.CollectedPoints += 1;
                    //music
                    ParticleSystem ex = Instantiate(Explosions[0], collision.transform.position, Quaternion.identity);
                    ex.Play();
                }

            }

            else if (gameObject.tag == "YellowBall" && collision.gameObject.tag == "YellowBall")
            {
                TrailRenderer trail = GetComponent<TrailRenderer>();
                TrailRenderer trail1 = collision.GetComponent<TrailRenderer>();

                if ((trail.isVisible && !trail1.isVisible) || (!trail.isVisible && trail1.isVisible))
                {
                    if (MainMenu.IsSoundOn)
                        pop = true;
                    this.gameObject.SetActive(false);
                    if (this.gameObject != null) Destroy(this.gameObject, 2f);
                    collision.gameObject.SetActive(false);
                    if (collision.gameObject != null) Destroy(collision.gameObject, 2f);
                    GameManager.CollectedPoints += 1;
                    //music
                    ParticleSystem ex = Instantiate(Explosions[1], collision.transform.position, Quaternion.identity);
                    ex.Play();
                }

            }

            else if (gameObject.tag == "RedBall" && collision.gameObject.tag == "RedBall")
            {
                TrailRenderer trail = GetComponent<TrailRenderer>();
                TrailRenderer trail1 = collision.GetComponent<TrailRenderer>();

                if ((trail.isVisible && !trail1.isVisible) || (!trail.isVisible && trail1.isVisible))
                {
                    if (MainMenu.IsSoundOn)
                        pop = true;
                    this.gameObject.SetActive(false);
                    if (this.gameObject != null) Destroy(this.gameObject, 2f);
                    collision.gameObject.SetActive(false);
                    if (collision.gameObject != null) Destroy(collision.gameObject, 2f);
                    GameManager.CollectedPoints += 1;
                    //music
                    ParticleSystem ex = Instantiate(Explosions[2], collision.transform.position, Quaternion.identity);
                    ex.Play();
                }

            }


            else if (gameObject.tag == "GreenBall" && collision.gameObject.tag == "GreenBall")
            {
                TrailRenderer trail = GetComponent<TrailRenderer>();
                TrailRenderer trail1 = collision.GetComponent<TrailRenderer>();

                if ((trail.isVisible && !trail1.isVisible) || (!trail.isVisible && trail1.isVisible) )
                {
                    if (MainMenu.IsSoundOn)
                        pop = true;
                    this.gameObject.SetActive(false);
                    if (this.gameObject != null) Destroy(this.gameObject, 2f);
                    collision.gameObject.SetActive(false);
                    if (collision.gameObject != null) Destroy(collision.gameObject, 2f);
                    GameManager.CollectedPoints += 1;
                    //music
                    ParticleSystem ex = Instantiate(Explosions[3], collision.transform.position, Quaternion.identity);
                    ex.Play();
                }

            }

            else if (collision.gameObject.tag == "Shield" )
            {
                TrailRenderer trail = GetComponent<TrailRenderer>();

                if (!trail.isVisible)
                {
                    this.gameObject.SetActive(false);
                    if (this.gameObject != null) Destroy(this.gameObject, 2f);

                    if (gameObject.tag == "BlueBall")
                    {
                        if (MainMenu.IsSoundOn)
                            pop = true;
                        ParticleSystem ex = Instantiate(Explosions[0], gameObject.transform.position, Quaternion.identity);
                        ex.Play();
                    }

                    if (gameObject.tag == "YellowBall")
                    {
                        if (MainMenu.IsSoundOn)
                            pop = true;
                        ParticleSystem ex = Instantiate(Explosions[1], gameObject.transform.position, Quaternion.identity);
                        ex.Play();
                    }

                    if (gameObject.tag == "GreenBall")
                    {
                        if (MainMenu.IsSoundOn)
                            pop = true;
                        ParticleSystem ex = Instantiate(Explosions[3], gameObject.transform.position, Quaternion.identity);
                        ex.Play();
                    }

                    if (gameObject.tag == "RedBall")
                    {
                        if (MainMenu.IsSoundOn)
                            pop = true;
                        ParticleSystem ex = Instantiate(Explosions[2], gameObject.transform.position, Quaternion.identity);
                        ex.Play();
                    }
                }
            }

            else if (collision.gameObject.tag == "ShieldPackage")
            {
                TrailRenderer trail = GetComponent<TrailRenderer>();

                if (trail.isVisible)
                {
                    
                    collision.gameObject.SetActive(false);
                    Destroy(collision.gameObject, 2f);
                    if (MainMenu.IsSoundOn)
                        isShieldOn = true;
                }

            }

            else if (collision.gameObject.tag == "HealthPackage")
            {
                TrailRenderer trail = GetComponent<TrailRenderer>();

                if (trail.isVisible)
                {
                    collision.gameObject.SetActive(false);
                    Destroy(collision.gameObject, 2f);
                    isHealthTaken = true;
                    if(MainMenu.IsSoundOn)
                    HealthSound = true;
                }
            }

            else if (collision.gameObject.tag == "Bomb")
            {
                TrailRenderer trail = GetComponent<TrailRenderer>();

                if (trail.isVisible)
                {
                    collision.gameObject.SetActive(false);
                    GameObject ex = Instantiate(Explosion, collision.transform.position, Quaternion.identity);
                    Destroy(ex,2f);
                    Destroy(collision.gameObject, 2f);
                    if (MainMenu.IsSoundOn)
                        isBombTaken = true;
                }
            }

            else if (collision.gameObject.tag == "Clock")
            {
                TrailRenderer trail = GetComponent<TrailRenderer>();

                if (trail.isVisible)
                {
                    isClockTaken = true;
                    collision.gameObject.SetActive(false);
                    Destroy(collision.gameObject, 2f);
                    if (MainMenu.IsSoundOn)
                        clocksound = true;
                }
            }

        }
    }

    private bool IsObjectInCameraView(GameObject obj)
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(obj.transform.position);
        return viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1;
    }

    [SerializeField] GameObject Explosion;
}
