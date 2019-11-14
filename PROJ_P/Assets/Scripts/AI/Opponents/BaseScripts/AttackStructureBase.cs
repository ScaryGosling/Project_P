//Main Author: Emil Dahl

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



[RequireComponent(typeof(CapsuleCollider))]
public class AttackStructureBase : ChaseBase
{

    public override void EnterState()
    {
        base.EnterState();
        Mathf.Clamp(hesitationChance, 0, 1);
       
    }


    public override void ToDo()
    {
        base.ToDo();
        OperateHesitation();
    }

    protected override void Chase()
    {
        distanceToTarget = Vector3.Distance(owner.transform.position, owner.target.transform.position);
        owner.agent.SetDestination(owner.target.transform.position);
        owner.transform.LookAt(owner.target.transform.position);
    }

    protected override void CheckForDamage()
    {
        if (distanceToTarget < owner.GetAttackRange && CapsuleCast() && owner.AliveProp && !attacking)
        {
            if (owner.getGenericTimer.TimeTask)
            {
                attacking = true;
                owner.PlayDamageAudio(owner.attackSound);
                owner.getGenericTimer.SetTimer(owner.AttackSpeed);
                attacking = !attacking;
                DamageTarget();
            }
        }
    }
}
#region EnemyBaseLegacy
// lightTreshold = owner.LightThreshold;
//     spreadAngle = Quaternion.AngleAxis(lightField.spotAngle, owner.agent.velocity);
//// protected float lightAngle;
// //private Quaternion spreadAngle;
//protected float DotMethod()
//{
//    heading = (owner.player.transform.position - owner.transform.position).normalized;
//    dotProduct = Vector3.Dot(owner.agent.velocity.normalized, heading);
//    return dotProduct;
//}
#endregion