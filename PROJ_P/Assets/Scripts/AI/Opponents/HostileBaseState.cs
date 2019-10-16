//Main Author: Emil Dahl

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(CapsuleCollider))]
public class HostileBaseState : State
{
    // Attributes
    [SerializeField] protected Material material;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float enemyHealth { get { return owner.Health; } }
    [SerializeField] private float hp = 100f;
    [SerializeField] private Vector3 scale;
    [SerializeField] private bool specialDeath;
    private CapsuleCollider capsuleCollider;
    private Vector3 heading;
    private float dotProduct;
    private const float rotationalSpeed = 0.035f;

    [SerializeField] protected float enemyBaseDamage = 5f;
    [SerializeField] private float maxCritical = 10f;
    [SerializeField] private float attackSpeed = 1f;
    protected float deathTimer;
    protected float actualDamage;
    protected const float damageDistance = 2.5f;
    protected Unit owner;
    private UnitDeath death;
    protected bool alive = true;
    protected float distanceToPlayer;
    protected GameObject timer;
    private bool timerRunning = false;
    private float startTime;
    protected bool attacking = false;



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

        if (distanceToPlayer < damageDistance && LineOfSight() && alive && !attacking)
        {
            if (owner.getGenericTimer.timeTask)
            {
                attacking = true;
                owner.getGenericTimer.SetTimer(attackSpeed);
                attacking = !attacking;
                DamagePlayer();
            }
        }




        //if (distanceToPlayer < damageDistance && LineOfSight() && alive)
        //{
        //    if (startTime >= 0)
        //    {
        //        startTime -= Time.deltaTime;
        //    }
        //    else
        //    {
        //        startTime = attackSpeed;
        //        DamagePlayer();
        //    }
        //}
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

    protected void DamagePlayer()
    {
        actualDamage = Random.Range(enemyBaseDamage, maxCritical);
        owner.player.GetComponent<Player>().healthProp -= actualDamage;
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
            while (startTIme > 0)
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