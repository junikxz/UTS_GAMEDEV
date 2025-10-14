using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class NPCPosController : MonoBehaviour
{
    public DialogueData dialogAwal;
    public bool isPosSelesai = false;
    public GameObject tandaSelesai;
    public int posIndex;

    private bool playerInRange = false;
    private bool hasInteracted = false;
    private BaseQuizLogic quizLogic;

    // Tambahan animasi dan rotasi
    private Animator anim;
    private Transform player;
    public float lookSpeed = 5f; // kecepatan menengok
    private bool isTalking = false;

    void Awake()
    {
        GetComponent<SphereCollider>().isTrigger = true;
        quizLogic = GetComponent<BaseQuizLogic>();
        anim = GetComponent<Animator>(); // ambil komponen animator
    }

    void Start()
    {
        if (tandaSelesai != null) tandaSelesai.SetActive(false);
    }

    void Update()
    {
        // Jika player ada di area dan belum selesai
        if (playerInRange && player != null && !isPosSelesai)
        {
            // NPC menengok ke arah player
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0; // jangan miring ke atas/bawah
            if (direction.magnitude > 0.1f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookSpeed);
            }
        }

        // Jika player di range dan tekan E untuk mulai interaksi
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !hasInteracted && !InteractionManager.instance.isInteracting)
        {
            hasInteracted = true;
            InteractionManager.instance.HideInteractPrompt();
            InteractionManager.instance.StartInteraction(this);

            // Aktifkan animasi bicara
            if (anim != null)
            {
                isTalking = true;
                anim.SetBool("isTalk", true);
            }
        }

        // Jika sedang bicara dan interaksi selesai
        if (isTalking && !InteractionManager.instance.isInteracting)
        {
            isTalking = false;
            if (anim != null)
            {
                anim.SetBool("isTalk", false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (isPosSelesai) return;

        playerInRange = true;
        player = other.transform;

        // Aktifkan animasi bicara ringan (misal sapaan)
        if (anim != null)
        {
            anim.SetBool("isTalk", true);
        }

        if (!hasInteracted && !InteractionManager.instance.isInteracting)
        {
            InteractionManager.instance.ShowInteractPrompt();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;

        // Matikan animasi bicara saat player menjauh
        if (anim != null)
        {
            anim.SetBool("isTalk", false);
        }

        player = null;
        InteractionManager.instance.HideInteractPrompt();
    }

    // --- Fungsi Kuis tetap sama ---
    public void JalankanKuis()
    {
        if (quizLogic == null) quizLogic = GetComponent<BaseQuizLogic>();
        quizLogic.StartQuiz();
    }

    public string AmbilPetunjuk()
    {
        if (quizLogic == null) quizLogic = GetComponent<BaseQuizLogic>();
        if (quizLogic is MultipleChoiceQuiz mcq) return mcq.quizData.petunjukBerikutnya;
        return "Petunjuk tidak ditemukan.";
    }

    public void SelesaikanPos()
    {
        isPosSelesai = true;
        if (tandaSelesai != null) tandaSelesai.SetActive(true);
        Debug.Log("Pos selesai: " + gameObject.name);

        PosManager.instance.UnlockNextPos();
    }

    public void ResetInteraction()
    {
        hasInteracted = false;

        if (playerInRange && !isPosSelesai)
        {
            InteractionManager.instance.ShowInteractPrompt();
        }
    }
}
