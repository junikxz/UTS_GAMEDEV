using UnityEngine;

[CreateAssetMenu(fileName = "New Conversation", menuName = "Dialogue/Conversation")]
public class Conversation : ScriptableObject
{
    [Header("Dialogue Content")]
    public DialogueLine[] dialogueLines;

    [Header("Settings")]
    public bool freezePlayerMovement = true;
}
