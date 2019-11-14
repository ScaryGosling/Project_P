//Main Author: Emil Dahl


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/BossChase")]
public class BossChase : ChaseBase
{
    [SerializeField] private float resistance;

    public override void EnterState()
    {
        base.EnterState();
        Mathf.Clamp(resistance, 0.01f, 1);
    }

    public override void ToDo()
    {
        base.ToDo();
        if (owner.target != null && owner.agent.enabled)
        {
            Chase();
            CheckForDamage();
            Debug.Log("Boss doing shit");
        }
    }

    protected override void Die()
    {
        base.Die();
        owner.ChangeState<BossDeath>();
    }

    public override void TakeDamage(float damage, float magnitude)
    {
        base.TakeDamage(damage, magnitude);

        float oldHealth = owner.Health;
        owner.Health -= damage * resistance;
        owner.ui.ChangeHealth(owner.InitialHealth, owner.Health);

        Stagger(magnitude);
        
    }

    protected override void OperateHesitation()
    {
        base.OperateHesitation();
        if (Vector3.Distance(owner.gameObject.transform.position, owner.target.gameObject.transform.position) <= hesitationDistance)
        {
            owner.ChangeState<FanaticHesitate>();
        }
    }

    protected override void Chase()
    {
        owner.agent.avoidancePriority = 99;
        if (owner.target.CompareTag("Player"))
            owner.agent.stoppingDistance = playerStoppingDistance;
        else
        {
            owner.agent.stoppingDistance = buildingStoppingDistance;
        }

        distanceToTarget = Vector3.Distance(owner.transform.position, owner.target.transform.position);
        owner.agent.SetDestination(owner.target.transform.position);
        owner.transform.LookAt(owner.target.transform.position + new Vector3(0, owner.capsuleCollider.radius, 0));
    }

    protected override void CheckLife()
    {
        if (owner.Health <= 0)
        {
            if (owner.AliveProp)
            {
                death = new UnitDeath();
                death.eventDescription = "Unit Died";
                death.enemyObject = owner.gameObject;
                death.isBoss = true;
                EventSystem.Current.FireEvent(death);
            }
            Die();
        }
    }

    protected override void Stagger(float magnitude){}
}

#region ChaseLegacy
// lightAngle = lightField.spotAngle;
//ChaseEvent chaseEvent = new ChaseEvent();
//chaseEvent.gameObject = owner.gameObject;
//chaseEvent.eventDescription = "Chasing Enemy";
//chaseEvent.audioSpeaker = audioSpeaker;

//EventSystem.Current.FireEvent(chaseEvent);
#endregion