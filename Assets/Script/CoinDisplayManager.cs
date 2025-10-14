using UnityEngine;
using TMPro;

public class CoinDisplayManager : MonoBehaviour
{
    // ✨ JADIKAN SINGLETON
    public static CoinDisplayManager instance;

    public TextMeshProUGUI coinText;

    void Awake()
    {
        // Setup singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // ✨ AKTIFKAN LAGI BARIS INI SELAMA TESTING
        PlayerPrefs.SetInt("coins", 0);
        PlayerPrefs.Save(); // Pastikan untuk menyimpan perubahan

        UpdateCoinDisplay();
    }

    void UpdateCoinDisplay()
    {
        if (coinText == null) return; // Pengaman jika teks belum di-assign

        int currentCoins = PlayerPrefs.GetInt("coins", 0);
        coinText.text = "Koin: " + currentCoins.ToString();
    }

    // Fungsi ini akan kita panggil dari luar
    public void RefreshCoins()
    {
        UpdateCoinDisplay();
    }
}