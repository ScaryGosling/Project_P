using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Quest : MonoBehaviour
{
    [SerializeField] private int reward = 50;
    [SerializeField] private Color32 arrowColor = new Color32(89,0,0,255);
    private TMP_Text questReminder;
    [SerializeField] private string reminderText = "Finish the quest";
    private ToggleArrowEvent toggleArrow = new ToggleArrowEvent();
     private GameObject questReminderPanel;
    protected virtual void Start()
    {
        questReminder = QuestHandler.instance.GetQuestReminder();
        questReminderPanel = questReminder.transform.parent.gameObject;
    }
    public virtual void StartQuest()
    {
        questReminderPanel.SetActive(true);
        questReminder.text = reminderText;
        questReminder.color = new Color32(arrowColor.r, arrowColor.g, arrowColor.b, 255) ;
        Debug.Log(questReminder.color);
        Debug.Log(arrowColor);
    }

    public virtual void QuestDialogue()
    {

    }

    protected virtual void QuestFailed()
    {
        Prompt.instance.RunMessage("You failed", MessageType.WARNING);
    }

    public virtual void QuestSucceeded()
    {
        Player.instance.GoldProp += reward;
        Prompt.instance.RunMessage("You succeeded", MessageType.BONUS);
    }

    public virtual void EndQuest()
    {
        questReminderPanel.SetActive(false);
    }

    protected void FireArrow(bool toggle, GameObject target)
    {
        toggleArrow.goal = target;
        toggleArrow.toggle = toggle;
        toggleArrow.arrowColor = arrowColor;
        EventSystem.Current.FireEvent(toggleArrow);
    }
}
