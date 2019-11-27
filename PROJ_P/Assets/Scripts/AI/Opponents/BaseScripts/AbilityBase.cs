//Main Author: Emil Dahl

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// States that inherit from this class share some sort behavior in the form of a special ability.
/// </summary>
[RequireComponent(typeof(CapsuleCollider))]
public class AbilityBase : AliveBase
{
    protected bool intersection, environmentIntersection;
    protected RaycastHit hit;

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ToDo()
    {
        base.ToDo();
        ExecuteAbility();
        CheckIntersection();
    }

    protected override void CheckForDamage() { }

    private void CheckIntersection()
    {
        intersection = owner.rigidbody.SweepTest(owner.agent.transform.forward, out hit, owner.capsuleCollider.radius * 2, QueryTriggerInteraction.Collide);

        if (intersection && !(hit.collider.CompareTag("Player") || hit.collider.CompareTag("Weapon") || hit.collider.CompareTag("Zone") || hit.collider.CompareTag("Enemy")))
        {
            CancelState();
        }

    }

    protected virtual void CancelState() { }

    /// <summary>
    /// Decides which type of ability should be used in given situation.
    /// </summary>
    protected virtual void ExecuteAbility() { }

}
#region EnemyBaseLegacy
            //Debug.Log("Ability Canceled");
            //Debug.Log("GameObject hit: " + hit.collider.gameObject);
            //Debug.Log("E-Intersection is: " + environmentIntersection);
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