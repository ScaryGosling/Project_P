using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public virtual void StartQuest()
    {

    }

    protected virtual void QuestFailed()
    {

    }

    public virtual void QuestSucceeded()
    {
        Debug.Log("You won");
    }

    public virtual void EndQuest()
    {

    }
}
