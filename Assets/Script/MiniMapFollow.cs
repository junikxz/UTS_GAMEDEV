using UnityEngine;

public class MiniMapFollow : MonoBehaviour
{
    public Transform player;
    public float height = 20f;

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 newPos = player.position;
        newPos.y += height;
        transform.position = newPos;

        // Rotasi mengikuti arah player (biar mini map rotasinya sinkron)
        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
    }
}
