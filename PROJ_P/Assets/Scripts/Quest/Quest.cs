using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    [SerializeField] private int reward = 50;
    private ToggleArrowEvent toggleArrow = new ToggleArrowEvent();
    public virtual void StartQuest()
    {

    }

    public virtual void QuestDialogue()
    {

    }

    protected virtual void QuestFailed()
    {
        Debug.Log("You failed");
    }

    public virtual void QuestSucceeded()
    {
        Player.instance.GoldProp += reward;
    }

    public virtual void EndQuest()
    {

    }

    protected void FireArrow(bool toggle, GameObject target)
    {
        toggleArrow.goal = target;
        toggleArrow.toggle = toggle;
        EventSystem.Current.FireEvent(toggleArrow);
    }
}
