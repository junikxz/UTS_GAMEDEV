using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GamePos2 : MonoBehaviour
{
    [Header("UI")]
    public GameObject miniGamePanel;
    public Image questionImage;
    public TMP_InputField answerInput;
    public Button submitButton;
    public TextMeshProUGUI timerText;
    public GameObject feedbackPanel;
    public TextMeshProUGUI feedbackText;

    [System.Serializable]
    public class Question
    {
        public Sprite image;
        public string correctAnswer;
    }

    [Header("Daftar Soal Kebangsaan")]
    public Question[] questions;
    private int currentIndex = 0;

    [Header("Timer Settings")]
    public float timePerQuestion = 20f;
    private float currentTime;
    private bool isPlaying = false;

    private bool allCorrect = true;

    void Start()
    {
        miniGamePanel.SetActive(false);
        feedbackPanel.SetActive(false);
        submitButton.onClick.AddListener(OnSubmit);
    }

    void Update()
    {
        if (!isPlaying) return;

        currentTime -= Time.deltaTime;
        timerText.text = Mathf.CeilToInt(currentTime).ToString();

        if (currentTime <= 0)
        {
            isPlaying = false;
            feedbackText.text = "⏰ Waktu habis! Coba lagi dari awal pos.";
            feedbackPanel.SetActive(true);
            miniGamePanel.SetActive(false);
        }
    }

    public void StartMiniGame()
    {
        currentIndex = 0;
        miniGamePanel.SetActive(true);
        feedbackPanel.SetActive(false);
        ShowQuestion();
        isPlaying = true;
    }

    void ShowQuestion()
    {
        if (currentIndex >= questions.Length)
        {
            EndMiniGame();
            return;
        }

        questionImage.sprite = questions[currentIndex].image;
        answerInput.text = "";
        currentTime = timePerQuestion;
    }

    void OnSubmit()
    {
        string playerAnswer = answerInput.text.Trim().ToLower();
        string correctAnswer = questions[currentIndex].correctAnswer.Trim().ToLower();

        if (playerAnswer == correctAnswer)
        {
            currentIndex++;
            if (currentIndex < questions.Length)
                ShowQuestion();
            else
                EndMiniGame();
        }
        else
        {
            allCorrect = false;
            feedbackText.text = "❌ Jawaban salah! Coba lagi dari awal pos.";
            feedbackPanel.SetActive(true);
            miniGamePanel.SetActive(false);
            isPlaying = false;
        }
    }

    void EndMiniGame()
    {
        isPlaying = false;
        miniGamePanel.SetActive(false);

        if (allCorrect)
        {
            feedbackPanel.SetActive(true);
            feedbackText.text = "🎉 Hebat! Kamu mengenal budaya Nusantara dengan baik!\nSilakan menuju Pos 3.";
            
            // 🚩 Pemicu munculnya POS berikutnya
            PosManager.instance.UnlockNextPos();
        }
    }

    public void CloseFeedback()
    {
        feedbackPanel.SetActive(false);
    }
}
