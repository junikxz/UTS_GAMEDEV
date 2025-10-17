    using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(BaseQuizLogic))] // Wajibkan setiap NPC punya script kuis
public class NPCPosController : MonoBehaviour
{
    [Header("Data & Status")]
    public DialogueData dialogAwal;
    public bool isPosSelesai = false;
    public GameObject tandaSelesai;

    [Header("Pengaturan Animasi & Rotasi")]
    public float lookSpeed = 5f; // Kecepatan NPC menengok

    [Header("Pengaturan Khusus")] // <-- HEADER BARU
    public bool skipPreQuizPanel = false; // <-- VARIABEL BARU (SAKLAR)

    // Variabel internal
    private bool playerInRange = false;
    private bool hasInteracted = false;
    private BaseQuizLogic quizLogic;
    private Animator anim;
    private Transform player;
    private bool isTalking = false;

    void Awake()
    {
        GetComponent<SphereCollider>().isTrigger = true;
        // ✨ DIGABUNGKAN: Mengambil semua komponen yang diperlukan
        quizLogic = GetComponent<BaseQuizLogic>();
        anim = GetComponent<Animator>(); 
    }

    void Start()
    {
        if (tandaSelesai != null) tandaSelesai.SetActive(false);
    }

    void Update()
    {
        // ✨ DARI 'main': NPC akan selalu menengok ke arah player jika di dalam jangkauan
        if (playerInRange && player != null && !isPosSelesai)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0; // Pastikan NPC tidak miring ke atas/bawah
            if (direction.magnitude > 0.1f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookSpeed);
            }
        }

        // Logika untuk memulai interaksi saat 'E' ditekan
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !hasInteracted && !InteractionManager.instance.isInteracting)
        {
            hasInteracted = true;
            InteractionManager.instance.HideInteractPrompt();
            InteractionManager.instance.StartInteraction(this);

            // ✨ DARI 'main': Aktifkan animasi bicara saat dialog dimulai
            isTalking = true;
            if (anim != null) anim.SetBool("isTalk", true);
        }

        // ✨ DARI 'main': Hentikan animasi bicara setelah dialog/kuis selesai
        if (isTalking && !InteractionManager.instance.isInteracting)
        {
            isTalking = false;
            if (anim != null) anim.SetBool("isTalk", false);
        }
    }

    // Dipanggil oleh InteractionManager untuk memulai kuis
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
        PosManager.instance.UnlockNextPos(); // Pastikan nama manager Anda benar
    }

    // Dipanggil oleh InteractionManager jika kuis gagal/dibatalkan
    public void ResetInteraction()
    {
        hasInteracted = false;
        if (playerInRange && !isPosSelesai) InteractionManager.instance.ShowInteractPrompt();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // ✨ DIGABUNGKAN: Mengambil guard clause dari 'livi_baru'
        if (!other.CompareTag("Player") || isPosSelesai) return;

        // ✨ DIGABUNGKAN: Mengambil logika dari 'main'
        playerInRange = true;
        player = other.transform; // Simpan transform player untuk rotasi

        if (!hasInteracted && !InteractionManager.instance.isInteracting)
        {
            InteractionManager.instance.ShowInteractPrompt();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // ✨ DIGABUNGKAN: Mengambil logika pembersihan dari 'main'
        playerInRange = false;
        player = null; // Hapus referensi player
        
        // Hentikan animasi bicara jika pemain menjauh saat belum berinteraksi
        if (anim != null && !isTalking)
        {
            anim.SetBool("isTalk", false);
        }
        
        InteractionManager.instance.HideInteractPrompt();
    }
}