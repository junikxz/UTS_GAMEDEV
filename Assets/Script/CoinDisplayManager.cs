using UnityEngine;
using TMPro;

public class CoinDisplayManager : MonoBehaviour
{
    public TextMeshProUGUI coinText;

    void Start()
    {
        PlayerPrefs.SetInt("coins", 0); // reset ke 0 setiap game mulai
        UpdateCoinDisplay();
    }

    void UpdateCoinDisplay()
    {
        int currentCoins = PlayerPrefs.GetInt("coins", 0);
        coinText.text = "Koin: " + currentCoins.ToString();
    }

    // Fungsi ini bisa kamu panggil dari QuizManager kalau mau update otomatis setelah kuis
    public void RefreshCoins()
    {
        UpdateCoinDisplay();
    }
}
