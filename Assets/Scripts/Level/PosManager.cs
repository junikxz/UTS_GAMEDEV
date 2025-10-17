using UnityEngine;

public class PosManager : MonoBehaviour
{
    public static PosManager instance;

    [Header("Daftar NPC Pos (Urut Sesuai Level)")]
    public GameObject[] npcPosList;
    public int CurrentPosIndex => currentPosIndex;


    private int currentPosIndex = 0;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        // Matikan semua pos dulu
        foreach (GameObject npc in npcPosList)
            npc.SetActive(false);

        // Hanya pos pertama yang aktif di awal
        if (npcPosList.Length > 0)
            npcPosList[0].SetActive(true);
    }

    // Dipanggil saat satu pos selesai
    public void UnlockNextPos()
    {
        // Matikan pos sebelumnya jika ada
        if (currentPosIndex < npcPosList.Length && currentPosIndex >= 0)
        {
            npcPosList[currentPosIndex].SetActive(false);
            Debug.Log($"[PosManager] Pos {currentPosIndex + 1} dimatikan.");
        }

        // Naikkan index ke pos berikutnya
        currentPosIndex++;

        Debug.Log($"[PosManager] UnlockNextPos() terpanggil, index sekarang: {currentPosIndex}");

        // Aktifkan pos baru jika masih ada
        if (currentPosIndex < npcPosList.Length)
        {
            npcPosList[currentPosIndex].SetActive(true);
            Debug.Log($"[PosManager] Pos {currentPosIndex + 1} sekarang aktif!");
        }
        else
        {
            Debug.Log("[PosManager] Semua pos selesai!");
        }
    }


}
