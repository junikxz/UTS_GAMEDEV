using UnityEngine;
using System.Collections.Generic;

// [System.Serializable] membuat class ini bisa terlihat di Inspector Unity
[System.Serializable]
public class SongQuestion
{
    public AudioClip songClip;
    public string correctTitle;
}

[CreateAssetMenu(fileName = "New Song Quiz (Multi)", menuName = "Nusantara/Song Quiz Data (Multi-Question)")]
public class SongQuizData : ScriptableObject
{
    // DIGANTI: Sekarang kita menggunakan List (daftar) dari SongQuestion
    public List<SongQuestion> questions;
}