using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI Referensi")]
    public TextMeshProUGUI coinTextUI;         // Ganti nama dari teksKoinUI
    public GameObject victoryPanel;            // Panel yang muncul di akhir
    public TextMeshProUGUI rewardMessageText;    // Teks di dalam victoryPanel
    public Image flagImage;                    // Tempat untuk menampilkan gambar bendera

    [Header("Pengaturan Hadiah")]
    public Sprite[] flagSprites; // Daftar gambar bendera (PNG) yang akan Anda masukkan

    // Variabel ini akan otomatis reset ke 0 setiap kali game dimulai
    private int currentCoins = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Sembunyikan panel kemenangan di awal
        if (victoryPanel != null) victoryPanel.SetActive(false);
        UpdateCoinDisplay(); // Pastikan tampilan koin 0 di awal
    }

    // Fungsi ini dipanggil oleh InteractionManager setelah kuis berhasil
    public void TambahKoin(int amount)
    {
        currentCoins += amount;
        UpdateCoinDisplay();
    }

    void UpdateCoinDisplay()
    {
        if (coinTextUI != null)
        {
            coinTextUI.text = "Koin: " + currentCoins.ToString();
        }
    }

    // FUNGSI BARU: Ini akan dipanggil oleh Pos 5
    public void GiveFlagReward()
    {
        // Pastikan ada gambar bendera yang bisa diberikan
        if (flagSprites == null || flagSprites.Length == 0)
        {
            Debug.LogError("Tidak ada gambar bendera (Flag Sprites) yang di-set di GameManager!");
            return;
        }

        // 1. Pilih bendera secara acak dari daftar
        Sprite randomFlag = flagSprites[Random.Range(0, flagSprites.Length)];

        // 2. Tampilkan di UI
        if (flagImage != null)
        {
            flagImage.sprite = randomFlag;
        }

        // // 3. Atur pesan kemenangan
        // if (rewardMessageText != null)
        // {
        //     rewardMessageText.text = $"Dengan 400 koin, Anda berhasil mengibarkan Bendera Pusaka!";
        // }

        // 4. Tampilkan panel kemenangan
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            InteractionManager.instance.ShowCursor(); // Tampilkan cursor agar bisa klik tombol
        }
    }
}