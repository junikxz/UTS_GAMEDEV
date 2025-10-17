using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    [Header("Dialogue Assets")]
    public Conversation welcomeMessage;
    public Conversation objectiveMessage;
    public Conversation completionMessage;

    [Header("Object References")]
    public Transform playerTransform;
    public Transform destinationPoint;
    public Sc_Camera mainCameraController;

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
        // DialogueManager.OnDialogueEnd += LoadIntroduction; // load leve1 after tutorial finished
        // DialogueManager.Instance.StartDialogue(completionMessage);

        DialogueManager.OnDialogueEnd += MovePlayerToDestination; // <-- CHANGE THIS
        DialogueManager.Instance.StartDialogue(completionMessage);
    }

    // private void LoadIntroduction()
    // {
    //     Debug.Log("Completion dialogue finished. Loading Level 1...");

    //     DialogueManager.OnDialogueEnd -= LoadIntroduction;

    //     SceneManager.LoadScene("Introduction");
    // }

    private void MovePlayerToDestination()
    {
        Debug.Log("Completion dialogue finished. Moving player to destination.");

        // IMPORTANT: Unsubscribe from the event to prevent this from running again.
        DialogueManager.OnDialogueEnd -= MovePlayerToDestination;

        // Check if the references are set to avoid errors
        if (playerTransform != null && destinationPoint != null)
        {
            // Instantly move the player to the destination's position and rotation
            playerTransform.position = destinationPoint.position;
            playerTransform.rotation = destinationPoint.rotation;
        }
        else
        {
            Debug.LogWarning("Player Transform or Destination Point is not set in the TutorialManager Inspector!");
        }
    }
}