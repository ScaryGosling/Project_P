//Main Author: Emil Dahl


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/FanaticDodge")]
public class FanaticDodge : ChaseBase
{
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    public override void EnterState()
    {
        base.EnterState();

        initialPosition = owner.agent.transform.position;
        targetPosition = initialPosition + new Vector3(10, 0, 0);

    }

    public override void ToDo()
    {
        base.ToDo();
        if (owner.target != null && owner.agent.enabled)
        {
            CheckForDamage();
            Dodge();
        }

    }

    protected override void Die()
    {
        base.Die();
        owner.ChangeState<FanaticDeath>();
    }

    public override void TakeDamage(float damage, float magnitude)
    {
        base.TakeDamage(damage, magnitude);

        float oldHealth = owner.Health;
        owner.Health -= damage;
        owner.ui.ChangeHealth(owner.InitialHealth, owner.Health);

        Stagger(magnitude);

    }

    protected void Dodge()
    {
        owner.agent.SetDestination(targetPosition);
        if(owner.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathPartial || Vector3.Distance(owner.agent.transform.position, targetPosition) < 1f)
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