using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizManager : BaseQuizLogic
{
    public static QuizManager Instance;
    [Header("UI References (Milik Kuis Ini)")]
    public GameObject quizPanel;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI timerText; // PASTIKAN INI DIISI DI INSPECTOR
    public Button[] optionButtons;
    [System.Serializable]
    public class Question
    {
        public string question;
        public string[] options;
        public int correctAnswerIndex;
    }

    [Header("Question List")]
    public List<Question> questions;
    private int currentQuestionIndex = 0;

    [Header("Timer Settings")]
    public float timePerQuestion = 15f;
    private float currentTime;
    private bool isCountingDown = false;

    [Header("Feedback Colors")]
    public Color correctColor = Color.green;
    public Color wrongColor = Color.red;
    public Color defaultColor = Color.white;

    void Start()
    {
        if (quizPanel != null)
        {
            quizPanel.SetActive(false);
        }
    }

    void Update()
    {
        // Hanya update timer jika isCountingDown aktif
        if (isCountingDown)
        {
            currentTime -= Time.deltaTime;

            // Pastikan timerText tidak null sebelum digunakan
            if (timerText != null)
            {
                timerText.text = Mathf.CeilToInt(currentTime).ToString();
            }

            if (currentTime <= 0)
            {
                isCountingDown = false;
                CompleteQuiz(false, "⏰ Waktu habis! Coba lagi.");
            }
        }
    }

    public override void StartQuiz()
    {
        Debug.Log("✅ Memulai Kuis Pilihan Ganda!");

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        quizPanel.SetActive(true);
        currentQuestionIndex = 0;
        DisplayNextQuestion();
    }

    void DisplayNextQuestion()
    {
        if (currentQuestionIndex >= questions.Count)
        {
            CompleteQuiz(true, "Selamat!");
            return;
        }

        Question q = questions[currentQuestionIndex];
        questionText.text = q.question;

        for (int i = 0; i < optionButtons.Length; i++)
        {
            optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = q.options[i];
            optionButtons[i].onClick.RemoveAllListeners();
            int index = i;
            optionButtons[i].onClick.AddListener(() => OnAnswerSelected(index));
            optionButtons[i].image.color = defaultColor;
            optionButtons[i].interactable = true; // Aktifkan kembali tombol
        }

        currentTime = timePerQuestion;
        isCountingDown = true; // Timer mulai berjalan di sini
    }

    void OnAnswerSelected(int index)
    {
        if (!isCountingDown) return;

        // Hentikan timer & nonaktifkan semua tombol agar tidak bisa diklik ganda
        isCountingDown = false;
        foreach (var btn in optionButtons)
        {
            btn.interactable = false;
        }

        Question q = questions[currentQuestionIndex];
        bool isCorrect = (index == q.correctAnswerIndex);

        if (isCorrect)
        {
            optionButtons[index].image.color = correctColor;
            currentQuestionIndex++;
            Invoke(nameof(DisplayNextQuestion), 1.2f); // Beri jeda 1.2 detik
        }
        else
        {
            optionButtons[index].image.color = wrongColor;
            Invoke(nameof(FailQuiz), 1.2f);
        }
    }

    void FailQuiz()
    {
        CompleteQuiz(false, "Jawaban salah! Coba lagi.");
    }

    void CompleteQuiz(bool success, string reason)
    {
        isCountingDown = false;
        quizPanel.SetActive(false);

        if (success)
        {
            OnQuizSuccess();
        }
        else
        {
            OnQuizFailed(reason);
        }
    }
}