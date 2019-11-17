//Main Author: Emil Dahl

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// Every behavior that inherits from this state shares some "living" functionality. 
/// </summary>
[RequireComponent(typeof(CapsuleCollider))]
public class AliveBase : HostileBaseState
{
    protected GameObject otherTimer;

    public override void EnterState()
    {
        base.EnterState();
        owner.agent.obstacleAvoidanceType = ObstacleAvoidanceType.GoodQualityObstacleAvoidance;
    }

    public override void ToDo()
    {
        base.ToDo();
        CheckLife();
    }

    /// <summary>
    /// Keeps track of units life. 
    /// </summary>
    protected virtual void CheckLife()
    {
        if (owner.Health <= 0)
        {

            if (owner.AliveProp)
            {
                death = new UnitDeath();
                death.eventDescription = "Unit Died";
                death.enemyObject = owner.gameObject;
                death.isBoss = false;
                EventSystem.Current.FireEvent(death);

                Die();
            }
        }
    }

    /// <summary>
    /// Checks if unit can attack or not. Override if unit should not be able to do damage in specific situations.
    /// </summary>
    protected virtual void CheckForDamage()
    {
        owner.agent.avoidancePriority = 99;

        if (distanceToTarget < owner.GetAttackRange && CapsuleCast() && owner.AliveProp && !attacking)
        {
            if (owner.getGenericTimer.TimeTask)
            {
                attacking = true;
                owner.PlayDamageAudio(owner.attackSound);
                owner.getGenericTimer.SetTimer(owner.AttackSpeed);
                attacking = !attacking;
                DamageTarget();
                animator.SetTrigger("Melee");
            }
        }
    }

    /// <summary>
    /// Deals damage to target based on random range.
    /// </summary>
    protected virtual void DamageTarget()
    {
        actualDamage = Random.Range(owner.Attack, owner.Attack * 3);
        owner.agent.avoidancePriority = 0;
        TargetControl();
    }

    /// <summary>
    /// Chooses how to attack based on target.
    /// </summary>
    protected void TargetControl()
    {
        if (owner.weapon) //if basic foe animate
        {
            owner.weapon.Attack();
        }
        if (owner.target != Player.instance.gameObject)  //if quest
        {
            owner.ProtectionQuestProp.TakeDamage(actualDamage);

        }
        else    //if target == player
        {
            if (owner.weapon) //if basic foe
            {
                owner.weapon.damage = actualDamage;
            }
            else       //if other foes
            {
                Player.instance.HealthProp = -actualDamage;
            }
        }
    }


    protected virtual void Die()
    {
        owner.AliveProp = false;
    }

    /// <summary>
    /// Checks if player (or their weapons/projectles) is in front of this unit. Used to deal damage and dodge.
    /// </summary>
    /// <returns></returns>
    protected bool CapsuleCast()
    {
        bool capsuleCast = Physics.CapsuleCast(owner.agent.transform.position, owner.transform.forward, owner.capsuleCollider.radius, owner.gameObject.transform.forward, 20f, owner.visionMask);
        if (capsuleCast)
            return true;
        return false;
    }


    /// <summary>
    /// What happens when unit takes damage.
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="magnitude"></param>
    public override void TakeDamage(float damage, float magnitude)
    {
        base.TakeDamage(damage, magnitude);
        owner.PlayHurtAudio(owner.takeDamageClip);
        GameObject splatter = Instantiate(bloodParticle, owner.transform.position, Quaternion.identity);
        splatter.AddComponent<Timer>().RunCountDown(4, PlaceboMethod, Timer.TimerType.DELAY);
        if (owner.target.CompareTag("Player"))
            owner.target.GetComponent<Player>().GoldProp += owner.GetGold;
    }
    protected virtual void SetCrowdControl(float magnitude) { }

    private void StandStill()
    {
        if (owner.agent.enabled)
            owner.agent.SetDestination(owner.gameObject.transform.position);
    }


    private void PlaceboMethod() { }
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
//owner.agent.enabled = false;
//owner.rigidbody.isKinematic = true;
//owner.agent.transform.LookAt(owner.player.transform.position);
//Vector3 direction = owner.player.transform.position - owner.agent.transform.position;
//Vector3 newPosition = direction.normalized * force;
//owner.agent.SetDestination((owner.agent.transform.forward.normalized * -1));
//owner.agent.velocity = direction * force * Time.deltaTime;
//timer.AddComponent<Timer>().RunCountDown(1f, StandStill, Timer.TimerType.DELAY);

// MoveBack
//protected virtual void Stagger(float magnitude)
//{
//    otherTimer = new GameObject();

//    if (magnitude <= owner.GetWeight && magnitude != 0)
//    {
//        otherTimer.AddComponent<Timer>().RunCountDown(staggerDuration, StandStill, Timer.TimerType.WHILE);
//    }
//    else
//    {
//        ManageKnockBack(magnitude);
//        otherTimer.AddComponent<Timer>().RunCountDown(BaseKnockBackDuration, ForceBack, Timer.TimerType.WHILE);
//    }

//}

//protected void ManageKnockBack(float magnitude)
//{
//    weightDiff = magnitude - owner.GetWeight;

//    if (weightDiff < 10)
//    {
//        BaseKnockBackDuration = BaseKnockBackDuration * 1.1f;
//    }
//    else if (weightDiff > 10 && weightDiff < 15)
//    {
//        BaseKnockBackDuration = BaseKnockBackDuration * 1.2f;
//    }
//    else
//    {
//        BaseKnockBackDuration = BaseKnockBackDuration * 1.3f;
//    }
//}

//private void ForceBack()
//{
//    if (owner.agent && owner.agent.enabled)
//    {
//        owner.agent.isStopped = true;
//        owner.transform.LookAt(owner.target.transform.position);
//        owner.rigidbody.AddRelativeForce(new Vector3(0, 0, -1) * force, ForceMode.Impulse);
//        BaseKnockBackDuration = knockStartValue;
//        owner.agent.isStopped = false;
//    }
//}
    //protected bool CombatAwareness()
    //{
    //    //I'll optimize this later 

    //    bool hit = Physics.CapsuleCast(owner.agent.transform.position, Player.instance.gameObject.transform.position, owner.capsuleCollider.radius, owner.gameObject.transform.forward,
    //        100f, owner.visionMask);
    //    if (hit)
    //        return false;
    //    return true;
    //}
#endregion