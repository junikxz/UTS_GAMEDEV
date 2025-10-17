// QuizData.cs
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Quiz", menuName = "Nusantara/Quiz Data")]
public class QuizData : ScriptableObject
{
    public List<Pertanyaan> daftarPertanyaan;
    public int hadiahKoin = 100;
    public string petunjukBerikutnya;
    public float waktuPerPertanyaan = 15f;
}