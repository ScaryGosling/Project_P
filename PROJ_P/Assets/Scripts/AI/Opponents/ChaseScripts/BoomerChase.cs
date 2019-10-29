//Main Author: Emil Dahl


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/BoomerChase")]
public class BoomerChase : ChaseBase
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
            Chase();
            CheckForDamage();
        }
    }

    protected override void Die()
    {
        base.Die();
        owner.ChangeState<BoomerDeath>();
    }

    public override void TakeDamage(float damage)
    {
        float oldHealth = owner.Health;
        owner.Health -= damage;
        owner.ui.ChangeHealth(owner.InitialHealth, owner.Health);

        if (controlBehaviors == Behaviors.STAGGER && owner.player.GetComponent<Player>().playerClass == PlayerClass.WARRIOR)
        {
            Stagger();
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