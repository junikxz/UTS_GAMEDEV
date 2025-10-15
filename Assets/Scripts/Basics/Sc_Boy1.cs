using UnityEngine;

// Atribut ini memastikan objek selalu punya komponen yang dibutuhkan
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class Sc_Boy1 : MonoBehaviour
{
    // Pengaturan bisa diubah di Inspector
    public float speed = 5f;
    public float turnSmoothTime = 0.1f;
    private bool canMove = true;
    private Vector3 direction;

    // Komponen yang dibutuhkan
    private Animator anim;
    private CharacterController controller;
    private Transform cam;

    // Variabel internal untuk rotasi halus
    private float turnSmoothVelocity;

    void Start()
    {
        // Ambil semua komponen saat game dimulai
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        cam = Camera.main.transform;

        // Kunci kursor di tengah layar
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // cek apakah karakter bisa bergerak/tidak (jika sedang ada dialogue, karakter tidak bisa bergerak)
        if (canMove)
        {
            // 1. DAPATKAN INPUT
            // Menggunakan GetAxisRaw agar lebih responsif
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            direction = new Vector3(horizontal, 0f, vertical).normalized;
            // Cek apakah ada input gerakan (joystick/tombol ditekan)
            if (direction.magnitude >= 0.1f)
            {
                // 2. HITUNG ARAH & ROTASI
                // Menghitung sudut tujuan berdasarkan arah input dan arah kamera
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

                // Membuat rotasi menjadi halus agar tidak patah-patah
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

                // Terapkan rotasi ke karakter
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                // 3. GERAKKAN KARAKTER
                // Buat vektor gerakan berdasarkan arah yang sudah dihitung (relatif terhadap kamera)
                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                // Gunakan CharacterController.Move() untuk menggerakkan karakter.
                // Ini adalah cara yang benar karena menangani collision/tabrakan.
                controller.Move(moveDir.normalized * speed * Time.deltaTime);

                // 4. SET ANIMASI
                // Jika ada gerakan, set parameter "isJalan" menjadi true
                anim.SetBool("isJalan", true);
            }
            else
            {
                // Jika tidak ada gerakan, set parameter "isJalan" menjadi false
                anim.SetBool("isJalan", false);
            }
        }
    }

    private void OnEnable()
    {
        DialogueManager.OnDialogueStart += HandleDialogueStart;
        DialogueManager.OnDialogueEnd += UnfreezeMovement;
    }

    private void OnDisable()
    {
        DialogueManager.OnDialogueStart -= HandleDialogueStart;
        DialogueManager.OnDialogueEnd -= UnfreezeMovement;
    }

    private void HandleDialogueStart(Conversation conversation)
    {
        if (conversation.freezePlayerMovement)
        {

            canMove = false;
        }
    }

    private void UnfreezeMovement()
    {
        canMove = true;
    }
}