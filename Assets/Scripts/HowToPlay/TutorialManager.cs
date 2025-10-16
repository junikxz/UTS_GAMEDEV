using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }
    public Conversation welcomeMessage;
    public Conversation objectiveMessage;
    public Conversation completionMessage;

    private bool hasMoved = false;

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
    }

    void Start()
    {
        ShowWelcomeMessage();
    }

    void Update()
    {
        // This part is for advancing from Welcome -> Objective
        if (!hasMoved && Input.GetKeyDown(KeyCode.E))
        {
            hasMoved = true; // Set the flag so this block only runs once
            ShowObjectiveMessage();
        }
    }

    public void ShowWelcomeMessage()
    {
        DialogueManager.Instance.StartDialogue(welcomeMessage);
    }

    public void ShowObjectiveMessage()
    {
        Invoke(nameof(StartObjectiveDialogue), 0.5f);
    }

    private void StartObjectiveDialogue()
    {
        // This method's only job is now to show the objective message.
        DialogueManager.Instance.StartDialogue(objectiveMessage);
    }

    // This will be called by our ObjectiveTrigger script when the player arrives.
    public void PlayerReachedObjective()
    {
        Debug.Log("TutorialManager notified that player reached objective!");

        // End the "Objective" dialogue that is currently on screen
        DialogueManager.Instance.EndDialogue();

        // Show the final completion message
        Invoke(nameof(StartCompletionDialogue), 0.5f);
    }

    private void StartCompletionDialogue()
    {
        DialogueManager.OnDialogueEnd += LoadIntroduction; // load leve1 after tutorial finished
        DialogueManager.Instance.StartDialogue(completionMessage);
    }

    private void LoadIntroduction()
    {
        Debug.Log("Completion dialogue finished. Loading Level 1...");

        DialogueManager.OnDialogueEnd -= LoadIntroduction;

        SceneManager.LoadScene("Introduction");
    }
}