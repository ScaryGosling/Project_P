//Main Author: Emil Dahl

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



[RequireComponent(typeof(CapsuleCollider))]
public class AliveBase : HostileBaseState
{
    [SerializeField] private float staggerDuration = 1f;
    private float force = 1.5f;
    //[SerializeField] private float pushMultiplier1 = 1.1f;
    //[SerializeField] private float pushMultiplier2 = 1.3f;
    //[SerializeField] private float pushMultiplier3 = 3f;
    [SerializeField] private float knockBackDuration = 0.5f;
    private float knockBackBase;
    private float baseMultiplier;
    float weightDiff;
    [SerializeField] float extraForce = 0.5f;
    protected GameObject otherTimer;
    public override void EnterState()
    {
        base.EnterState();
        Mathf.Clamp(force, 1, 5);
        knockBackBase = knockBackDuration;
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

    protected virtual void Stagger(float magnitude)
    {
        otherTimer = new GameObject();

        if (magnitude <= owner.GetWeight && magnitude != 0)
        {
            otherTimer.AddComponent<Timer>().RunCountDown(staggerDuration, StandStill, Timer.TimerType.WHILE);
        }
        else
        {
            //ManageKnockBack(magnitude);
            otherTimer.AddComponent<Timer>().RunCountDown(knockBackDuration, MoveBack, Timer.TimerType.WHILE);
        }

    }

    protected void ManageKnockBack(float magnitude)
    {
        weightDiff = magnitude - owner.GetWeight;
        
        if (weightDiff < 10)
        {
            knockBackDuration = knockBackDuration * 1.2f;
            Debug.Log("P1");
        }
        else if (weightDiff > 10 && weightDiff < 15)
        {
            knockBackDuration = knockBackDuration * 1.5f;
            Debug.Log("P2");
        }
        else
        {
            knockBackDuration = knockBackDuration * 2; 
            Debug.Log("P3");
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

    public override void TakeDamage(float damage, float magnitude)
    {
        base.TakeDamage(damage, magnitude);
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
            owner.rigidbody.AddRelativeForce(new Vector3(0, 0, -1) * force, ForceMode.Impulse);
            knockBackDuration = knockBackBase;
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