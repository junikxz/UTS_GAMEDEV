using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NPC_Pos4_FlagGiver : MonoBehaviour
{
    [Header("NPC Settings")]
    public string NpcName = "NPC";
    public string CountryFlag = "Indonesia";
    public bool DeductCoinsOnReward = true;
    public int CoinsToDeduct = 400;

    [Header("UI References - EXISTING PANELS")]
    public GameObject DialogPanel;
    public TMP_Text DialogueText;
    public GameObject InteractPrompt;
    public TMP_Text PromptText;

    [Header("Reward Panel References")]
    public GameObject RewardPanel;
    public TMP_Text RewardMessageText;
    public Image RewardFlagImage;

    [Header("Interaction Settings")]
    public KeyCode InteractKey = KeyCode.E;
    public float InteractionRange = 3f;

    [Header("Visual Feedback")]
    public GameObject ExclamationMark;
    public Color PromptColorNormal = Color.white;
    public Color PromptColorReady = Color.green;

    [Header("Animation Settings")]
    public bool UseNPCAnimation = true;
    public string GreetAnimationTrigger = "Talk";
    public string RewardAnimationTrigger = "GiveReward";

    private Animator anim;
    private Transform player;
    private bool playerInRange;

    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // Validasi manager
        if (CoinDisplayManager.instance == null)
            Debug.LogError("❌ CoinDisplayManager not found!");
        if (FlagInventoryManager.instance == null)
            Debug.LogError("❌ FlagInventoryManager not found!");

        Debug.Log($"✅ NPC {NpcName} initialized.");
    }

    void Update()
    {
        // if (player
    }
}