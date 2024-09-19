using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine.EventSystems;
using System.Data;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject PausePanel;
    [SerializeField] private Transform ballSpawnpoint;
    [SerializeField] private GameObject startball;
    [SerializeField] private GameObject[] balls = new GameObject[4];
    [SerializeField] private Transform[] SpawnPoints = new Transform[31];
    [SerializeField] private GameObject BallHolder;
    [SerializeField] private Animator CannonShot;
    [SerializeField] private ParticleSystem ShotParticle;
    [SerializeField] private ParticleSystem ShotParticlePrefab;

    [SerializeField] private Sprite[] CannonColors = new Sprite[12];

    [SerializeField] private Transform cannon;

    static public GameObject aktifball;  
    static public int CollectedPoints;
    [SerializeField] private Text Scoretxt;

    private float CurrentFallSpeed;
    private float CurrentThrowInterval;

    private float lastThrowInterval;

    public Button leftButton;      // Sola döndürme butonu
    public Button rightButton;     // Saða döndürme butonu
    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;

    [SerializeField] GameObject l, r;

    [SerializeField] AudioSource pauseSounds;
    [SerializeField] AudioSource Pop;
    [SerializeField] AudioSource ClockSound;
    [SerializeField] AudioSource ShieldSound;
    [SerializeField] AudioSource BombSound;
    [SerializeField] AudioSource HealthSoundd;

    void Start()
    {

        if (PlayerPrefs.GetInt("Tutorial", 0) == 0)
        {
            l.SetActive(true);
            r.SetActive(true);
        }
        // Cihazın ekran yenileme hızını al
        double refreshRate = Screen.currentResolution.refreshRateRatio.value;

        // FPS ayarını ekran yenileme hızına göre ayarla
        Application.targetFrameRate = Mathf.RoundToInt((float)refreshRate);


        int SelectedColor = PlayerPrefs.GetInt("ChoosedOne", 0);
        cannon.gameObject.GetComponent<Image>().sprite = CannonColors[SelectedColor];




        Time.timeScale = 1f;

        CurrentThrowInterval = 5;
        lastThrowInterval = CurrentThrowInterval; // İlk atış aralığını kaydediyoruz
        CurrentFallSpeed = 2f;

        if (!HitLine.gameOverTriggered ) { InvokeRepeating("ThrowTheBalls", 0, CurrentThrowInterval); }
        

        CollectedPoints = 0;
        aktifball = startball;


        AddEventTrigger(leftButton.gameObject, EventTriggerType.PointerDown, (e) => StartRotatingLeft());
        AddEventTrigger(leftButton.gameObject, EventTriggerType.PointerUp, (e) => StopRotatingLeft());
        AddEventTrigger(rightButton.gameObject, EventTriggerType.PointerDown, (e) => StartRotatingRight());
        AddEventTrigger(rightButton.gameObject, EventTriggerType.PointerUp, (e) => StopRotatingRight());

    }

    
    void Update()
    {
        Scoretxt.text = "SCORE: " + CollectedPoints;

        CannonRotation();
        DifficultyControl();
        

        if (HitLine.gameOverTriggered)
        {
            CancelInvoke("ThrowTheBalls");
        }

        // Atış aralığı değiştiyse, eski aralığı iptal edip yenisini başlatıyoruz
        if (CurrentThrowInterval != lastThrowInterval)
        {
            CancelInvoke("ThrowTheBalls");
            InvokeRepeating("ThrowTheBalls", 0, CurrentThrowInterval);
            lastThrowInterval = CurrentThrowInterval; // Yeni aralığı kaydediyoruz
        }

        


        if (Balls.isShieldOn)
        {
            
                StartCoroutine(SendShieldPackage());
                Balls.isShieldOn = false;
            
           


        }

        if (Balls.isBombTaken)
        {
            BombTaken();
            Balls.isBombTaken = false;
        }

         if(Balls.isClockTaken)
        {
           
                
                StartCoroutine(ClockTaken());
                Balls.isClockTaken = false;

            
            
        }

        if (Balls.pop)
        {
            
                Pop.Play();
                Balls.pop = false;
            
        }

        if (Balls.clocksound)
        {
            
                ClockSound.Play();
                Balls.clocksound = false;
            
        }

        if(Balls.HealthSound)
        {
            HealthSoundd.Play();
            Balls.HealthSound = false;
        }

    }


    void AddEventTrigger(GameObject obj, EventTriggerType eventType, System.Action<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        if (trigger == null) trigger = obj.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventType;
        entry.callback.AddListener(new UnityEngine.Events.UnityAction<BaseEventData>(action));
        trigger.triggers.Add(entry);
    }

    void StartRotatingLeft()
    {
        isRotatingLeft = true;

        if (PlayerPrefs.GetInt("Tutorial", 0) == 0)
        {
            l.SetActive(false);
            r.SetActive(false);
            PlayerPrefs.SetInt("Tutorial", 1);
        }
    }

    void StopRotatingLeft()
    {
        isRotatingLeft = false;
        Shoot();
        CannonShot.Play("Shot", 0, 0f); // Bu şekilde animasyon sıfırdan başlatılabilir
    }

    void StartRotatingRight()
    {
        isRotatingRight = true;
        if (PlayerPrefs.GetInt("Tutorial", 0) == 0)
        {
            l.SetActive(false);
            r.SetActive(false);
            PlayerPrefs.SetInt("Tutorial", 1);
        }
    }

    void StopRotatingRight()
    {
        isRotatingRight = false;
        Shoot();
        CannonShot.Play("Shot", 0, 0f); // Bu şekilde animasyon sıfırdan başlatılabilir
    }





    public void OpenPausePanel()
    {

        if (MainMenu.IsSoundOn) pauseSounds.Play();
        CloseTrail();

        DeactiveParticlesWithTag("ps");
        PausePanel.SetActive(true);

        Time.timeScale = 0f;

                   



    }

    GameObject[] Ballss;
    void CloseTrail()
    {
        string[] tags = { "RedBall", "BlueBall", "YellowBall", "GreenBall", "Ball" };

        // Topları listele ve diziyi oluştur
        List<GameObject> ballsList = new List<GameObject>();

        foreach (string tag in tags)
        {
            // Belirtilen tag ile olan tüm objeleri bul
            GameObject[] balls = GameObject.FindGameObjectsWithTag(tag);

            foreach (GameObject b in balls)
            {
                // Objede TrailRenderer bileşeni olup olmadığını kontrol et
                TrailRenderer tr = b.GetComponent<TrailRenderer>();
                if (tr != null && tr.enabled) // TrailRenderer varsa ve aktifse
                {
                    // Objeyi listeye ekle
                    ballsList.Add(b);

                    // TrailRenderer'ı kapat
                    tr.enabled = false;
                }
            }
        }

        // Listeyi diziye dönüştür
        Ballss = ballsList.ToArray();
    }

    void OpenTrail()
    {
        if (Ballss == null)
            return;

        foreach (GameObject b in Ballss)
        {
            TrailRenderer tr = b.GetComponent<TrailRenderer>();
            if (tr != null)
            {
                tr.enabled = true;
            }
        }
    }

    public void Resume()
    {
        OpenTrail();
        if (MainMenu.IsSoundOn) pauseSounds.Play();
        PausePanel.SetActive(false);

        if(!isTimeSlow)
        Time.timeScale = 1f; // Zamanı geri yükle

        if (isTimeSlow)
            Time.timeScale = 0.6f;



    }

    public void RestartGame()
    {
        if (MainMenu.IsSoundOn) pauseSounds.Play();
        DOTween.KillAll();
        HitLine.gameOverTriggered = false;
        StartCoroutine(loadGame());
    }

    IEnumerator loadGame()
    {
        yield return new WaitForSecondsRealtime(0.05f);
        SceneManager.LoadScene("Game");
    }

    IEnumerator loadMenu()
    {
        yield return new WaitForSecondsRealtime(0.05f);
        SceneManager.LoadScene("MainMenu");
    }

    public void GoMainMenu()
    {
        if (MainMenu.IsSoundOn) pauseSounds.Play();
        DOTween.KillAll();
        StartCoroutine(loadMenu()); 

    }

    void CannonRotation()
    {
        if (!HitLine.gameOverTriggered)
        {
            if (Time.timeScale != 0f)
            {

                float rotationZ = cannon.transform.rotation.eulerAngles.z;

                // Z rotasyonunu -40 ile 40 derece arasında tut
                if (rotationZ > 180) rotationZ -= 360;


                // A tuşuna basıldığında rotasyonu artır
                if ( isRotatingLeft)
                {
                    rotationZ += 100f * Time.deltaTime;
                }

               

                // D tuşuna basıldığında rotasyonu azalt
                if ( isRotatingRight)
                {
                    rotationZ -= 100f * Time.deltaTime;
                }


                // Z rotasyonunu sınırla
                rotationZ = Mathf.Clamp(rotationZ, -60f, 60f);

                // Yeni rotasyonu uygula
                cannon.transform.rotation = Quaternion.Euler(0, 0, rotationZ);
            }
        }
        
    }

    [SerializeField] AudioSource Fire;
    void Shoot()
    {
        GameObject ball = Instantiate(aktifball, ballSpawnpoint.position, ballSpawnpoint.rotation);
        ball.transform.SetParent(BallHolder.transform,false);

        ball.transform.position = ballSpawnpoint.position;

        RectTransform rt = ball.GetComponent<RectTransform>();

        if(MainMenu.IsSoundOn) Fire.Play();

        ParticleSystem ps = Instantiate(ShotParticlePrefab, ShotParticle.transform.position, ShotParticle.transform.rotation);
        ps.transform.SetParent(BallHolder.transform, false);
        ps.transform.position = ShotParticle.transform.position;
        ps.transform.rotation = ShotParticle.transform.rotation;
        ps.Play();

        Vector3 hareketyon = ballSpawnpoint.up * 15f;
        Vector3 hedefPozisyon = ball.transform.position + hareketyon;

        if (ball != null)
        {
            if(rt!=null)
                ball.transform.DOMove(hedefPozisyon, 2f).SetRelative(false) ;
            
        }

        // Destroy the ball after 3 seconds
        if (ball != null)
        {

            Destroy(ball, 3f);
        }


    }

    void DeactiveParticlesWithTag(string tag)
    {
        GameObject[] particles = GameObject.FindGameObjectsWithTag(tag);
        GameObject BombAnim = GameObject.FindGameObjectWithTag("BombAnim");
        if(BombAnim != null) Destroy(BombAnim);
        foreach (GameObject particle in particles)
        {

            ParticleSystem ps = particle.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }
    }

    
    

    void ThrowTheBalls()
    {
        GameObject FallingBall = Instantiate(balls[Random.Range(0, balls.Length)], SpawnPoints[Random.Range(0, SpawnPoints.Length)].position, Quaternion.identity, BallHolder.transform);
        Rigidbody2D rb = FallingBall.GetComponent<Rigidbody2D>();

        TrailRenderer trailRenderer = FallingBall.GetComponent<TrailRenderer>();

        if (trailRenderer != null)
        {
            trailRenderer.enabled = false;
        }

        rb.velocity = Vector2.down * CurrentFallSpeed;
    }



    private bool SendAnyPackage = false;
    void DifficultyControl()
    {
        if (CollectedPoints >= 5 && CollectedPoints <15 )
        {
            CurrentThrowInterval = 3f;                                                                        //ilk    !sendany  ve sendany = true
            
        }
                                                                                                              ///////***************//////////
        

        if (CollectedPoints >= 15 && CollectedPoints < 20)
        {

            CurrentThrowInterval = 2f;
        }

        if (CollectedPoints >= 20 &&  CollectedPoints < 30 &&!SendAnyPackage)
        {

            StartCoroutine(SendPackages(0,2));
            
            SendAnyPackage = true; 
        }

        if (CollectedPoints >= 30 && CollectedPoints < 40 )
        {

            CurrentFallSpeed = 3f;
        }

        if (CollectedPoints >= 40 && CollectedPoints < 50 && SendAnyPackage)
        {

            CurrentFallSpeed = 3f;
            StartCoroutine(SendPackages(1,3));

            SendAnyPackage = false;
        }

        if (CollectedPoints >= 50 && CollectedPoints < 60 && !SendAnyPackage)
        {

            
            CurrentThrowInterval = 1.75f;
            StartCoroutine(SendPackages(0,2));

            SendAnyPackage = true;
        }

        if (CollectedPoints >= 60 && CollectedPoints < 70 && SendAnyPackage)
        {

            CurrentFallSpeed = 3.25f;
            StartCoroutine(SendPackages(1,3));

            SendAnyPackage = false;
        }

        if (CollectedPoints >= 70 && CollectedPoints < 80 && !SendAnyPackage)
        {

            CurrentThrowInterval = 1.5f;
            StartCoroutine(SendPackages(0, 2));

            SendAnyPackage = true;
        }

        if (CollectedPoints >= 80 && CollectedPoints < 90 && SendAnyPackage)
        {

            CurrentFallSpeed = 3.5f;
            StartCoroutine(SendPackages(1,3));

            SendAnyPackage = false;
        }

        if (CollectedPoints >= 90 && CollectedPoints < 100 && !SendAnyPackage)
        {

            CurrentThrowInterval = 1.25f;
            StartCoroutine(SendPackages(0, 2));

            SendAnyPackage = true;
        }

        if (CollectedPoints >= 100 && CollectedPoints < 110 && SendAnyPackage)
        {

            CurrentFallSpeed = 3.75f;
            StartCoroutine(SendPackages(1,3));

            SendAnyPackage = false;
        }

        if (CollectedPoints >= 110 && CollectedPoints < 120 && !SendAnyPackage)
        {

            CurrentFallSpeed = 4;
            StartCoroutine(SendPackages(0, 2));

            SendAnyPackage = true;
        }

        if (CollectedPoints >= 120 && CollectedPoints < 130 && SendAnyPackage)
        {

            CurrentThrowInterval = 1f;
            StartCoroutine(SendPackages(1,3));

            SendAnyPackage = false;
        }

        if (CollectedPoints >= 130 && CollectedPoints < 145 && !SendAnyPackage)
        {

            
            StartCoroutine(SendPackages(0,2));

            SendAnyPackage = true;
        }

        if (CollectedPoints >= 145 && CollectedPoints < 160 && SendAnyPackage)
        {


            StartCoroutine(SendPackages(1, 3));

            SendAnyPackage = false;
        }

        if (CollectedPoints >= 160  && !SendAnyPackage)
        {


            StartCoroutine(SendPackages(0,2));

            SendAnyPackage = true;
        }



    }

    [SerializeField] GameObject Shield;
    

    IEnumerator SendShieldPackage()
    {
        if (!Shield.activeSelf)
        {
            // Alpha değerini 200/255 olarak ayarla
            Image shieldImage = Shield.GetComponent<Image>();
            Color color = shieldImage.color;
            color.a = 200f / 255f;  // 200/255 aralığına çekiyoruz
            shieldImage.color = color;
             if(MainMenu.IsSoundOn)    ShieldSound.Play();
            Shield.SetActive(true);
            yield return new WaitForSeconds(10f);
            AnimatorStateInfo stateInfo = Shield.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
            float animationLength = stateInfo.length + 2f;
            Shield.GetComponent<Animator>().Play("Shield", 0,0);
            yield return new WaitForSeconds(animationLength);


            Shield.SetActive(false);
            Balls.isShieldOn = false;
        }

    }

    [SerializeField] private ParticleSystem[] Explosions = new ParticleSystem[4];
    void BombTaken()
    {
        StartCoroutine(OpenAndCloseBombPanel());
        if(MainMenu.IsSoundOn) BombSound.Play();
        string[] tags = { "RedBall", "BlueBall", "YellowBall", "GreenBall" };

        foreach (string tag in tags)
        {
            GameObject[] objectsToDestroy = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject obj in objectsToDestroy)
            {
                // TrailRenderer bileşeni var mı kontrol et
                TrailRenderer trail = obj.GetComponent<TrailRenderer>();
                if (trail != null && !trail.isVisible)
                {
                    // Objeyi devre dışı bırak ve 2 saniye sonra yok et
                    obj.SetActive(false);
                    Destroy(obj, 2f);
                    CollectedPoints += 1;

                    // Eğer RedBall ise patlama efektini oynat
                    if (obj.tag == "RedBall")
                    {
                        ParticleSystem ex = Instantiate(Explosions[2], obj.transform.position, Quaternion.identity);
                        ex.Play();
                    }

                    else if (obj.tag == "BlueBall")
                    {
                        ParticleSystem ex = Instantiate(Explosions[0], obj.transform.position, Quaternion.identity);
                        ex.Play();
                    }

                    else if (obj.tag == "YellowBall")
                    {
                        ParticleSystem ex = Instantiate(Explosions[1], obj.transform.position, Quaternion.identity);
                        ex.Play();
                    }

                    else if (obj.tag == "GreenBall")
                    {
                        ParticleSystem ex = Instantiate(Explosions[3], obj.transform.position, Quaternion.identity);
                        ex.Play();  
                    }
                }
            }
        }
    }

    [SerializeField] GameObject BombPanel;
    IEnumerator OpenAndCloseBombPanel()
    {
        BombPanel.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        BombPanel.SetActive(false);
    }

    private bool isTimeSlow;
    
    IEnumerator ClockTaken()
    {
        
        Time.timeScale = 0.6f; // Zamanı yavaşlat
        isTimeSlow = true;

        yield return new WaitForSeconds(6f);

        // Zaman dolduğunda zamanı normale döndür
        Time.timeScale = 1f;
        
        
        isTimeSlow = false;
        


    }
    

    [SerializeField] GameObject[] Packages = new GameObject[4];
    IEnumerator SendPackages(int a,int b)
    {
        yield return new WaitForSeconds(1f);
        GameObject FallingPackage = Instantiate(Packages[Random.Range(a, b)]  , SpawnPoints[Random.Range(0, SpawnPoints.Length)].position, Quaternion.identity, BallHolder.transform);
        Rigidbody2D rb = FallingPackage.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.down * 2f;
    }



    
}
