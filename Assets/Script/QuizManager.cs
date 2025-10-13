using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject quizPanel;
    public GameObject preQuizPanel;
    public GameObject feedbackPanel;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI feedbackText;
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

    [Header("Reward System")]
    public int rewardCoins = 100;
    private bool allAnswersCorrect = true;

    void Update()
    {
        if (isCountingDown)
        {
            currentTime -= Time.deltaTime;
            timerText.text = Mathf.CeilToInt(currentTime).ToString();

            if (currentTime <= 0)
            {
                Debug.Log("⏰ Waktu habis! Kembali ke preQuizPanel...");
                isCountingDown = false;
                allAnswersCorrect = false; // gagal
                ReturnToPreQuiz();
            }
        }
    }

    public void StartQuiz()
    {
        Debug.Log("✅ StartQuiz() dipanggil!");
        quizPanel.SetActive(true);
        preQuizPanel.SetActive(false);
        feedbackPanel.SetActive(false);

        currentQuestionIndex = 0;
        allAnswersCorrect = true;
        DisplayNextQuestion();
    }

    void DisplayNextQuestion()
    {
        if (currentQuestionIndex >= questions.Count)
        {
            EndQuiz();
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
        }

        // Reset timer setiap pertanyaan baru
        currentTime = timePerQuestion;
        isCountingDown = true;
    }

    void OnAnswerSelected(int index)
    {
        if (!isCountingDown) return;

        isCountingDown = false;
        Question q = questions[currentQuestionIndex];
        bool isCorrect = (index == q.correctAnswerIndex);

        if (isCorrect)
        {
            optionButtons[index].image.color = correctColor;
            Debug.Log("✅ Jawaban benar!");
            Invoke(nameof(NextQuestion), 1.0f);
        }
        else
        {
            optionButtons[index].image.color = wrongColor;
            Debug.Log("❌ Jawaban salah!");
            allAnswersCorrect = false;
            Invoke(nameof(ReturnToPreQuiz), 1.0f);
        }
    }

    void NextQuestion()
    {
        currentQuestionIndex++;
        DisplayNextQuestion();
    }

    void ReturnToPreQuiz()
    {
        quizPanel.SetActive(false);
        preQuizPanel.SetActive(true);
    }

    void EndQuiz()
    {
        quizPanel.SetActive(false);

        if (allAnswersCorrect)
        {
            int currentCoins = PlayerPrefs.GetInt("coins", 0);
            currentCoins += rewardCoins;
            PlayerPrefs.SetInt("coins", currentCoins);
            PlayerPrefs.Save();

            feedbackPanel.SetActive(true);
            feedbackText.text = $"Selamat! Kamu menjawab semua pertanyaan dengan benar! Silahkan pergi ke Pos 2 sesuai petunjuk yang diberikan dan selesaikan soal medium!\n\n+{rewardCoins} Koin";

            // 🔁 Update tampilan koin di pojok layar
            FindObjectOfType<CoinDisplayManager>()?.RefreshCoins();
        }

        else
        {
            // Kalau ada yang salah, langsung kembali ke preQuizPanel
            preQuizPanel.SetActive(true);
        }
    }

    public void CloseFeedback()
    {
        feedbackPanel.SetActive(false);

        // ✅ Pastikan hanya unlock kalau semua jawaban benar
        if (allAnswersCorrect)
        {
            Debug.Log("[QuizManager] Semua pertanyaan benar — membuka Pos berikutnya!");
            PosManager.instance.UnlockNextPos(); // 👈 ini yang menyalakan NPC Pos 2
        }
    }

}
