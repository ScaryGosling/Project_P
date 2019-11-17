//Main Author: Emil Dahl

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// States that inherit from this class shares some "death" behavior. They are no longer possible actors in the game world and can not be interacted with.
/// </summary>
[RequireComponent(typeof(CapsuleCollider))]
public class DeathBase : HostileBaseState
{
    [SerializeField] protected float corpseTimer = 2f;

    public override void EnterState()
    {
        base.EnterState();
        owner.PlayHurtAudio(owner.deathClip);
        DisableUnit();
    }


    public override void ToDo()
    {
        base.ToDo();
        DeathAnimation();
    }

    /// <summary>
    /// Disables the collider etc.
    /// </summary>
    protected void DisableUnit()
    {
        owner.rigidbody.isKinematic = true;

        if (owner.agent.enabled)
        {
            owner.agent.isStopped = true;
            owner.agent.enabled = false;
        }
        owner.capsuleCollider.enabled = false;
        Destroy(owner.gameObject, corpseTimer);
    }

    protected virtual void DeathAnimation(){ }
}
#region EnemyBaseLegacy
        //Quaternion rotation = Quaternion.Euler(-90, 0, 0);
        //owner.transform.localRotation = rotation;
        //owner.AliveProp = false;
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