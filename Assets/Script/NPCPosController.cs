using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class NPCPosController : MonoBehaviour
{
    public DialogueData dialogAwal;
    public bool isPosSelesai = false;
    public GameObject tandaSelesai;

    private bool playerInRange = false;
    private bool hasInteracted = false; // setelah true, prompt tidak akan muncul lagi
    private BaseQuizLogic quizLogic;

    void Awake()
    {
        GetComponent<SphereCollider>().isTrigger = true;
        quizLogic = GetComponent<BaseQuizLogic>(); // tambahkan ini!
    }


    void Start()
    {
        if (tandaSelesai != null) tandaSelesai.SetActive(false);
    }

    void Update()
    {
        // Trigger dialog hanya jika player di range, tekan E, belum interaksi, dan tidak sedang interaksi
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !hasInteracted && !InteractionManager.instance.isInteracting)
        {
            hasInteracted = true; // KUNCI: setelah ini prompt tidak akan muncul lagi
            InteractionManager.instance.HideInteractPrompt();
            InteractionManager.instance.StartInteraction(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (isPosSelesai) return;

        playerInRange = true;

        // Tampilkan prompt hanya jika belum pernah interaksi dan tidak sedang interaksi
        if (!hasInteracted && !InteractionManager.instance.isInteracting)
        {
            InteractionManager.instance.ShowInteractPrompt();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;

        // Saat keluar, sembunyikan prompt.
        InteractionManager.instance.HideInteractPrompt();

        // NOTE: kita TIDAK mereset hasInteracted di sini.
        // Jika kamu ingin agar setelah keluar area prompt bisa muncul lagi,
        // kamu harus mereset hasInteracted di tempat yang tepat.
    }

    // --- Fungsi Kuis tetap sama ---
    void AwakeQuiz() { quizLogic = GetComponent<BaseQuizLogic>(); }

    public void JalankanKuis()
    {
        if (quizLogic == null) quizLogic = GetComponent<BaseQuizLogic>();
        quizLogic.StartQuiz();
    }

    public string AmbilPetunjuk()
    {
        if (quizLogic == null) quizLogic = GetComponent<BaseQuizLogic>();
        if (quizLogic is MultipleChoiceQuiz mcq) return mcq.quizData.petunjukBerikutnya;
        if (quizLogic is GuessPictureQuiz gpq) return gpq.quizData.petunjukBerikutnya;
        return "Petunjuk tidak ditemukan.";
    }

    public void SelesaikanPos()
    {
        isPosSelesai = true;
        if (tandaSelesai != null) tandaSelesai.SetActive(true);
        // Jika kamu mau mengizinkan prompt muncul lagi setelah menyelesaikan pos,
        // tambahkan: hasInteracted = false;  di sini.
    }
}
