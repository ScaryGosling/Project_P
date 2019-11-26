//Main Author: Emil Dahl

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Hostile/Fanatic/FanaticDeath")]
public class FanaticDeath : DeathBase
{
    public override void EnterState()
    {
        base.EnterState();
        animator.SetTrigger("Death");
    }

    public override void ToDo()
    {
            base.ToDo();
    }

    protected override void RemoveObject()
    {
        BowoniaPool.instance.AddToPool(PoolObject.FANATIC, owner.gameObject);
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