using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Sc_pauseManager : MonoBehaviour
{
    public static Sc_pauseManager Instance { get; private set; }
    public bool isPaused = false;

    [Header("Panel Referensi")]
    public GameObject pauseMenuPanel;

    [Header("Nama Scene")]
    public string mainMenuSceneName = "Menu";
    public GameObject firstSelectedButton;

    void Awake()
    {
        // Cek jika sudah ada instance lain
        if (Instance != null && Instance != this)
        {
            // Jika ada, hancurkan game object ini (duplikat)
            Destroy(gameObject);
        }
        else
        {
            // Jika belum ada, jadikan ini sebagai instance satu-satunya
            Instance = this;

            // (Opsional) Jika ingin PauseManager tetap ada saat ganti scene
            // DontDestroyOnLoad(gameObject); 
        }
    }

    void Update()
    {
        // Cek jika tombol Escape ditekan
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                // Jika sedang pause (dan options tidak terbuka), lanjutkan game
                if (isPaused)
                {
                    Resume();
                }
            }
            else
            {
                // Jika tidak sedang pause, pause game
                Pause();
            }
        }
    }

    public void Resume()
    {
        EventSystem.current.SetSelectedGameObject(null);
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f; // Kembalikan waktu ke normal
        isPaused = false;
        // Sembunyikan cursor lagi (jika Anda punya fungsi ini)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Pause()
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f; // Hentikan waktu game
        isPaused = true;
        // Tampilkan cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        EventSystem.current.SetSelectedGameObject(null);
        // Atur fokus baru ke tombol
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);
    }

    public void LoadMainMenu()
    {
        // Pastikan waktu kembali normal sebelum pindah scene
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene(mainMenuSceneName);
    }


}
