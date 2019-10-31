//Main Author: Emil Dahl

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



[RequireComponent(typeof(CapsuleCollider))]
public class AliveBase : HostileBaseState
{
    [SerializeField] private float staggerDuration = 1f;
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
        if (distanceToPlayer < owner.getAttackRange && LineOfSight() && alive && !attacking)
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
        GameObject timer = new GameObject();
        timer.AddComponent<Timer>().RunCountDown(staggerDuration, StandStill, Timer.TimerType.WHILE);

    }

    protected bool LineOfSight()
    {
        bool lineCast = Physics.Linecast(owner.agent.transform.position, owner.player.transform.position, owner.visionMask);
        if (lineCast)
            return false;
        return true;
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
        if(owner.agent.enabled)
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
#endregion