using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MemoryCardQuiz : BaseQuizLogic
{
    [Header("Data & Pengaturan")]
    public MemoryCardData gameData;

    [Header("Referensi UI")]
    public GameObject memoryCardPanel;
    public List<Button> cardButtons; // Akan kita isi dengan 20 kartu

    [Header("Visual")]
    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;

    // Variabel internal permainan
    private List<int> correctSequence = new List<int>();
    private int playerInputIndex = 0;
    private bool isPlayerTurn = false;

    public override void StartQuiz()
    {
        memoryCardPanel.SetActive(true);
        InteractionManager.instance.ShowCursor();

        // Atur semua tombol agar bisa diklik dan memanggil fungsi
        for (int i = 0; i < cardButtons.Count; i++)
        {
            int index = i; // Penting untuk menghindari masalah closure
            cardButtons[i].onClick.RemoveAllListeners();
            cardButtons[i].onClick.AddListener(() => OnCardClicked(index));
        }

        StartCoroutine(StartNewGame());
    }

    IEnumerator StartNewGame()
    {
        correctSequence.Clear();
        playerInputIndex = 0;
        yield return new WaitForSeconds(1f); // Beri jeda sebelum game dimulai
        StartNextRound();
    }

    void StartNextRound()
    {
        isPlayerTurn = false;
        // Tambahkan satu kartu acak baru ke dalam urutan
        correctSequence.Add(Random.Range(0, cardButtons.Count));
        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        // Matikan interaksi saat urutan dimainkan
        foreach (Button btn in cardButtons) btn.interactable = false;

        yield return new WaitForSeconds(0.5f);

        // Mainkan urutan kartu yang menyala
        foreach (int index in correctSequence)
        {
            cardButtons[index].GetComponent<Image>().color = highlightColor;
            yield return new WaitForSeconds(gameData.timeBetweenFlashes);
            cardButtons[index].GetComponent<Image>().color = normalColor;
            yield return new WaitForSeconds(0.2f);
        }

        // Aktifkan kembali interaksi untuk giliran pemain
        foreach (Button btn in cardButtons) btn.interactable = true;
        isPlayerTurn = true;
        playerInputIndex = 0; // Reset input pemain untuk ronde ini
    }

    void OnCardClicked(int index)
    {
        if (!isPlayerTurn) return;

        // Cek apakah pemain menekan kartu yang benar
        if (index == correctSequence[playerInputIndex])
        {
            playerInputIndex++;
            // Cek apakah pemain sudah menyelesaikan urutan di ronde ini
            if (playerInputIndex >= correctSequence.Count)
            {
                // Cek apakah pemain sudah menang
                if (correctSequence.Count >= gameData.sequenceToWin)
                {
                    memoryCardPanel.SetActive(false);
                    InteractionManager.instance.HandleQuizSuccess();
                }
                else
                {
                    // Lanjut ke ronde berikutnya
                    StartNextRound();
                }
            }
        }
        else
        {
            // Pemain salah, game gagal
            memoryCardPanel.SetActive(false);
            InteractionManager.instance.HandleQuizFailure("Urutan salah!");
        }
    }
}