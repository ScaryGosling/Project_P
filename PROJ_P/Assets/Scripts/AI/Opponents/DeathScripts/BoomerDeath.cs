//Main Author: Emil Dahl


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/BoomerDeath")]
public class BoomerDeath : DeathBase
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

    protected override void DeathAnimation()
    {
        Quaternion rotation = Quaternion.Euler(-90, 0, 0);
        owner.transform.localRotation = rotation;
        alive = false;
        float startTIme = 2;

        while (startTIme > 0)
        {
            owner.transform.localScale = (owner.transform.localScale * 1.00003f);
            startTIme -= Time.deltaTime;
        }
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