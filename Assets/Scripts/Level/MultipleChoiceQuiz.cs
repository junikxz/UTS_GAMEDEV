using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

// Mewarisi dari BaseQuizLogic
public class MultipleChoiceQuiz : BaseQuizLogic
{
    public QuizData quizData; // Data ScriptableObject untuk kuis ini

    private int questionIndex = 0;
    private Coroutine timerCoroutine;

    public override void StartQuiz()
    {
        // Beritahu InteractionManager untuk menampilkan UI yang benar
        InteractionManager.instance.multipleChoicePanel.SetActive(true);
        questionIndex = 0;
        ShowNextQuestion();
    }

    private void ShowNextQuestion()
    {
        var panel = InteractionManager.instance.multipleChoicePanel;
        var questionText = panel.transform.Find("QuestionText").GetComponent<TextMeshProUGUI>(); // Sesuaikan nama objek
        var buttons = InteractionManager.instance.mcqAnswerButtons;

        Pertanyaan p = quizData.daftarPertanyaan[questionIndex];
        questionText.text = p.teksPertanyaan;

        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = p.jawaban[i];
            int buttonIndex = i;
            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(() => CheckAnswer(buttonIndex));
        }

        if (timerCoroutine != null) StopCoroutine(timerCoroutine);
        timerCoroutine = StartCoroutine(StartTimer());
    }

    private void CheckAnswer(int index)
    {
        if (timerCoroutine != null) StopCoroutine(timerCoroutine);

        if (index == quizData.daftarPertanyaan[questionIndex].indeksJawabanBenar)
        {
            if (questionIndex >= quizData.daftarPertanyaan.Count - 1)
            {
                // Berhasil menyelesaikan semua
                InteractionManager.instance.multipleChoicePanel.SetActive(false);
                GameManager.instance.TambahKoin(quizData.hadiahKoin);
                OnQuizSuccess(); // Panggil fungsi sukses dari base class
            }
            else
            {
                // Lanjut pertanyaan berikutnya
                questionIndex++;
                ShowNextQuestion();
            }
        }
        else
        {
            InteractionManager.instance.multipleChoicePanel.SetActive(false);
            OnQuizFailed("Jawaban Salah! Silakan coba lagi.");
        }
    }

    IEnumerator StartTimer()
    {
        var timerText = InteractionManager.instance.mcqTimerText;
        float timeLeft = InteractionManager.instance.quizTimePerQuestion;
        while (timeLeft > 0)
        {
            timerText.text = Mathf.CeilToInt(timeLeft).ToString();
            timeLeft -= Time.deltaTime;
            yield return null;
        }
        timerText.text = "0";
        InteractionManager.instance.multipleChoicePanel.SetActive(false);
        OnQuizFailed("Waktu Habis! Silakan coba lagi.");
    }
}