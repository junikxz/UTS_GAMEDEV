using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }
    public static event Action<Conversation> OnDialogueStart;
    public static event Action OnDialogueEnd;
    public GameObject dialogeBoxPanel;
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueText;
    private Queue<DialogueLine> lines;
    private bool isDialogueActive = false;
    private Coroutine typingCoroutine;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        lines = new Queue<DialogueLine>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogeBoxPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.E))
        {
            DisplayNextLine();
        }
    }

    public void StartDialogue(Conversation conversation)
    {
        isDialogueActive = true;
        OnDialogueStart?.Invoke(conversation);

        dialogeBoxPanel.SetActive(true);
        lines.Clear();

        foreach (DialogueLine line in conversation.dialogueLines)
        {
            lines.Enqueue(line);
        }

        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine currentLine = lines.Dequeue();

        speakerNameText.text = currentLine.speakerName;
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeSentence(currentLine.sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.01f); // Adjust typing speed here
        }
    }

    public void EndDialogue()
    {
        OnDialogueEnd?.Invoke();
        isDialogueActive = false;

        dialogeBoxPanel.SetActive(false);
        Debug.Log("End of conversation");
    }
}
