using UnityEngine;

// abstract class tidak bisa ditempel langsung ke objek, hanya untuk diwarisi
public abstract class BaseQuizLogic : MonoBehaviour
{
    // Setiap kuis harus punya fungsi untuk memulai
    public abstract void StartQuiz();

    // Fungsi ini dipanggil dari luar (InteractionManager) jika kuis berhasil
    public virtual void OnQuizSuccess()
    {
        Debug.Log("Kuis Berhasil!");
        InteractionManager.instance.ShowClue(); // Tampilkan petunjuk
    }

    // Fungsi ini dipanggil jika kuis gagal
    public virtual void OnQuizFailed(string reason)
    {
        Debug.Log("Kuis Gagal: " + reason);
        InteractionManager.instance.ShowFeedback(reason); // Tampilkan feedback
    }
}