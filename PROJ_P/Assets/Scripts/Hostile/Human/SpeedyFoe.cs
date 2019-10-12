//Main Author: Emil Dahl
//Secondary Author: Paschalis Tolios

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/SpeedyFoe")]
public class SpeedyFoe : EnemyBaseState
{

    public override void EnterState()
    {
        base.EnterState();
    }
    public override void ToDo()
    {
        base.ToDo();
        if (owner.player != null)
        {
            Chase();

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