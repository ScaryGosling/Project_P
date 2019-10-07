//Main Author: Emil Dahl
//Secondary Author: Paschalis Tolios

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/ChaseState")]
public class ChaseState : EnemyBaseState
{
    private float distanceToPlayer;


    public override void EnterState()
    {
        base.EnterState();
    }
    public override void ToDo()
    {
        base.ToDo();
        if (owner.player != null)
        {
            distanceToPlayer = Vector3.Distance(owner.transform.position, owner.player.transform.position);
            owner.agent.SetDestination(owner.player.transform.position);

            if (distanceToPlayer < damageDistance && LineOfSight())
            {
                DamagePlayer(enemyBaseDamage);
            }

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