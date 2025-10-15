using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Conversation conversation;

    void OnMouseDown()
    {
        TriggerDialogue();
    }

    void TriggerDialogue()
    {
        DialogueManager.Instance.StartDialogue(conversation);
    }
}
