using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GuessTheSongQuiz : BaseQuizLogic
{
    [Header("Data Kuis")]
    public SongQuizData songData;

    [Header("Referensi UI")]
    public GameObject songQuizPanel;
    public Button playSongButton;
    public TMP_InputField answerInputField;
    public Button submitButton;
    public TextMeshProUGUI timerText;

    [Header("Pengaturan Permainan")]
    public float totalQuizTime = 90f;
    public float songPlayDuration = 10f; // Durasi pemutaran setiap lagu (detik)

    private AudioSource audioSource;
    private int currentQuestionIndex;
    private Coroutine quizTimerCoroutine;
    private Coroutine songPlaybackCoroutine;

    void Start()
    {
        if (answerInputField != null)
        {
            answerInputField.onEndEdit.AddListener(OnInputEndEdit);
        }
    }

    void OnInputEndEdit(string text)
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            CheckAnswer();
        }
    }


    public override void StartQuiz()
    {
        audioSource = FindObjectOfType<AudioSource>();
        if (audioSource == null)
        {
            audioSource = new GameObject("QuizAudioSource").AddComponent<AudioSource>();
        }

        songQuizPanel.SetActive(true);
        InteractionManager.instance.ShowCursor();

        submitButton.onClick.RemoveAllListeners();
        submitButton.onClick.AddListener(CheckAnswer);

        if (quizTimerCoroutine != null) StopCoroutine(quizTimerCoroutine);
        quizTimerCoroutine = StartCoroutine(QuizTimer());

        currentQuestionIndex = 0;
        ShowQuestion();
    }

    void ShowQuestion()
    {
        if (currentQuestionIndex >= songData.questions.Count) return;

        answerInputField.text = "";
        playSongButton.onClick.RemoveAllListeners();
        playSongButton.onClick.AddListener(PlayCurrentSong);
        answerInputField.ActivateInputField();
    }

    void PlayCurrentSong()
    {
        if (songPlaybackCoroutine != null)
        {
            StopCoroutine(songPlaybackCoroutine);
        }
        songPlaybackCoroutine = StartCoroutine(PlaySongClip());
    }

    IEnumerator PlaySongClip()
    {
        AudioClip clip = songData.questions[currentQuestionIndex].songClip;
        if (clip != null)
        {
            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.Play();

            yield return new WaitForSeconds(songPlayDuration);

            if (audioSource.isPlaying && audioSource.clip == clip)
            {
                audioSource.Stop();
            }
        }
    }

    IEnumerator QuizTimer()
    {
        float timeLeft = totalQuizTime;
        while (timeLeft > 0)
        {
            int minutes = Mathf.FloorToInt(timeLeft / 60);
            int seconds = Mathf.FloorToInt(timeLeft % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            timeLeft -= Time.deltaTime;
            yield return null;
        }

        StopAllPlayback();
        timerText.text = "00:00";
        songQuizPanel.SetActive(false);
        InteractionManager.instance.HandleQuizFailure("Waktu Habis!");
    }

    void CheckAnswer()
    {
        string correctAnswer = songData.questions[currentQuestionIndex].correctTitle;
        string playerAnswer = answerInputField.text.Trim();

        if (playerAnswer.Equals(correctAnswer, System.StringComparison.OrdinalIgnoreCase))
        {
            // ✅ Jawaban benar → efek hijau
            StartCoroutine(ShowAnswerEffect(true));
        }
        else
        {
            // ❌ Jawaban salah → efek merah
            StartCoroutine(ShowAnswerEffect(false));
        }
    }

    IEnumerator ShowAnswerEffect(bool isCorrect)
    {
        // Simpan warna asli background input
        Color originalInputColor = answerInputField.image.color;
        Color originalButtonColor = submitButton.image.color;

        // Tentukan warna target (hijau atau merah)
        Color targetColor = isCorrect
            ? new Color(0.4f, 1f, 0.4f) // hijau muda
            : new Color(1f, 0.4f, 0.4f); // merah muda

        float duration = 0.3f;
        float elapsed = 0f;

        // Fade in ke warna target
        while (elapsed < duration)
        {
            answerInputField.image.color = Color.Lerp(originalInputColor, targetColor, elapsed / duration);
            submitButton.image.color = Color.Lerp(originalButtonColor, targetColor, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        answerInputField.image.color = targetColor;
        submitButton.image.color = targetColor;

        // Tahan warna sebentar
        yield return new WaitForSeconds(0.5f);

        // Fade out kembali ke warna asli
        elapsed = 0f;
        while (elapsed < duration)
        {
            answerInputField.image.color = Color.Lerp(targetColor, originalInputColor, elapsed / duration);
            submitButton.image.color = Color.Lerp(targetColor, originalInputColor, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        answerInputField.image.color = originalInputColor;

        // 🔁 Lanjut logika setelah efek selesai
        if (isCorrect)
        {
            currentQuestionIndex++;
            if (currentQuestionIndex >= songData.questions.Count)
            {
                StopAllPlayback();
                songQuizPanel.SetActive(false);
                InteractionManager.instance.HandleQuizSuccess();
            }
            else
            {
                if (audioSource.isPlaying) audioSource.Stop();
                ShowQuestion();
            }
        }
        else
        {
            StopAllPlayback();
            songQuizPanel.SetActive(false);
            InteractionManager.instance.HandleQuizFailure("Judul lagu salah!");
        }
    }

    void StopAllPlayback()
    {
        if (quizTimerCoroutine != null) StopCoroutine(quizTimerCoroutine);
        if (songPlaybackCoroutine != null) StopCoroutine(songPlaybackCoroutine);
        if (audioSource != null && audioSource.isPlaying) audioSource.Stop();
    }
}
