//Main Author: Emil Dahl

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class EnemyBaseState : State
{
    // Attributes
    [SerializeField] protected Material material;
    [SerializeField] protected float moveSpeed;
    private CapsuleCollider capsuleCollider;
    private Vector3 heading;
    private float dotProduct;
    private const float rotationalSpeed = 0.035f;

    protected const float enemyBaseDamage = 5f;
    protected float deathTimer;
    protected float actualDamage;
    protected const float damageDistance = 2.5f;
    protected float enemyHealth = 100f;
    protected Enemy owner;


    // Methods
    public override void EnterState()
    {
        base.EnterState();
        owner.Renderer.material = material;
        owner.agent.speed = moveSpeed;
        capsuleCollider = owner.GetComponent<CapsuleCollider>();
    }

    public override void InitializeState(StateMachine owner)
    {
        this.owner = (Enemy)owner;
    }

    protected bool LineOfSight()
    {
        bool lineCast = Physics.Linecast(owner.agent.transform.position, owner.player.transform.position, owner.visionMask);
        if (lineCast)
            return false;
        return true;

        //if (DotMethod() > lightTreshold && Vector3.Distance(owner.agent.transform.position, owner.player.transform.position) < lightField)
        //    return true;
        //return false;
    }

    protected void DamagePlayer(float val)
    {
        actualDamage = Mathf.Floor(Random.Range(enemyBaseDamage, enemyBaseDamage * 1.5f));
        Debug.Log("Damage dealt: " + actualDamage);
    }

    protected void Die()
    {
        DeathAnimation();
        Destroy(owner.gameObject, deathTimer);
    }

    protected float DotMethod()
    {
        heading = (owner.player.transform.position - owner.transform.position).normalized;
        dotProduct = Vector3.Dot(owner.agent.velocity.normalized, heading);
        return dotProduct;
    }

    protected void DeathAnimation()
    {
        Quaternion rotation = Quaternion.Euler(-90, 0, 0);
        owner.transform.localRotation = rotation;
        owner.agent.enabled = false;
    }


    
}
#region EnemyBaseLegacy
// lightTreshold = owner.LightThreshold;
//     spreadAngle = Quaternion.AngleAxis(lightField.spotAngle, owner.agent.velocity);
//// protected float lightAngle;
// //private Quaternion spreadAngle;
#endregion