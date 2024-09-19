using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MarketMenuButtons : MonoBehaviour
{
    private bool[] CannonIndexUnlocked = new bool[11];
    [SerializeField] GameObject[] BuyButtons = new GameObject[11];
    

    [SerializeField] Text CoinText;

    // Tag'lere karþýlýk gelen integer deðerler
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
    
    // Diðer tag'ler burada tanýmlanabilir
};

    
    void Start()
    {
        CannonIndexUnlocked[0] = true;
        

        // Tüm butonlarý kontrol et ve duruma göre sil
        for (int i = 0; i < BuyButtons.Length; i++)
        {
            if(PlayerPrefs.GetInt("CannonUnlocked" + i , 0) == 1)
            {
                BuyButtons[i].SetActive(false); 
            }
        }
    }

    
    void Update()
    {
       
    }
    public static bool soundBuy = false;
    public void BuyButton()
    {
        int tagIndex;

        // BuyButton'a basýlan butonun tag'ine göre ilgili index'i alýyoruz
        if (tagToIndex.TryGetValue(this.gameObject.tag, out tagIndex))
        {
            if (int.Parse(CoinText.text) >= 500)
            {
                // Eðer yeterli Coins varsa, topun kilidini açýyoruz
                CannonIndexUnlocked[tagIndex] = true;
                soundBuy = true;
                
                PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - 500);
                CoinText.text = PlayerPrefs.GetInt("Coins").ToString();

                // PlayerPrefs'e bu topun kilidini açtýðýmýzý kaydediyoruz
                PlayerPrefs.SetInt("CannonUnlocked" + tagIndex, 1);

                // Ýlgili butonu yok ediyoruz
                Destroy(BuyButtons[tagIndex]);
            }
        }
    }

    public void PP()
    {
        PlayerPrefs.DeleteAll();

    }

    
}
