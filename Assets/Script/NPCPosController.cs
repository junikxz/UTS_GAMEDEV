// NPCPosController.cs (VERSI BARU)
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(BaseQuizLogic))] // Wajibkan setiap NPC punya script kuis
public class NPCPosController : MonoBehaviour
{
    public DialogueData dialogAwal;
    public bool isPosSelesai = false;
    public GameObject tandaSelesai;

    private bool playerInRange = false;
    private bool hasInteracted = false;
    private BaseQuizLogic quizLogic;

    void Awake()
    {
        GetComponent<SphereCollider>().isTrigger = true;
        // Otomatis mencari script kuis apa pun yang terpasang
        quizLogic = GetComponent<BaseQuizLogic>();
    }

    void Start()
    {
        if (tandaSelesai != null) tandaSelesai.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !hasInteracted && !InteractionManager.instance.isInteracting)
        {
            hasInteracted = true;
            InteractionManager.instance.HideInteractPrompt();
            InteractionManager.instance.StartInteraction(this);
        }
    }

    // Dipanggil oleh InteractionManager
    public void JalankanKuis()
    {
        if (quizLogic != null)
        {
            quizLogic.StartQuiz();
        }
        else
        {
            Debug.LogError($"Tidak ada script kuis (turunan BaseQuizLogic) pada {gameObject.name}!");
        }
    }
    
    // Dipanggil oleh InteractionManager setelah kuis berhasil
    public void SelesaikanPos()
    {
        if (isPosSelesai) return;

        isPosSelesai = true;
        if (tandaSelesai != null) tandaSelesai.SetActive(true);
        Debug.Log("Pos selesai: " + gameObject.name);
        
        // Memberi tahu PosManager untuk memunculkan NPC berikutnya
        PosManager.instance.UnlockNextPos();
    }

    #region Interaksi dan Trigger
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || isPosSelesai) return;
        playerInRange = true;
        if (!hasInteracted && !InteractionManager.instance.isInteracting)
        {
            InteractionManager.instance.ShowInteractPrompt();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = false;
        InteractionManager.instance.HideInteractPrompt();
    }

    public void ResetInteraction()
    {
        hasInteracted = false;
        if (playerInRange && !isPosSelesai) InteractionManager.instance.ShowInteractPrompt();
    }
    #endregion
}