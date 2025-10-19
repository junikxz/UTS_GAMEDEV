using UnityEngine;

public class Sc_Camera : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;
    public Vector3 offset = new Vector3(0, 2f, 0); // Offset dari target (ketinggian shoulder)

    [Header("Camera Settings")]
    public float distance = 5.0f;
    public float minDistance = 2.0f;
    public float maxDistance = 10.0f;
    public float mouseSensitivity = 4.0f;
    public float scrollSensitivity = 2.0f;

    [Header("Rotation Settings")]
    public float rotationSmoothTime = 0.1f;
    public float minPitch = -20f;
    public float maxPitch = 60f;

    [Header("Collision Settings")]
    public LayerMask collisionLayers;
    public float collisionOffset = 0.3f;

    private float yaw = 0.0f;
    private float pitch = 20.0f;
    private float currentYaw;
    private float currentPitch;
    private float yawVelocity;
    private float pitchVelocity;
    private float currentDistance;
    private float distanceVelocity;

    void Start()
    {
        if (target != null)
        {
            yaw = target.eulerAngles.y;
            currentYaw = yaw;
            currentPitch = pitch;
            currentDistance = distance;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Toggle cursor lock dengan tombol Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ?
                CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = !Cursor.visible;
        }

        if (Sc_pauseManager.Instance.isPaused)
        {
            return; // Langsung keluar dari fungsi LateUpdate
        }

        HandleInput();
        UpdateCameraPosition();
    }

    void HandleInput()
    {
        // Mouse input untuk rotasi kamera
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // Scroll wheel untuk zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance -= scroll * scrollSensitivity;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        // Smooth rotation
        currentYaw = Mathf.SmoothDampAngle(currentYaw, yaw, ref yawVelocity, rotationSmoothTime);
        currentPitch = Mathf.SmoothDampAngle(currentPitch, pitch, ref pitchVelocity, rotationSmoothTime);
        currentDistance = Mathf.SmoothDamp(currentDistance, distance, ref distanceVelocity, rotationSmoothTime);
    }

    void UpdateCameraPosition()
    {
        // Hitung posisi target dengan offset
        Vector3 targetPosition = target.position + offset;

        // Hitung rotasi kamera
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);

        // Hitung posisi kamera yang diinginkan
        Vector3 desiredPosition = targetPosition - (rotation * Vector3.forward * currentDistance);

        // Collision detection (opsional, uncomment jika ingin collision)
        // desiredPosition = HandleCameraCollision(targetPosition, desiredPosition);

        // Set posisi dan rotasi kamera
        transform.position = desiredPosition;
        transform.LookAt(targetPosition);
    }

    // Fungsi untuk mencegah kamera menembus dinding (opsional)
    Vector3 HandleCameraCollision(Vector3 targetPos, Vector3 desiredPos)
    {
        Vector3 direction = desiredPos - targetPos;
        float targetDistance = direction.magnitude;

        RaycastHit hit;
        if (Physics.Raycast(targetPos, direction.normalized, out hit, targetDistance, collisionLayers))
        {
            return hit.point + direction.normalized * collisionOffset;
        }

        return desiredPos;
    }

    // Fungsi untuk mendapatkan arah forward kamera (berguna untuk kontrol karakter)
    public Vector3 GetCameraForward()
    {
        Vector3 forward = transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    public Vector3 GetCameraRight()
    {
        Vector3 right = transform.right;
        right.y = 0;
        return right.normalized;
    }

    public float GetYaw()
    {
        return currentYaw;
    }
}