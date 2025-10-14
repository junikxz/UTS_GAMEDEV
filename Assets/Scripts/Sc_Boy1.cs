using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class Sc_Boy1 : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 1f;
    public float runSpeed = 7f;
    public float rotationSpeed = 10f; // Kecepatan rotasi karakter
    public float gravity = -9.81f;
    public float jumpForce = 3f;

    [Header("References")]
    public Sc_Camera cameraScript; // Reference ke script kamera

    private CharacterController controller;
    private Animator anim;
    private Vector3 velocity;
    private Transform cameraTransform;

    // Attack
    private float comboAtk = 0f;
    private float lastAtk = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        // Dapatkan reference kamera
        if (cameraScript == null)
            cameraScript = FindObjectOfType<Sc_Camera>();

        if (cameraScript != null)
            cameraTransform = cameraScript.transform;
        else
            cameraTransform = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {

        // Input WASD
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 inputDirection = new Vector3(h, 0f, v).normalized;

        bool isMoving = inputDirection.magnitude >= 0.1f;
        bool isRunning = isMoving && Input.GetKey(KeyCode.LeftShift) ;

        if (isMoving)
        {
            // Hitung arah gerakan berdasarkan kamera
            Vector3 cameraForward = cameraTransform.forward;
            Vector3 cameraRight = cameraTransform.right;

            // Hilangkan komponen Y
            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();

            // Hitung arah gerakan relatif terhadap kamera
            Vector3 moveDirection = (cameraForward * inputDirection.z + cameraRight * inputDirection.x).normalized;

            // Rotasi karakter menghadap arah gerakan
            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            // Gerakkan karakter
            float speed = isRunning ? runSpeed : walkSpeed;
            controller.Move(moveDirection * speed * Time.deltaTime);

            // Update animasi
            anim.SetBool("isJalan", !isRunning);
            anim.SetBool("isRun", isRunning);
        }
        else
        {
            anim.SetBool("isJalan", false);
            anim.SetBool("isRun", false);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}