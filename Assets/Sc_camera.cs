using UnityEngine;

public class Sc_camera : MonoBehaviour
{
    public Transform target;
    public float distance = 5.0f;
    public float mouseSensitivity = 4.0f;
    public float smoothTime = 0.2f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    private Vector3 currentRotation;
    private Vector3 smoothVelocity = Vector3.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (target != null)
        {
            yaw = target.eulerAngles.y;
            pitch = 15.0f;
        }
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;

        pitch = Mathf.Clamp(pitch, -30f, 60f);

        Vector3 targetRotation = new Vector3(pitch, yaw);
        currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref smoothVelocity, smoothTime);

        transform.eulerAngles = currentRotation;

        transform.position = target.position - transform.forward * distance;
    }
}
