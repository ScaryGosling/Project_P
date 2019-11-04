//Main Author: Emil Dahl

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



[RequireComponent(typeof(CapsuleCollider))]
public class AliveBase : HostileBaseState
{
    [SerializeField] private float staggerDuration = 1f;
    [SerializeField] private float pushForce = 1.3f;
    protected GameObject otherTimer;
    public override void EnterState()
    {
        base.EnterState();
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

    protected void CheckForDamage()
    {
        if (distanceToPlayer < owner.getAttackRange && CapsuleCast() && alive && !attacking)
        {
            if (owner.getGenericTimer.TimeTask)
            {
                attacking = true;
                owner.getGenericTimer.SetTimer(owner.AttackSpeed);
                attacking = !attacking;
                DamagePlayer();
            }
        }
    }

    protected virtual void DamagePlayer()
    {
        actualDamage = Random.Range(owner.Attack, owner.Attack * 3);
        owner.player.GetComponent<Player>().HealthProp = -actualDamage;
    }

    /// <summary>
    /// Used to call different death-states from different enemies 
    /// </summary>
    protected virtual void Die() { }

    protected virtual void Stagger()
    {
        otherTimer = new GameObject();
        if (controlBehaviors == Behaviors.STAGGER)
            otherTimer.AddComponent<Timer>().RunCountDown(staggerDuration, StandStill, Timer.TimerType.WHILE);
        else if (controlBehaviors == Behaviors.KNOCKBACK)
        {
            owner.agent.ResetPath();
            otherTimer.AddComponent<Timer>().RunCountDown(0.5f, MoveBack, Timer.TimerType.WHILE);
        }

    }

    protected bool CapsuleCast()
    {
        //bool lineCast = Physics.Linecast(owner.agent.transform.position, owner.player.transform.position, owner.visionMask);
        bool capsuleCast = Physics.CapsuleCast(owner.agent.transform.position, owner.player.transform.position,
            owner.capsuleCollider.radius, owner.gameObject.transform.forward);
        if (capsuleCast)
            return true;
        return false;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        GameObject splatter = Instantiate(bloodParticle, owner.transform.position, Quaternion.identity);
        splatter.AddComponent<Timer>().RunCountDown(4, PlaceboMethod, Timer.TimerType.DELAY);
        owner.player.GetComponent<Player>().GoldProp += owner.GetGold;
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
            owner.rigidbody.AddRelativeForce(new Vector3(0, 0, -1) * pushForce, ForceMode.Impulse);
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