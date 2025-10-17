using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager instance;

    [Header("Panel Umum")]
    public GameObject dialoguePanel;
    public GameObject preQuizPanel;
    public GameObject interactPromptPanel;
    public GameObject feedbackPanel;

    public GameObject victoryPanel;

    [Header("UI Komponen")]
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI preQuizText;
    public TextMeshProUGUI feedbackText;
    [TextArea] public string preQuizMessage = "Apakah kamu siap?";

    [Header("Pengaturan")]
    public float typingSpeed = 0.04f;
    public int coinsPerQuiz = 100;

    // State
    public bool isInteracting { get; private set; }
    private NPCPosController currentNPC;
    private Queue<string> sentences;
    private bool wasQuizSuccessful; // ✨ VARIABEL BARU untuk mengingat hasil kuis

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        sentences = new Queue<string>();
    }

    // === Sistem Penanganan Hasil Kuis (Diperbaiki) ===
    public void HandleQuizSuccess()
    {
        if (currentNPC == null) return;
        wasQuizSuccessful = true; // Ingat bahwa kuis berhasil

        // --- Logika Penambahan Koin ---
        int currentCoins = PlayerPrefs.GetInt("coins", 0);
        currentCoins += coinsPerQuiz;
        PlayerPrefs.SetInt("coins", currentCoins);
        PlayerPrefs.Save();
        CoinDisplayManager.instance.RefreshCoins();

        // Siapkan pesan dan tampilkan panel
        string successMessage = $"Selamat!\nKamu telah berhasil menyelesaikan Pos ini!\nSilahkan pergi ke pos selanjutnya untuk menyelesaikan tantangan selanjutnya!\n\n+{coinsPerQuiz} Koin ditambahkan.";
        feedbackText.text = successMessage;
        feedbackPanel.SetActive(true);
        GameManager.instance.TambahKoin(coinsPerQuiz);
        ShowCursor(); // Tampilkan cursor agar bisa klik tombol
    }

    public void HandleQuizFailure(string reason)
    {
        if (currentNPC == null) return;

        Debug.Log("Kuis Gagal: " + reason + ". Kembali ke Pre-Quiz Panel.");

        // Tampilkan cursor agar bisa klik tombol di preQuizPanel
        ShowCursor();

        // Aktifkan kembali preQuizPanel
        preQuizPanel.SetActive(true);

        // Pastikan isInteracting tetap true agar pemain tidak bisa bergerak
        isInteracting = true;
    }

    // ✨ FUNGSI BARU: Ini akan dipanggil oleh tombol "Tutup" Anda
    public void CloseFeedbackPanel()
    {
        feedbackPanel.SetActive(false);
        isInteracting = false;
        HideCursor();

        // Jalankan logika lanjutan setelah panel ditutup
        if (wasQuizSuccessful)
        {
            // Jika kuis berhasil, selesaikan pos
            if (currentNPC != null)
            {
                currentNPC.SelesaikanPos();
                currentNPC = null;
            }
        }
        else
        {
            // Jika kuis gagal, reset interaksi
            if (currentNPC != null)
            {
                currentNPC.ResetInteraction();
            }
        }
    }

    public void LoadIntroduction()
    {
        SceneManager.LoadScene("Introduction");
    }

    // DIHAPUS: Coroutine ShowFeedbackAndProceed dan ShowFeedbackAndReset yang lama

    // ... (Sisa kode untuk dialog, prompt, dll. tidak perlu diubah) ...
    #region Logika Dialog dan Interaksi Lainnya
    public void ShowCursor() { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }
    private void HideCursor() { Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false; }
    public void ShowInteractPrompt() { if (interactPromptPanel) interactPromptPanel.SetActive(true); }
    public void HideInteractPrompt() { if (interactPromptPanel) interactPromptPanel.SetActive(false); }

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
        foreach (string sentence in dialogue.kalimat) sentences.Enqueue(sentence);
        DisplayNextSentence();
    }

    public void OnClickStartQuiz()
    {
        preQuizPanel.SetActive(false);
        isInteracting = false;
        HideCursor();

        if (currentNPC != null)
        {
            currentNPC.JalankanKuis();
        }
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0) { EndDialogue(); return; }
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentences.Dequeue()));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    // Ganti fungsi EndDialogue() yang lama dengan yang ini
    void EndDialogue()
    {
        // Sembunyikan panel dialog
        dialoguePanel.SetActive(false);

        // --- LOGIKA BARU ---
        // Cek "saklar" di NPC yang sedang aktif
        if (currentNPC != null && currentNPC.skipPreQuizPanel)
        {
            // Jika saklar aktif, lewati PreQuizPanel dan langsung jalankan "kuis" (pemberian hadiah)
            currentNPC.JalankanKuis();
        }
        else
        {
            // Jika saklar tidak aktif (untuk Pos 1-4), tampilkan PreQuizPanel seperti biasa
            if (preQuizPanel != null)
            {
                preQuizPanel.SetActive(true);
            }
            else
            {
                // Cadangan jika PreQuizPanel lupa dihubungkan
                currentNPC.JalankanKuis();
            }
        }
    }

    public void CloseVictoryPanel()
    {
        // 1. Sembunyikan panel kemenangan
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }

        // 2. Tandai bahwa interaksi sudah selesai
        isInteracting = false;

        // 3. Sembunyikan cursor dan kembalikan kontrol ke pemain
        HideCursor();

        // Tidak ada lagi yang perlu dilakukan. 
        // Kita tidak perlu mereset NPC karena permainan sudah selesai.
    }

    public void CancelQuiz()
    {
        preQuizPanel.SetActive(false);
        isInteracting = false;
        HideCursor();
        if (currentNPC != null)
        {
            currentNPC.ResetInteraction();
            currentNPC = null;
        }
    }
    #endregion
}