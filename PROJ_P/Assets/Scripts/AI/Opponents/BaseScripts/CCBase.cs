//Main Author: Emil Dahl

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// States that inherit from this class can not attack player. They also share some form of crowd control behavior. 
/// </summary>
[RequireComponent(typeof(CapsuleCollider))]
public class CCBase : AliveBase
{
    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ToDo()
    {
        base.ToDo();
    }
    
    protected override void CheckForDamage() { }

    /// <summary>
    /// Decides which type of CC should be used in given situation.
    /// </summary>
    protected virtual void ApplyCC() { }
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