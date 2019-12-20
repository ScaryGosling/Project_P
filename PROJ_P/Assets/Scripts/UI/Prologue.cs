using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Prologue : MonoBehaviour
{
    private Dialogue dialogue;
    [SerializeField] private DialogueData dialogueData;
 

    
    // Start is called before the first frame update
    void Start()
    {
        dialogue = gameObject.GetComponent<Dialogue>();
        dialogue.DialogueProp = Dialogue.DialogueType.TUTORIAL;
        dialogue.InitializeTextProtocol(dialogueData);
    }


}
