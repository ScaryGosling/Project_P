//Main Author: Emil Dahl

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



[RequireComponent(typeof(CapsuleCollider))]
public class HostileBaseState : State
{
    // Attributes
    protected enum Behaviors { STAGGER, KNOCKBACK }
    [SerializeField] protected Behaviors controlBehaviors = Behaviors.STAGGER;
    [SerializeField] protected Material material;
    [SerializeField] protected float moveSpeed;
    [SerializeField] private Vector3 scale;
    [SerializeField] private bool specialDeath;
    [SerializeField] private float staggerCD = 0.5f;
    #region components
    private CapsuleCollider capsuleCollider;
    protected Unit owner;
    private UnitDeath death;
    protected GameObject timer;
    #endregion
    private Vector3 heading;
    private const float rotationalSpeed = 0.035f;
    protected const float damageDistance = 2.5f;
    protected float deathTimer;
    protected float actualDamage;
    protected float distanceToPlayer;
    private bool damaged = false;
    protected bool alive = true;
    private bool timerRunning = false;
    protected bool attacking = false;



    // Methods
    public override void EnterState()
    {
        base.EnterState();
        owner.renderer.material = material;
        owner.agent.speed = moveSpeed;
        owner.transform.localScale = scale;
        capsuleCollider = owner.GetComponent<CapsuleCollider>();
    }


    public override void InitializeState(StateMachine owner)
    {
        this.owner = (Unit)owner;
    }

    public override void ToDo()
    {

        if (owner.Health <= 0)
        {
            if (alive)
            {
                death = new UnitDeath();
                death.eventDescription = "Unit Died";
                death.enemyObject = owner.gameObject;
                EventSystem.Current.FireEvent(death);
            }
            deathTimer = 2f;
            Die();
        }


    }
    protected void CheckForDamage()
    {
        if (distanceToPlayer < damageDistance && LineOfSight() && alive && !attacking)
        {
            if (owner.getGenericTimer.timeTask)
            {
                attacking = true;
                owner.getGenericTimer.SetTimer(owner.AttackSpeed);
                attacking = !attacking;
                DamagePlayer();
            }
        }
    }

    public override void TakeDamage(float damage)
    {
        owner.Health -= damage;
        if (controlBehaviors == Behaviors.STAGGER)
        {
            ControlEffects();
        }
    }

    protected bool LineOfSight()
    {
        bool lineCast = Physics.Linecast(owner.agent.transform.position, owner.player.transform.position, owner.visionMask);
        if (lineCast)
            return false;
        return true;
    }

    protected virtual void DamagePlayer()
    {
        actualDamage = Random.Range(owner.Attack, owner.Attack * 3);
        owner.player.GetComponent<Player>().HealthProp = -actualDamage;
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
            while (startTIme > 0)
            {
                owner.transform.localScale = (owner.transform.localScale * 1.00003f);
                startTIme -= Time.deltaTime;
            }
        }
    }

    protected virtual void ControlEffects()
    {
        if (owner.getGenericTimer.timeTask && !damaged)
        {
            damaged = true;
            owner.getGenericTimer.SetTimer(staggerCD);
            damaged = false;
            owner.agent.SetDestination(owner.transform.position);
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
    //private float dotProduct;
#endregion