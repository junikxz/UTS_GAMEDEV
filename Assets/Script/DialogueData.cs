// DialogueData.cs
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Nusantara/Dialogue Data")]
public class DialogueData : ScriptableObject { [TextArea(3, 10)] public string[] kalimat; }