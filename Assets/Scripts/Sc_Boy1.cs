using UnityEngine;

// Atribut ini memastikan objek selalu punya komponen yang dibutuhkan
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class Sc_Boy1 : MonoBehaviour
{
    // Pengaturan bisa diubah di Inspector
    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    public float turnSmoothTime = 0.1f;

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
        // 1. DAPATKAN INPUT
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        bool isMoving = direction.magnitude >= 0.1f;
        bool shiftHeld = Input.GetKey(KeyCode.LeftShift);
        bool isSprinting = isMoving && shiftHeld; // langsung sprint jika shift ditekan

        // 2. GERAK & ROTASI
        if (isMoving)
        {
            // Hitung arah berdasarkan kamera
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Pilih kecepatan
            float currentSpeed = isSprinting ? runSpeed : walkSpeed;

            // Gerakkan karakter
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);

            // 3. ANIMASI
            anim.SetBool("isRun", isSprinting);
            anim.SetBool("isJalan", !isSprinting);
        }
        else
        {
            // Diam
            anim.SetBool("isRun", false);
            anim.SetBool("isJalan", false);
        }
    }
}
