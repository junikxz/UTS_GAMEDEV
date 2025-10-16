using UnityEngine;

[CreateAssetMenu(fileName = "New Memory Card Game", menuName = "Nusantara/Memory Card Data")]
public class MemoryCardData : ScriptableObject
{
    public int sequenceToWin = 10; // Harus mengulang 10 urutan untuk menang
    public float timeBetweenFlashes = 0.5f; // Jeda antar kartu yang menyala
}