// BaseQuizLogic.cs (VERSI FINAL YANG BENAR)
using UnityEngine;

public abstract class BaseQuizLogic : MonoBehaviour
{
    // Setiap script kuis WAJIB memiliki fungsi ini
    public abstract void StartQuiz();

    // Fungsi ini dipanggil dari script kuis jika BERHASIL
    public virtual void OnQuizSuccess()
    {
        // Panggil handler sukses di InteractionManager
        InteractionManager.instance.HandleQuizSuccess();
    }

    // Fungsi ini dipanggil dari script kuis jika GAGAL
    public virtual void OnQuizFailed(string reason)
    {
        // Panggil handler gagal di InteractionManager
        InteractionManager.instance.HandleQuizFailure(reason);
    }
}