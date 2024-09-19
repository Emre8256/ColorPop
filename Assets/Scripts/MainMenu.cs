using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject Canclick;

    [SerializeField] private Sprite SoundOnImg;
    [SerializeField] private Sprite SoundOffImg;
    [SerializeField] private Sprite VibrateOnImg;
    [SerializeField] private Sprite VibrateOffImg;

    [SerializeField] private Button SoundBtn;
    [SerializeField] private Button VibrateBtn;

    [SerializeField] private Text HighScoreTxt;


    public static bool IsSoundOn = true;
    public static bool IsVibrateOn = true;

    [SerializeField] AudioSource SoundAndVibrate;
    [SerializeField] AudioSource NormalButton;
    [SerializeField] AudioSource Background;

    void Start()
    {
        if(!IsSoundOn) Background.Pause();

        Time.timeScale = 1f;

        HighScoreTxt.text = "HIGH SCORE :" + "" + PlayerPrefs.GetInt("Score", 0);

        UpdateButtonSprites();
        InvokeRepeating("ThrowTheBalls", 1f, 3f);
    }

    
    void Update()
    {
        if (SplashScreen.isFinished) Destroy(Canclick);
    }


    public void soundButton()
    {
        IsSoundOn = !IsSoundOn;
        SoundBtn.image.sprite = IsSoundOn ? SoundOnImg : SoundOffImg;

        
        if(!IsSoundOn) Background.Pause();
        if (IsSoundOn){SoundAndVibrate.Play();  Background.UnPause(); }
    }

    public void vibrateButton()
    {
        IsVibrateOn = !IsVibrateOn;    // sesleri unutma
        VibrateBtn.image.sprite = IsVibrateOn ? VibrateOnImg : VibrateOffImg;
        if (IsSoundOn) SoundAndVibrate.Play();
    }

    void UpdateButtonSprites()
    {
        // Butonlarýn sprite'ýný `IsSoundOn` ve `IsVibrateOn` deðiþkenlerine göre güncelle
        SoundBtn.image.sprite = IsSoundOn ? SoundOnImg : SoundOffImg;
        VibrateBtn.image.sprite = IsVibrateOn ? VibrateOnImg : VibrateOffImg;
    }

    public void PlayButton()
    {
        if (IsSoundOn) NormalButton.Play();
        HitLine.gameOverTriggered = false;
        Invoke("GameScene", 0.05f);
    }

    public void MarketButton()
    {
        if (IsSoundOn) NormalButton.Play();
        Invoke("MarketScene", 0.05f);
    }

    [SerializeField] GameObject[] balls = new GameObject[4];
    [SerializeField] Transform[] SpawnPoints = new Transform[4];
    [SerializeField] GameObject BallHolder;
    void ThrowTheBalls()
    {
        // Cihazýn ekran yenileme hýzýný al
        double refreshRate = Screen.currentResolution.refreshRateRatio.value;

        // FPS ayarýný ekran yenileme hýzýna göre ayarla
        Application.targetFrameRate = Mathf.RoundToInt((float)refreshRate);
        GameObject FallingBall = Instantiate(balls[Random.Range(0, balls.Length)], SpawnPoints[Random.Range(0, SpawnPoints.Length)].position, Quaternion.identity, BallHolder.transform);
        Rigidbody2D rb = FallingBall.GetComponent<Rigidbody2D>();

        TrailRenderer trailRenderer = FallingBall.GetComponent<TrailRenderer>();

        if (trailRenderer != null)
        {
            trailRenderer.enabled = false;
        }

        rb.velocity = Vector2.down * 2f;

        Destroy( FallingBall ,6f);
    }

    void GameScene()
    {
        SceneManager.LoadScene("Game");
    }

    void MarketScene()
    {
        SceneManager.LoadScene("Market");
    }
}
