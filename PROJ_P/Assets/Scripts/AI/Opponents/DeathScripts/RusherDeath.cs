//Main Author: Emil Dahl
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Hostile/Rusher/RusherDeath")]
public class RusherDeath : DeathBase
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
    //protected override void RemoveObject()
    //{
    //    //owner.ChangeState<RusherChase>();
    //    BowoniaPool.instance.AddToPool(PoolObject.ZOOMER, owner.gameObject);
    //}

    protected override void DisableUnit()
    {
        base.DisableUnit();
        BowoniaPool.instance.AddToPool(PoolObject.ZOOMER, owner.gameObject, 2);
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