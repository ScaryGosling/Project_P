//Author: Emil Dahl
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// To use this class, please set panels gameObject to active and call the "InitText" function.
/// Enter desired type to control which of text you wish to display. 
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
    private Timer timerScript;
    [SerializeField] private float lifeTime = 15;
    [SerializeField] private string[] exposition;
    [SerializeField] private string[] tutorial;
    [SerializeField] private string[] quests;

    private string[] arr;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && dialogueActive)
            Next();

        if (Input.GetKeyDown(KeyCode.P))
            DeveloperKey();
    }

    private void TickUI()
    {
        dialogueField = arr[n];
        gameObject.GetComponentInChildren<Text>().text = dialogueField;
        DisableWithDelay();
        
    }

    public void InitText()
    {
        n = 0;
        dialogueActive = true;
        if (DialogueProp == DialogueType.QUEST)
            arr = quests;
        else if (DialogueProp == DialogueType.EXPOSITION)
            arr = exposition;
        else
            arr = tutorial;

        dialogueField = arr[n];
        TickUI();
    }


    public void Next()
    {
        if (n < arr.Length - 1)
        {
            n++;
            dialogueField = arr[n];
            TickUI();
        }
        else
            TerminateDialogue();

    }

    private void DisableWithDelay()
    {
        time = lifeTime;
        if(timer == null)
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
