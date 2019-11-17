//Main Author: Emil Dahl

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// Currently does nothing.
/// </summary>
[RequireComponent(typeof(CapsuleCollider))]
public class HesitationBase : AliveBase
{
    [SerializeField] protected float timeToHesitate = 1;
    protected float currentTime;
    public override void EnterState()
    {
        base.EnterState();
        currentTime = timeToHesitate;
    }


    public override void ToDo()
    {
        base.ToDo();
        Stop();
    }

    protected virtual void Stop() { }
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