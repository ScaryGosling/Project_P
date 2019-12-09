//Author: Emil Dahl
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// To use this class, please set panels gameObject to active and call the "InitText" function.
/// Enter desired type to control which of text you wish to display. Change the current dialogue/conversation by using the "IndexProp". 
/// </summary>
public class Dialogue : MonoBehaviour
{
    [SerializeField] float timePerQuestion = 10f;
    public enum DialogueType { EXPOSITION, TUTORIAL, QUEST }
    private DialogueType type = DialogueType.QUEST;
    public DialogueType DialogueProp { get { return type; } set { type = value; } }
    private GameObject timer;
    private GameObject textField;
    private bool dialogueActive;
    private string dialogueField;
    public List<DialogueData> currentDialogue { get; private set; }
    private int n;
    private float time;
    public int IndexProp { get; set; }
    private Timer timerScript;
    string[] testMessages = { "HI", "BYE", "YOU STILL HERE?", "OK BOOMER" };

    [SerializeField] private Text text;
    private float lifeTime = 0;
    private Image[] images;
    private string[] messages;
    private AudioClip[] voiceLines;
    private AudioClip music;

    private Player player;
    private DialogueEffect dialogueEffect;

    public void Start()
    {
        player = Player.instance;
    }
    private void Awake()
    {

        dialogueEffect = GetComponent<DialogueEffect>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(player.GetSettings().GetBind(KeyFeature.DIALOGUE)) && dialogueActive)
            Next();
    }

    /// <summary>
    /// Updates dialogue UI based on current dialogue index.
    /// </summary>§
    private void TickUI()
    {
        text.text = dialogueField;
        PlayNextMessage();
        DisableWithDelay();
    }

    /// <summary>
    /// Begins the dialogue by taking a dialogue-struct argument. 
    /// </summary>
    /// <param name="dialogue"></param>
    public void InitializeTextProtocol(DialogueData dialogue)
    {
        if (dialogue.messages != null)
        {
            this.messages = dialogue.messages;

            n = 0;
            dialogueActive = true;

            foreach (string msg in messages)
                lifeTime += timePerQuestion;

            TickUI();
        }
    }

    /// <summary>
    /// Moves the dialogue index forward. 
    /// </summary>
    public void Next()
    {
        if (n < messages.Length - 1)
        {
            n++;

            TickUI();
        }
        else
            TerminateDialogue();

    }

    private void PlayNextMessage()
    {

        if (dialogueEffect)
        {
            dialogueEffect.SetDialogue(text, messages[n]);
        }
        else
        {

            dialogueField = messages[n];
        }
    }

    /// <summary>
    /// Makes sure dialogue doesn't stay on the screen forever if player forgets to remove it.
    /// </summary>
    private void DisableWithDelay()
    {
        time = lifeTime;
        if (timer == null)
        {
            timer = BowoniaPool.instance.GetFromPool(PoolObject.TIMER);
            timer.GetComponent<Timer>().RunCountDown(time, TerminateDialogue, Timer.TimerType.DELAY);
        }
    }

    /// <summary>
    /// Resets index, textfield and updates UI.
    /// </summary>
    private void ResetDialogue()
    {
        n = 0;
        dialogueField = "";
        //TickUI();

    }

    /// <summary>
    /// Removes dialogue form the screen.
    /// </summary>
    private void TerminateDialogue()
    {
        ResetDialogue();
        gameObject.SetActive(false);
    }


}

/// <summary>
/// This struct takes all relevant data for dialogue sequences.
/// </summary>
[System.Serializable]
public struct DialogueData
{
    public Image[] imges;
    public AudioClip[] voiceLines;
    public AudioClip music;
    [TextArea] public string[] messages;
    public string characterName;
    public string textPrompt;
}

#region dialogueLegacy
//[SerializeField] private List<DialogueSystems> questList;
//[SerializeField] private List<DialogueSystems> tutorialSnippets;
//[SerializeField] private List<DialogueSystems> expositionMessages;
//if (DialogueProp == DialogueType.QUEST)
//    messages = questList[IndexProp].messages;
//else if (DialogueProp == DialogueType.EXPOSITION)
//    messages = expositionMessages[IndexProp].messages;
//else
//    messages = tutorialSnippets[IndexProp].messages;
    //private void DeveloperKey()
    //{
    //    DialogueData textSystem = new DialogueData();
    //    textSystem.messages = testMessages;
    //    InitializeTextProtocol(new DialogueData());
    //}
#endregion