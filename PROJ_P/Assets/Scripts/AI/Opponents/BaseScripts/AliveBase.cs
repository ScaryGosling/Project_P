//Main Author: Emil Dahl

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



[RequireComponent(typeof(CapsuleCollider))]
public class AliveBase : HostileBaseState
{
    [SerializeField] protected float staggerDuration = 1f;
    [SerializeField] protected float force = 4f;
    [SerializeField] protected float BaseKnockBackDuration = 2f;
    private float knockStartValue;
    float weightDiff;
    protected GameObject otherTimer;
    public override void EnterState()
    {
        base.EnterState();
        Mathf.Clamp(force, 1, 5);
        knockStartValue = BaseKnockBackDuration;
        owner.agent.obstacleAvoidanceType = ObstacleAvoidanceType.GoodQualityObstacleAvoidance;
    }


    public override void InitializeState(StateMachine owner)
    {
        this.owner = (Unit)owner;
    }

    public override void ToDo()
    {
        base.ToDo();
        CheckLife();
        CheckForDamage();
    }

    protected void CheckLife()
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
            Die();
        }
    }

    protected virtual void CheckForDamage()
    {
        owner.agent.avoidancePriority = 99;
        if(owner.agent.isActiveAndEnabled)
        owner.agent.isStopped = false;
        if (distanceToTarget < owner.GetAttackRange && CapsuleCast() && alive && !attacking)
        {
            //owner.agent.SetDestination(owner.agent.transform.position);
            if (owner.getGenericTimer.TimeTask)
            {
                attacking = true;
                owner.PlayAudio(owner.attackSound);
                owner.getGenericTimer.SetTimer(owner.AttackSpeed);
                attacking = !attacking;
                DamageTarget();
            }
        }
    }

    protected virtual void DamageTarget()
    {
        actualDamage = Random.Range(owner.Attack, owner.Attack * 3);
        owner.agent.avoidancePriority = 0;
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

    /// <summary>
    /// Used to call different death-states from different enemies 
    /// </summary>
    protected virtual void Die() { }

    protected virtual void Stagger(float magnitude)
    {
        otherTimer = new GameObject();

        if (magnitude <= owner.GetWeight && magnitude != 0)
        {
            otherTimer.AddComponent<Timer>().RunCountDown(staggerDuration, StandStill, Timer.TimerType.WHILE);
        }
        else
        {
            ManageKnockBack(magnitude);
            otherTimer.AddComponent<Timer>().RunCountDown(BaseKnockBackDuration, MoveBack, Timer.TimerType.WHILE);
        }

    }

    protected void ManageKnockBack(float magnitude)
    {
        weightDiff = magnitude - owner.GetWeight;
        
        if (weightDiff < 10)
        {
            BaseKnockBackDuration = BaseKnockBackDuration * 1.2f;
        }
        else if (weightDiff > 10 && weightDiff < 15)
        {
            BaseKnockBackDuration = BaseKnockBackDuration * 1.3f;
        }
        else
        {
            BaseKnockBackDuration = BaseKnockBackDuration * 1.4f; 
        }
    }
    protected bool CapsuleCast()
    {
        //bool lineCast = Physics.Linecast(owner.agent.transform.position, owner.player.transform.position, owner.visionMask);
        bool capsuleCast = Physics.CapsuleCast(owner.agent.transform.position, owner.target.transform.position,
            owner.capsuleCollider.radius, owner.gameObject.transform.forward);
        if (capsuleCast)
            return true;
        return false;
    }

    public override void TakeDamage(float damage, float magnitude)
    {
        base.TakeDamage(damage, magnitude);
        owner.PlayAudio(owner.takeDamageClip);
        GameObject splatter = Instantiate(bloodParticle, owner.transform.position, Quaternion.identity);
        splatter.AddComponent<Timer>().RunCountDown(4, PlaceboMethod, Timer.TimerType.DELAY);
        if(owner.target.CompareTag("Player"))
        owner.target.GetComponent<Player>().GoldProp += owner.GetGold;
    }

    private void StandStill()
    {
        if (owner.agent.enabled)
            owner.agent.SetDestination(owner.gameObject.transform.position);
    }

    private void MoveBack()
    {
        if (owner.agent.enabled)
        {
            owner.agent.isStopped = true;
            owner.transform.LookAt(owner.target.transform.position);

            owner.rigidbody.AddRelativeForce(new Vector3(0,0,-1) * force, ForceMode.Impulse);
            BaseKnockBackDuration = knockStartValue;
            owner.agent.isStopped = false;
            Debug.Log("MovesBack");
        }
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
#endregion