using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MarketMenuChose : MonoBehaviour
{
    

    [SerializeField] Sprite Selected, NonSelected;
    [SerializeField] Text CoinText;

    [SerializeField] AudioSource Button;
    [SerializeField] AudioSource ColorChoose;
    [SerializeField] AudioSource Background;
    [SerializeField] AudioSource Buy;


    Dictionary<string, int> tagToIndex = new Dictionary<string, int>()
{
    { "0", 0 },
    { "1", 1 },
    { "2", 2 },
    { "3", 3 },
    { "4", 4 },
    { "5", 5 },
    { "6", 6 },
    { "7", 7 },
    { "8", 8 },
    { "9", 9 },
    { "10", 10 },
    { "11", 11 },
    
    // Di�er tag'ler burada tan�mlanabilir
};

    

    void Start()
    {
        if(!MainMenu.IsSoundOn) Background.Stop();
        // Cihaz�n ekran yenileme h�z�n� al
        double refreshRate = Screen.currentResolution.refreshRateRatio.value;

        // FPS ayar�n� ekran yenileme h�z�na g�re ayarla
        Application.targetFrameRate = Mathf.RoundToInt((float)refreshRate);
        CoinText.text = PlayerPrefs.GetInt("Coins", 0).ToString();
    }

    
    void Update()
    {
        ChoosedControl();

        if (MarketMenuButtons.soundBuy)
        {
            if (MainMenu.IsSoundOn)
            {
                Buy.Play();
                MarketMenuButtons.soundBuy = false;
            }
        }
    }

    public void ChooseColor()
    {
        int tagIndex;
        
        
        if (tagToIndex.TryGetValue(this.gameObject.tag, out tagIndex))
        {
            if (tagIndex != 0)
            {
                if (PlayerPrefs.GetInt("CannonUnlocked" + (tagIndex - 1), 0) == 1)
                {
                    PlayerPrefs.SetInt("ChoosedOne", tagIndex);
                    if(MainMenu.IsSoundOn) 
                    ColorChoose.Play();
                }
            }
            else
            {
                PlayerPrefs.SetInt("ChoosedOne", 0);
                if (MainMenu.IsSoundOn)
                    ColorChoose.Play();
            }
        }
    }

    void ChoosedControl()
    {
        int tagIndex;


        if (tagToIndex.TryGetValue(this.gameObject.tag, out tagIndex))
        {
            if(PlayerPrefs.GetInt("ChoosedOne" , 0) == tagIndex)
            {
                this.gameObject.GetComponent<Image>().sprite = Selected;

            }
            else this.gameObject.GetComponent<Image>().sprite = NonSelected;
        }
    }

    public void backToMENU()
    {
        if(MainMenu.IsSoundOn) Button.Play();
        Invoke("menu", 0.05f);
    }

    void menu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void addmoney()
    {
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins", 0) + 5000);
        SceneManager.LoadScene("Market");
    }
}
