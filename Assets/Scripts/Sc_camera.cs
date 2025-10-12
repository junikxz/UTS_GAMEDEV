using UnityEngine;

public class Sc_camera : MonoBehaviour
{
    public Transform target; // Target yang akan diikuti (Pemain)
    public Vector3 offset = new Vector3(0f, 5f, -7f); // Jarak kamera dari pemain (X, Atas, Belakang)
    public float smoothSpeed = 10f; // Kecepatan kamera mengikuti pemain

    // LateUpdate dipanggil setelah semua Update selesai.
    // Ini pilihan terbaik untuk kamera agar tidak patah-patah saat mengikuti.
    void LateUpdate()
    {
        // Pastikan ada target yang di-set
        if (target == null)
        {
            Debug.LogWarning("Target kamera belum diatur!");
            return;
        }

        // Tentukan posisi yang diinginkan kamera
        Vector3 desiredPosition = target.position + offset;

        // Gerakkan kamera secara halus dari posisi saat ini ke posisi yang diinginkan
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        // Pastikan kamera selalu melihat ke arah pemain
        transform.LookAt(target);
    }
}