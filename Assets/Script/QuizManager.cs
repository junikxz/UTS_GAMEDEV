using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject quizPanel;
    public GameObject preQuizPanel;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI timerText;
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
                ReturnToPreQuiz();
            }
        }
    }

    public void StartQuiz()
    {
        Debug.Log("✅ StartQuiz() dipanggil!");
        quizPanel.SetActive(true);
        preQuizPanel.SetActive(false);
        currentQuestionIndex = 0;
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

        // Mulai timer baru
        currentTime = timePerQuestion;
        isCountingDown = true;
    }

    void OnAnswerSelected(int index)
    {
        if (!isCountingDown) return; // biar gak bisa spam klik setelah timeout

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
            Debug.Log("❌ Jawaban salah! Kembali ke preQuizPanel...");
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
        Debug.Log("🎉 Kuis selesai!");
        quizPanel.SetActive(false);
    }
}
