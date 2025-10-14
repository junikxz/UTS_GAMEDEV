using UnityEngine;

public class Sc_camera : MonoBehaviour
{
    public Transform target; // Target yang akan diikuti (Pemain)
    public Vector3 offset = new Vector3(2f, 2f, 2f); // Offset dari target
    public float smoothSpeed = 0.125f; // Kecepatan smooth (lebih kecil = lebih smooth)

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Target kamera belum diatur!");
            return;
        }

        // Tentukan posisi yang diinginkan
        Vector3 desiredPosition = target.position + offset;

        // Interpolasi posisi kamera ke posisi yang diinginkan
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;

        // Kamera menghadap ke target
        transform.LookAt(target);
    }
}