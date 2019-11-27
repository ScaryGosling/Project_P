//Main Author: Emil Dahl


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Currently does nothing.
/// </summary>
[CreateAssetMenu(menuName = "Hostile/Fanatic/FanaticHesitate")]
public class FanaticHesitate : HesitationBase
{
    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ToDo()
    {
        base.ToDo();
        if (owner.target != null && owner.agent.enabled)
        {
            CheckForDamage();
        }
    }

    protected override void Die()
    {
        base.Die();
        owner.ChangeState<FanaticDeath>();
    }


    protected override void Stop()
    {
        base.Stop();
        if (currentTime >= 0)
        {
            currentTime -= Time.deltaTime;
            owner.agent.SetDestination(owner.gameObject.transform.position);
        }
        else
            owner.ChangeState<FanaticChase>();
    }
}

#region ChaseLegacy
// lightAngle = lightField.spotAngle;
//ChaseEvent chaseEvent = new ChaseEvent();
//chaseEvent.gameObject = owner.gameObject;
//chaseEvent.eventDescription = "Chasing Enemy";
//chaseEvent.audioSpeaker = audioSpeaker;

//EventSystem.Current.FireEvent(chaseEvent);
#endregion