//Main Author: Emil Dahl

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// States that inherit from this class share some sort behavior in the form of a special ability.
/// </summary>
[RequireComponent(typeof(CapsuleCollider))]
public class AbilityBase: AliveBase
{
    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ToDo()
    {
        base.ToDo();
        ExecuteAbility();
    }
    
    protected override void CheckForDamage() { }

    /// <summary>
    /// Decides which type of ability should be used in given situation.
    /// </summary>
    protected virtual void ExecuteAbility() { }

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