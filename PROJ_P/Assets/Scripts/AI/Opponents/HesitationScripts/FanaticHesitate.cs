//Main Author: Emil Dahl


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/FanaticHesitate")]
public class FanaticHesitate : HesitationBase
{
    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ToDo()
    {
        base.ToDo();
        if (owner.player != null && owner.agent.enabled)
        {
            CheckForDamage();
        }
    }

    protected override void Die()
    {
        base.Die();
        owner.ChangeState<FanaticDeath>();
    }

    public override void TakeDamage(float damage)
    {
        float oldHealth = owner.Health;
        owner.Health -= damage;
        owner.ui.ChangeHealth(owner.InitialHealth, owner.Health);

        if (controlBehaviors == Behaviors.STAGGER)
        {
            Stagger();
        }
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