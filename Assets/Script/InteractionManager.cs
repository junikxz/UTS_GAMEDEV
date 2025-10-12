using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager instance;

    [Header("Panel Utama")]
    public GameObject dialoguePanel;
    public GameObject feedbackPanel;
    public GameObject cluePanel;
    public GameObject preQuizPanel;

    [Header("Panel Pre-Quiz")]
    public TextMeshProUGUI preQuizText;
    [TextArea] public string preQuizMessage = "Apakah kamu siap memulai kuis?";
    public Button startQuizButton; // opsional kalau kamu mau tombol muncul setelah teks selesai

    [Header("UI Interaksi Jarak")]
    public GameObject interactPromptPanel; // Panel "Tekan E untuk Bicara"

    [Header("Komponen Dialog & Feedback")]
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI feedbackText;
    public TextMeshProUGUI clueText;

    [Header("Panel Kuis: Pilihan Ganda")]
    public GameObject multipleChoicePanel;
    public QuizManager quizManager;
    public TextMeshProUGUI mcqTimerText;
    public List<Button> mcqAnswerButtons;

    [Header("Panel Kuis: Tebak Gambar")]
    public GameObject guessPicturePanel;
    public Image guessPictureImage;
    public TMP_InputField guessPictureInputField;
    public Button guessPictureSubmitButton;

    [Header("Pengaturan Umum")]
    public float typingSpeed = 0.04f;
    public float quizTimePerQuestion = 15f;

    // State
    public bool isInteracting { get; private set; }
    private Queue<string> sentences;
    private NPCPosController currentNPC;
    private bool isTyping = false;
    private string currentSentence = "";

    // Coroutine references (biar ga saling ganggu)
    private Coroutine typingDialogueCoroutine;
    private Coroutine typingPreQuizCoroutine;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        sentences = new Queue<string>();
    }

    // === Cursor & Prompt ===
    private void ShowCursor() { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }
    private void HideCursor() { Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false; }

    public void ShowInteractPrompt() { if (interactPromptPanel) interactPromptPanel.SetActive(true); }
    public void HideInteractPrompt() { if (interactPromptPanel) interactPromptPanel.SetActive(false); }

    // === Mulai Interaksi ===
    public void StartInteraction(NPCPosController npc)
    {
        isInteracting = true;
        currentNPC = npc;
        ShowCursor();
        StartDialogue(npc.dialogAwal);
    }

    void StartDialogue(DialogueData dialogue)
    {
        dialoguePanel.SetActive(true);
        sentences.Clear();
        foreach (string sentence in dialogue.kalimat)
            sentences.Enqueue(sentence);

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (isTyping)
        {
            // kalau user klik next pas efek ngetik → langsung tampil full
            if (typingDialogueCoroutine != null)
                StopCoroutine(typingDialogueCoroutine);

            dialogueText.text = currentSentence;
            isTyping = false;
            return;
        }

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        currentSentence = sentences.Dequeue();

        if (typingDialogueCoroutine != null)
            StopCoroutine(typingDialogueCoroutine);

        typingDialogueCoroutine = StartCoroutine(TypeSentence(currentSentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    // === Setelah Dialog Selesai ===
    void EndDialogue()
    {
        Debug.Log("EndDialogue() terpanggil!");

        dialoguePanel.SetActive(false);

        if (preQuizPanel != null)
        {
            preQuizPanel.SetActive(true);
            if (preQuizText != null)
            {
                StopAllCoroutines();
                // Pastikan efek ngetik jalan setelah panel aktif sepenuhnya
                StartCoroutine(ShowPreQuizWithTyping());
            }
        }
    }

    IEnumerator ShowPreQuizWithTyping()
    {
        // Tunggu 1 frame agar panel bener-bener aktif sebelum ngetik
        yield return null;
        yield return new WaitForSeconds(0.05f);

        preQuizText.text = "";
        foreach (char letter in preQuizMessage.ToCharArray())
        {
            preQuizText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }


    // === Tombol Mulai Kuis ===
    public void StartQuizFromButton()
    {
        if (preQuizPanel != null)
            preQuizPanel.SetActive(false);

        multipleChoicePanel.SetActive(true);
        quizManager.StartQuiz();
    }


    // === Feedback & Clue ===
    public void ShowFeedback(string message)
    {
        StartCoroutine(ShowFeedbackAndReset(message));
    }

    IEnumerator ShowFeedbackAndReset(string message)
    {
        feedbackText.text = message;
        feedbackPanel.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        feedbackPanel.SetActive(false);
        isInteracting = false;
        HideCursor();
    }

    public void ShowClue()
    {
        currentNPC?.SelesaikanPos();
        cluePanel.SetActive(true);
        clueText.text = currentNPC != null ? currentNPC.AmbilPetunjuk() : "Petunjuk tidak ditemukan.";
    }

    public void CloseCluePanel()
    {
        cluePanel.SetActive(false);
        isInteracting = false;
        HideCursor();
    }
}
