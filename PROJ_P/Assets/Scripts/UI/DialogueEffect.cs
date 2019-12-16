using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueEffect : MonoBehaviour
{

    private string dialogueText;
    private Text text;
    [SerializeField] private float typeSpeed = 0.015f;
    public float SpeedProp { get { return typeSpeed; } set { typeSpeed = value; } }



    public void SetDialogue(Text textObject, string dialogueText)
    {
        StopCoroutine("PlayText");
        text = textObject;
        this.dialogueText = dialogueText;
        text.text = "";
        StartCoroutine("PlayText");
    }

    public void Stop()
    {
        StopCoroutine("PlayText");
    }


    IEnumerator PlayText()
    {

        foreach (char c in dialogueText)
        {
            text.text += c;

            yield return new WaitForSeconds(typeSpeed);
        }

    }

}
