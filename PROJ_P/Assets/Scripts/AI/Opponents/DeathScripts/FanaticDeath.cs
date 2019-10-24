//Main Author: Emil Dahl

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/FanaticDeath")]
public class FanaticDeath : DeathBase
{
    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ToDo()
    {
        if (owner.player != null)
            base.ToDo();
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