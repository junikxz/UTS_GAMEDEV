using UnityEngine;

[CreateAssetMenu(fileName = "New Guess Picture", menuName = "Nusantara/Guess Picture Data")]
public class GuessPictureData : ScriptableObject
{
    public Sprite gambarTebakan;
    public string jawabanBenar;
    public int hadiahKoin = 150;
    public string petunjukBerikutnya;
}