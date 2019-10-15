//Main Author: Emil Dahl

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CapsuleCollider))]
public class FriendlyBaseState : State
{
    // Attributes
    [SerializeField] protected Material material;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float enemyHealth { get { return owner.Health; }  }
    [SerializeField] private float hp = 100f;
    [SerializeField] private Vector3 scale;
    [SerializeField] private bool specialDeath;
    private CapsuleCollider capsuleCollider;
    private Vector3 heading;
    private float dotProduct;
    private const float rotationalSpeed = 0.035f;

    [SerializeField] protected float enemyBaseDamage = 0.5f;
    protected float deathTimer;
    protected float actualDamage;
    protected const float damageDistance = 2.5f;
    protected Unit owner;
    private UnitDeath death;
    protected bool alive = true;
    protected float distanceToPlayer;
    



    // Methods
    public override void EnterState()
    {
        base.EnterState();
        owner.Renderer.material = material;
        owner.agent.speed = moveSpeed;
        owner.Health = hp;
        owner.transform.localScale = scale;
        capsuleCollider = owner.GetComponent<CapsuleCollider>();

    }


    public override void InitializeState(StateMachine owner)
    {
        this.owner = (Unit)owner;
    }

    public override void ToDo()
    {

        if (enemyHealth <= 0)
        {
            if (alive)
            {
                death = new UnitDeath();
                death.eventDescription = "Unit Died";
                death.enemyObject = owner.gameObject;
                EventSystem.Current.FireEvent(death);
                Debug.Log("unitDead");
            }
            deathTimer = 2f;
            Die();
        }


    }
    protected void checkForDamage()
    {
        if (distanceToPlayer < damageDistance && LineOfSight() && alive)
        {
            DamagePlayer(enemyBaseDamage);
        }
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
        //actualDamage = Mathf.Floor(Random.Range(enemyBaseDamage, enemyBaseDamage * 1.5f));
        owner.player.GetComponent<Player>().healthProp -= enemyBaseDamage;

    }

    protected void Chase()
    {
        distanceToPlayer = Vector3.Distance(owner.transform.position, owner.player.transform.position);
        owner.agent.SetDestination(owner.player.transform.position);
    }


    protected float DotMethod()
    {
        heading = (owner.player.transform.position - owner.transform.position).normalized;
        dotProduct = Vector3.Dot(owner.agent.velocity.normalized, heading);
        return dotProduct;
    }

    protected virtual void Die()
    {
        DeathAnimation();
        owner.agent.isStopped = true;
        capsuleCollider.enabled = false;
        Destroy(owner.gameObject, deathTimer);

    }
    protected void DeathAnimation()
    {
        Quaternion rotation = Quaternion.Euler(-90, 0, 0);
        owner.transform.localRotation = rotation;
        alive = false;
        float startTIme = 2;

        if (specialDeath)
        {
            while(startTIme > 0)
            {
            owner.transform.localScale = (owner.transform.localScale * 1.00003f);
            startTIme -= Time.deltaTime;
            }
        }
    }



}
#region EnemyBaseLegacy
// lightTreshold = owner.LightThreshold;
//     spreadAngle = Quaternion.AngleAxis(lightField.spotAngle, owner.agent.velocity);
//// protected float lightAngle;
// //private Quaternion spreadAngle;
#endregion