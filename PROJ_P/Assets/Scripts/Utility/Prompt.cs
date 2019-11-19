using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Prompt : MonoBehaviour
{
    [SerializeField] private GameObject textPrefab;
    [SerializeField] private Transform panel;

    public static Prompt instance;
    private List<string> previousMessages = new List<string>();

    public void Awake()
    {
        instance = this;
    }

    public void RunMessage(string message, MessageType messageType) {

        if (previousMessages.Contains(message))
            return;

        switch (messageType)
        {
            case MessageType.WARNING:
                if (!Player.instance.GetKeybindSet().useWarnings)
                    return;
                break;

            case MessageType.INFO:
                if (!Player.instance.GetKeybindSet().useInfo)
                    return;
                break;

            case MessageType.BONUS:
                if (!Player.instance.GetKeybindSet().useBonus)
                    return;
                break;
        }

        if(panel.childCount > 2)
        {
            for(int i = panel.childCount - 1; i >= 2; i--)
            {
                Destroy(panel.GetChild(i).gameObject);
            }
        }

        TextMeshProUGUI text = Instantiate(textPrefab, panel).GetComponent<TextMeshProUGUI>();
        text.text = message;
        previousMessages.Add(message);
        StartCoroutine(DeleteMessage(message));

        switch (messageType)
        {
            case MessageType.WARNING:
                text.color = Color.red;
                break;

            case MessageType.BONUS:
                text.color = Color.green;
                break;

            case MessageType.INFO:
                text.color = Color.yellow;
                break;

        }




        StartCoroutine(KillMessage(text.gameObject));

    }

    public IEnumerator KillMessage(GameObject text) {

        yield return new WaitForSeconds(text.GetComponent<Animation>().clip.length);
        Destroy(text);
    }

    public IEnumerator DeleteMessage(string message) {

        yield return new WaitForSeconds(2);
        previousMessages.Remove(message);
    }

}

public enum MessageType
{
    WARNING, BONUS, INFO
}
