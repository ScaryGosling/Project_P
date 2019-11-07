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
    public enum DialogueType { EXPOSITION, TUTORIAL, QUEST }
    private DialogueType type = DialogueType.QUEST;
    public DialogueType DialogueProp { get { return type; } set { type = value; } }
    private GameObject timer;
    private GameObject textField;
    private bool dialogueActive;
    private string dialogueField;
    private int n;
    private float time;
    public int IndexProp { get; set; }
    private Timer timerScript;

    [SerializeField] Dictionary<int, string[]> quests;
    [SerializeField] Dictionary<int, string[]> expositions;
    [SerializeField] Dictionary<int, string[]> tutorials;

    private float lifeTime = 0;
    [SerializeField] float timePerQuestion = 10f;

    [SerializeField] private string[] expositionArray;
    [SerializeField] private string[] tutorialArray;
    [SerializeField] private string[] questArray;
    [SerializeField] private List<DialogueSystems> questList;
    [SerializeField] private List<DialogueSystems> tutorialSnippets;
    [SerializeField] private List<DialogueSystems> expositionMessages;

    [System.Serializable]
    public struct DialogueSystems
    {
        public string[] messages;
        public Image[] imges;
        public int index;
    }

    private string[] messages;



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && dialogueActive)
            Next();

        if (Input.GetKeyDown(KeyCode.P))
            DeveloperKey();

    }

    private void TickUI()
    {
        dialogueField = messages[n];
        gameObject.GetComponentInChildren<Text>().text = dialogueField;
        DisableWithDelay();
    }

    public void InitText()
    {
        n = 0;
        IndexProp = 1;
        dialogueActive = true;



        if (DialogueProp == DialogueType.QUEST)
            messages = questList[IndexProp].messages;
        else if (DialogueProp == DialogueType.EXPOSITION)
            messages = expositionMessages[IndexProp].messages;
        else
            messages = tutorialSnippets[IndexProp].messages;

        foreach (string msg in messages)
            lifeTime += timePerQuestion;

        Debug.Log(lifeTime);
        dialogueField = messages[n];
        TickUI();
    }


    public void Next()
    {
        if (n < messages.Length - 1)
        {
            n++;
            dialogueField = messages[n];
            TickUI();
        }
        else
            TerminateDialogue();

    }

    private void DisableWithDelay()
    {
        time = lifeTime;
        if (timer == null)
        {
            timer = new GameObject("DialogueTimer");
            timer.AddComponent<Timer>().RunCountDown(time, TerminateDialogue, Timer.TimerType.DELAY);
        }
    }

    private void ResetDialogue()
    {
        n = 0;
        dialogueField = "";
        TickUI();

    }

    private void TerminateDialogue()
    {
        ResetDialogue();
        gameObject.SetActive(false);
    }

    private void DeveloperKey()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            InitText();
        }
    }

}
