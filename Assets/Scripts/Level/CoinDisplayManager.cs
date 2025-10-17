using UnityEngine;
using TMPro;

public class CoinDisplayManager : MonoBehaviour
{
    // ✨ SINGLETON
    public static CoinDisplayManager instance;

    [Header("UI References")]
    public TextMeshProUGUI coinText;
    public GameObject rewardPanel; // Panel popup hadiah
    public TextMeshProUGUI rewardMessageText; // Text pesan hadiah

    [Header("Reward Settings")]
    public int coinsRequiredForFlag = 400;
    public string[] countryFlags = { "Indonesia", "Jepang", "Korea", "Amerika", "Inggris" }; // Daftar bendera

    private bool hasReceivedFlag = false;

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
        // ✨ TESTING: Reset coins
        // PlayerPrefs.SetInt("coins", 0);
        // PlayerPrefs.DeleteKey("hasReceivedFlag"); // Reset hadiah
        // PlayerPrefs.Save();

        // Cek apakah sudah pernah dapat hadiah
        hasReceivedFlag = PlayerPrefs.GetInt("hasReceivedFlag", 0) == 1;

        // Sembunyikan reward panel di awal
        if (rewardPanel != null)
            rewardPanel.SetActive(false);

        UpdateCoinDisplay();
    }

    void UpdateCoinDisplay()
    {
        if (coinText == null) return;

        int currentCoins = PlayerPrefs.GetInt("coins", 0);
        coinText.text = "Koin: " + currentCoins.ToString();

        // Cek apakah layak dapat hadiah
        CheckForReward(currentCoins);
    }

    void CheckForReward(int currentCoins)
    {
        // Jika sudah dapat 400+ koin DAN belum pernah dapat hadiah
        if (currentCoins >= coinsRequiredForFlag && !hasReceivedFlag)
        {
            GiveRandomFlag();
        }
    }

    void GiveRandomFlag()
    {
        // Pilih bendera random
        string randomFlag = countryFlags[Random.Range(0, countryFlags.Length)];

        // Simpan bendera yang didapat
        PlayerPrefs.SetString("receivedFlag", randomFlag);
        PlayerPrefs.SetInt("hasReceivedFlag", 1);
        PlayerPrefs.Save();

        hasReceivedFlag = true;

        // Tampilkan popup hadiah
        ShowRewardPopup(randomFlag);

        Debug.Log("Selamat! Anda mendapat bendera: " + randomFlag);
    }

    void ShowRewardPopup(string flagName)
    {
        if (rewardPanel != null)
        {
            rewardPanel.SetActive(true);

            if (rewardMessageText != null)
            {
                rewardMessageText.text = $"🎉 Selamat! 🎉\n\nAnda telah mendapatkan\nBendera {flagName}!";
            }

            // Auto-close setelah 3 detik (opsional)
            Invoke("CloseRewardPopup", 3f);
        }
    }

    public void CloseRewardPopup()
    {
        if (rewardPanel != null)
            rewardPanel.SetActive(false);
    }

    // Fungsi refresh dari luar
    public void RefreshCoins()
    {
        UpdateCoinDisplay();
    }

    // Fungsi untuk cek bendera apa yang dimiliki
    public string GetReceivedFlag()
    {
        return PlayerPrefs.GetString("receivedFlag", "Belum ada");
    }

    // Fungsi untuk reset hadiah (untuk testing)
    public void ResetReward()
    {
        PlayerPrefs.DeleteKey("hasReceivedFlag");
        PlayerPrefs.DeleteKey("receivedFlag");
        hasReceivedFlag = false;
        PlayerPrefs.Save();
        Debug.Log("Hadiah direset!");
    }
}