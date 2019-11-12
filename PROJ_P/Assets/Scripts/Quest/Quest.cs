using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    [SerializeField] private int reward = 50;
    [SerializeField] private Color32 arrowColor = new Color32(89,0,0,255);
    private ToggleArrowEvent toggleArrow = new ToggleArrowEvent();
    public virtual void StartQuest()
    {

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

    }

    protected void FireArrow(bool toggle, GameObject target)
    {
        toggleArrow.goal = target;
        toggleArrow.toggle = toggle;
        toggleArrow.arrowColor = arrowColor;
        EventSystem.Current.FireEvent(toggleArrow);
    }
}
