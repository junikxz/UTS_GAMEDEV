using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections; // ✨ Perlu ini untuk Coroutine

public class GamePos2 : BaseQuizLogic
{
    [Header("UI Panel")]
    public GameObject miniGamePanel;

    public Button submitButton;
    public TMP_InputField answerInput;
    public RawImage questionImage;
    public TextMeshProUGUI timerText;

    [System.Serializable]
    public class Question { public Texture image; public string correctAnswer; }
    public Question[] questions;

    public float timePerQuestion = 20f;
    private float currentTime;
    private bool isPlaying = false;
    private int currentIndex = 0;

    [Header("Feedback Colors")] // ✨ Baru: Warna untuk feedback
    public Color correctColor = Color.green;
    public Color wrongColor = Color.red;
    public Color defaultInputColor = Color.white; // Warna default input field
    public float feedbackDelay = 1.0f; // Jeda sebelum next/end

    void Start()
    {
        miniGamePanel.SetActive(false);
        if (submitButton != null) submitButton.onClick.AddListener(OnSubmit);
        answerInput.onEndEdit.AddListener(OnInputEndEdit); // ✨ Baru: Handle input selesai edit
    }

    public override void StartQuiz()
    {
        // Beri tahu Player Controller untuk berhenti mengunci kursor
        // Pastikan Anda sudah punya PlayerController.isUIActive di Player Controller Anda
        // PlayerController.isUIActive = true; 

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        currentIndex = 0;
        miniGamePanel.SetActive(true);
        isPlaying = true;
        ShowQuestion();
    }

    // ✨ Baru: Memungkinkan submit dengan Enter
    void OnInputEndEdit(string text)
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            OnSubmit();
        }
    }

    void OnSubmit()
    {
        if (!isPlaying) return; // Pengaman agar tidak submit saat tidak bermain

        // Hentikan timer dan nonaktifkan input & tombol untuk mencegah submit ganda
        isPlaying = false;
        answerInput.interactable = false;
        submitButton.interactable = false;
        
        string playerAnswer = answerInput.text.Trim().ToLower();
        string correctAnswer = questions[currentIndex].correctAnswer.Trim().ToLower();

        Debug.Log($"Player mengetik: [{playerAnswer}]");
        Debug.Log($"Jawaban benar seharusnya: [{correctAnswer}]");

        if (playerAnswer == correctAnswer)
        {
            Debug.Log("Hasil: BENAR!");
            StartCoroutine(CorrectAnswerFeedback()); // ✨ Panggil coroutine baru
        }
        else
        {
            Debug.Log("Hasil: SALAH!");
            StartCoroutine(WrongAnswerFeedback()); // ✨ Panggil coroutine baru
        }
    }

    // ✨ Baru: Coroutine untuk feedback jawaban benar
    IEnumerator CorrectAnswerFeedback()
    {
        answerInput.image.color = correctColor; // Ubah warna input field
        yield return new WaitForSeconds(feedbackDelay); // Tunggu sejenak

        answerInput.image.color = defaultInputColor; // Kembalikan warna
        answerInput.interactable = true; // Aktifkan kembali input field
        submitButton.interactable = true; // Aktifkan kembali tombol submit

        currentIndex++;
        if (currentIndex >= questions.Length)
        {
            EndMiniGame(true); // Semua soal terjawab
        }
        else
        {
            ShowQuestion(); // Lanjut soal berikutnya
            isPlaying = true; // Lanjutkan permainan
        }
    }

    // ✨ Baru: Coroutine untuk feedback jawaban salah
    IEnumerator WrongAnswerFeedback()
    {
        answerInput.image.color = wrongColor; // Ubah warna input field
        yield return new WaitForSeconds(feedbackDelay); // Tunggu sejenak

        // Langsung akhiri game dengan status gagal
        EndMiniGame(false); 
    }

    void EndMiniGame(bool success)
    {
        isPlaying = false;
        miniGamePanel.SetActive(false);

        // Kembalikan kontrol kursor (jika PlayerController Anda sudah disetting)
        // PlayerController.isUIActive = false; 
        
        // Pastikan input field dan tombol kembali aktif (jika ingin bisa berinteraksi di preQuizPanel)
        answerInput.interactable = true;
        submitButton.interactable = true;
        answerInput.image.color = defaultInputColor;

        if (success)
        {
            OnQuizSuccess();
        }
        else
        {
            OnQuizFailed("❌ Jawaban salah! Coba lagi dari awal pos.");
        }
    }

    #region Logika Kuis (Tidak berubah secara fungsional)
    void Update()
    {
        if (!isPlaying) return; // Hanya update timer jika isPlaying aktif
        currentTime -= Time.deltaTime;
        if (timerText != null) // Pengaman jika timerText belum di-assign
        {
            timerText.text = Mathf.CeilToInt(currentTime).ToString();
        }
        if (currentTime <= 0)
        {
            EndMiniGame(false);
        }
    }
    void ShowQuestion()
    {
        questionImage.texture = questions[currentIndex].image;
        answerInput.text = ""; // Kosongkan input field
        answerInput.ActivateInputField(); // Fokuskan input field agar bisa langsung mengetik
        currentTime = timePerQuestion;
        isPlaying = true; // Pastikan isPlaying aktif
    }
    #endregion
}