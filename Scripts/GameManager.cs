using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.EventSystems;


public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject PausePanel;

    [SerializeField]
    private GameObject[] balls = new GameObject[6];

    public LayerMask targetLayer; // Ayarlanacak layer


    public Transform DonmeNoktasi;


    [SerializeField] private Transform ballSpawnpoint;

    [SerializeField]
    private GameObject startball;

    public static GameObject top; // Instantiate edilen topa referans



    static public GameObject aktifball;  // at�lan top

    static public float CollectedPoints;
    [SerializeField] private Text Scoretxt;

    [SerializeField] private Transform[] SpawnPoints = new Transform[16];

    [SerializeField] private GameObject[] Flags = new GameObject[7];

    float initialFallSpeed = 2f; // Ba�lang�� d���� h�z�
    float fallSpeedIncrement = 0.3f; // Her 5 puan i�in d���� h�z� art�� miktar�
    float initialThrowInterval = 3f; // Ba�lang�� at�� aral���
    float minThrowInterval = 1f; // Minimum at�� aral���
    float intervalDecrement = 0.3f; // Her 10 puan i�in azalt�lacak at�� aral��� miktar�

    
    float currentThrowTimer = 0f; // Ge�en s�reyi takip eden de�i�ken

    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;


    public AudioSource backgroundmusic;



    void Start()
    {
        Time.timeScale = 1f;
        //InvokeRepeating("ThrowTheBalls", 0f, initialThrowInterval);
        CollectedPoints = 0;
        aktifball = startball;

        AddEventTrigger(leftButton.gameObject, EventTriggerType.PointerDown, (e) => StartRotatingLeft());
        AddEventTrigger(leftButton.gameObject, EventTriggerType.PointerUp, (e) => StopRotatingLeft());
        AddEventTrigger(rightButton.gameObject, EventTriggerType.PointerDown, (e) => StartRotatingRight());
        AddEventTrigger(rightButton.gameObject, EventTriggerType.PointerUp, (e) => StopRotatingRight());

        allParticleSystems = FindObjectsOfType<ParticleSystem>();


    }

   
    

     public void ThrowTheBalls()
    {
        // Her 5 puan art���nda d���� h�z�n� art�r
        float currentFallSpeed = initialFallSpeed + Mathf.Floor(CollectedPoints / 5) * fallSpeedIncrement;

        // Her biri farkl� spawn noktas�ndan gelen top say�s�
        GameObject fallingBall = Instantiate(balls[Random.Range(0, balls.Length)], SpawnPoints[Random.Range(0, SpawnPoints.Length)].position, Quaternion.identity);
        Rigidbody2D rb = fallingBall.GetComponent<Rigidbody2D>();

        // Topu a�a�� do�ru hareket ettir
        rb.velocity = Vector2.down * currentFallSpeed;



    }


    float GetThrowInterval()
    {
        // Her 10 puan art���nda at�� aral���n� azalt
        return Mathf.Max(minThrowInterval, initialThrowInterval - Mathf.Floor(CollectedPoints / 10) * intervalDecrement);
    }



    void Update()
    {             
        Scoretxt.text = "SCORE: " + CollectedPoints;

        FlagControl();
        BallAlpha();

        // Ge�en s�reyi art�r
        currentThrowTimer += Time.deltaTime;

        // At�� aral���n� kontrol et
        if (currentThrowTimer >= GetThrowInterval())
        {
            // At�� aral���na ula��ld���nda at�� yap
            ThrowTheBalls();

            // At�� yap�ld�ktan sonra ge�en s�reyi s�f�rla
            currentThrowTimer = 0f;
        }

        if (isRotatingLeft)
        {
            RotateWithLimit(5f);
        }

        if (isRotatingRight)
        {
            RotateWithLimit(-5f);
        }

        
    }




    private void FlagControl()
    {
        if (aktifball == balls[0]) //blue
        {
            Flags[0].SetActive(true);
            Flags[1].SetActive(false);
            Flags[2].SetActive(false);
            Flags[3].SetActive(false);
            Flags[4].SetActive(false);
            Flags[5].SetActive(false);           
        }
        else if (aktifball == balls[1]) // green
        {
            Flags[5].SetActive(true);
        }
        else if(aktifball == balls[2]) //orange
        {
            Flags[3].SetActive(true);
            Flags[4].SetActive(false);
            Flags[5].SetActive(false);
        }
        else if (aktifball == balls[3]) //purple
        {
            Flags[1].SetActive(true);
            Flags[2].SetActive(false);
            Flags[3].SetActive(false);
            Flags[4].SetActive(false);
            Flags[5].SetActive(false);
        }
        else if (aktifball == balls[4]) //red
        {
            Flags[2].SetActive(true);
            Flags[3].SetActive(false);
            Flags[4].SetActive(false);
            Flags[5].SetActive(false);
        }
        else if (aktifball == balls[5]) //yellow
        {
            Flags[4].SetActive(true);
            Flags[5].SetActive(false);
        }
        

    }
    private float originalAlpha = 1.0f; 
    private void BallAlpha()
    {
        // Oyun durdu�unda (Time.timeScale == 0 ise) i�lemleri yap
        if (Time.timeScale == 0)
        {
            // Layer'a sahip t�m objeleri bul
            Collider2D[] colliders = Physics2D.OverlapCircleAll(Vector2.zero, Mathf.Infinity, targetLayer);

            // Her bir objenin sprite renderer'�ndaki alpha de�erini s�f�rla
            foreach (Collider2D collider in colliders)
            {
                // Objede SpriteRenderer componenti var m� kontrol et
                SpriteRenderer renderer = collider.GetComponent<SpriteRenderer>();
                if (renderer != null)
                {
                    // Alpha de�erini s�f�rla
                    renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0f);
                }
            }
        }
        else // Oyun devam ederken
        {
            // Layer'a sahip t�m objelerin alpha de�erini geri y�kle
            Collider2D[] colliders = Physics2D.OverlapCircleAll(Vector2.zero, Mathf.Infinity, targetLayer);
            foreach (Collider2D collider in colliders)
            {
                SpriteRenderer renderer = collider.GetComponent<SpriteRenderer>();
                if (renderer != null)
                {
                    // Orijinal alpha de�erini geri y�kle
                    renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, originalAlpha);
                }
            }
        }
    }


    private ParticleSystem[] allParticleSystems;
    public void Pause()
    {
        DestroyParticlesWithTag("ps");
        Time.timeScale = 0f;
        backgroundmusic.Pause();
        PausePanel.SetActive(true);


        

    }

    public void Resume()
    {

        Time.timeScale = 1.0f;
        PausePanel.SetActive(false);
        backgroundmusic.UnPause();

        

    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public Button leftButton;      // Sola d�nd�rme butonu
    public Button rightButton;     // Sa�a d�nd�rme butonu
    public void RotateWithLimit(float rotationIncrement)
    {
        
        Quaternion currentRotation = DonmeNoktasi.localRotation;
        Vector3 currentEulerAngles = currentRotation.eulerAngles;

        
        float newRotationZ = currentEulerAngles.z + rotationIncrement;

        
        if (newRotationZ > 180) newRotationZ -= 360;
        if (newRotationZ < -180) newRotationZ += 360;

        
        newRotationZ = Mathf.Clamp(newRotationZ, -24f, 80f);

       
        currentRotation = Quaternion.Euler(0, 0, newRotationZ);
        DonmeNoktasi.localRotation = currentRotation;
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
    }

    void StopRotatingLeft()
    {
        isRotatingLeft = false;
        Fire();
    }

    void StartRotatingRight()
    {
        isRotatingRight = true;
    }

    void StopRotatingRight()
    {
        isRotatingRight = false;
        Fire();
    }

    void Fire() {

        if (aktifball != null)
        {
            top = Instantiate(aktifball, ballSpawnpoint.position, ballSpawnpoint.rotation);



            // Topun hareket edece�i y�n� belirliyoruz (ballSpawnpoint'in yerel X y�n�)
            Vector3 hareketYon = ballSpawnpoint.right * 15f;
            Vector3 hedefPozisyon = top.transform.position + hareketYon;

            if (!Object.ReferenceEquals(top, null))
            {
                top.transform.DOMove(hedefPozisyon, 2f).SetRelative(false);
            }
        }

    }

    public void playbubble()
    {
        AudioSource playbubble = GetComponent<AudioSource>();
        playbubble.PlayOneShot(playbubble.clip);
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
