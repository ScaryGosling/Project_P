﻿//Main Author: Emil Dahl

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
    protected NavMeshHit cornerHit;

    public override void EnterState()
    {
        base.EnterState();
        owner.agent.autoBraking = false;
    }

    public override void ToDo()
    {
        base.ToDo();
        ExecuteAbility();
       
    }

    protected override void CheckForDamage() { }


    protected void CheckIntersection(bool selfIntersection)
    {
        intersection = owner.rigidbody.SweepTest(owner.capsuleCollider.transform.forward, out hit, owner.capsuleCollider.radius * 2, QueryTriggerInteraction.Collide);

        //bool corner = NavMesh.FindClosestEdge(owner.agent.transform.position, out cornerHit, NavMesh.GetAreaFromName("Not Walkable"));
        
        if (intersection && !(hit.collider.CompareTag("Player") && hit.collider.CompareTag("Weapon") || hit.collider.CompareTag("Zone") ||
            (hit.collider.CompareTag("Enemy") && !selfIntersection)))
        {
                Debug.Log("Cancel due to intersection!");
                Debug.Log(hit.collider.gameObject + " <color=blue>" + +hit.collider.gameObject.GetInstanceID() + "</color>");

                Debug.Log("<color=purple>" + hit.collider.tag + "</color>");
                CancelState(); 
        }

    }

    

    //protected bool LineCast()
    //{
    //    //Debug.DrawLine(owner.transform.position, owner.capsuleCollider.transform.forward, Color.red, Mathf.Infinity);
    //    if (Physics.Linecast(owner.agent.transform.position, owner.agent.pathEndPosition, out hit))
    //        return true;
    //    return false;
    //}

    protected virtual void CancelState() { }

    /// <summary>
    /// Decides which type of ability should be used in given situation.
    /// </summary>
    protected virtual void ExecuteAbility() { }

    public override void ExitState()
    {
        base.ExitState();
        owner.agent.autoBraking = true;
    }
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