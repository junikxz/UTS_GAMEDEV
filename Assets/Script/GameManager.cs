using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int jumlahKoin { get; private set; }
    public TextMeshProUGUI teksKoinUI;

    void Awake()
    {
        if (instance == null) { instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }
    public void TambahKoin(int jumlah) { jumlahKoin += jumlah; UpdateKoinUI(); }
    void UpdateKoinUI() { if (teksKoinUI != null) teksKoinUI.text = "Koin: " + jumlahKoin; }
}