using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GuessPictureQuiz : BaseQuizLogic
{
    public GuessPictureData quizData;

    public override void StartQuiz()
    {
        var panel = InteractionManager.instance.guessPicturePanel;
        var imageUI = InteractionManager.instance.guessPictureImage;
        var inputField = InteractionManager.instance.guessPictureInputField;
        var submitButton = InteractionManager.instance.guessPictureSubmitButton;

        panel.SetActive(true);
        imageUI.sprite = quizData.gambarTebakan;
        inputField.text = ""; // Kosongkan input field

        submitButton.onClick.RemoveAllListeners();
        submitButton.onClick.AddListener(CheckAnswer);
    }

    private void CheckAnswer()
    {
        var inputField = InteractionManager.instance.guessPictureInputField;
        
        // Membandingkan jawaban (abaikan huruf besar/kecil dan spasi)
        if (inputField.text.Trim().Equals(quizData.jawabanBenar, System.StringComparison.OrdinalIgnoreCase))
        {
            // Jawaban Benar
            InteractionManager.instance.guessPicturePanel.SetActive(false);
            GameManager.instance.TambahKoin(quizData.hadiahKoin);
            OnQuizSuccess();
        }
        else
        {
            // Jawaban Salah
            InteractionManager.instance.guessPicturePanel.SetActive(false);
            OnQuizFailed("Jawaban kurang tepat! Coba lagi.");
        }
    }
}