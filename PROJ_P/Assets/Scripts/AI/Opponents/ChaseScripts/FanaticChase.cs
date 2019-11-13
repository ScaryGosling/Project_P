//Main Author: Emil Dahl


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/FanaticChase")]
public class FanaticChase : ChaseBase
{
    private float dodgeResult;
    [SerializeField] private float dodgeAwarenessDistance = 10f;
    [SerializeField] private float dodgeChance = 1f;
    public override void EnterState()
    {
        base.EnterState();
        Mathf.Clamp(dodgeChance, 0, 100);
        dodgeChance = dodgeChance / 100f;
    }

    public override void ToDo()
    {
        base.ToDo();
        if (owner.target != null && owner.agent.enabled)
        {
            Chase();
            CheckForDamage();
            ConsiderDodge();
           
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                owner.ChangeState<FanaticDodge>();
            }
        }
    }

    protected void ConsiderDodge()
    {
        dodgeResult = Random.Range(0f, 1f);
        if (CapsuleCast() && dodgeResult <= 0.01 && Vector3.Distance(owner.transform.position, Player.instance.transform.position) <= dodgeAwarenessDistance)
            owner.ChangeState<FanaticDodge>();
    }

    protected override void Die()
    {
        base.Die();
        owner.ChangeState<FanaticDeath>();
    }

    public override void TakeDamage(float damage, float magnitude)
    {
        base.TakeDamage(damage, magnitude);

        float oldHealth = owner.Health;
        owner.Health -= damage;
        owner.ui.ChangeHealth(owner.InitialHealth, owner.Health);

        Stagger(magnitude);

    }

    protected override void CheckForDamage()
    {
        owner.agent.avoidancePriority = 99;
        //if (owner.agent.isActiveAndEnabled)
        //    owner.agent.isStopped = false;
        if (distanceToTarget < owner.GetAttackRange && CapsuleCast() && owner.AliveProp && !attacking)
        {
            if (owner.getGenericTimer.TimeTask)
            {
                attacking = true;
                owner.PlayDamageAudio(owner.attackSound);
                owner.getGenericTimer.SetTimer(owner.AttackSpeed);
                attacking = !attacking;
                DamageTarget();
            }
        }
    }

  

    protected override void OperateHesitation()
    {
        base.OperateHesitation();
        if (Vector3.Distance(owner.gameObject.transform.position, owner.target.gameObject.transform.position) <= hesitationDistance)
        {
            owner.ChangeState<FanaticHesitate>();
        }
    }
}

#region ChaseLegacy
// lightAngle = lightField.spotAngle;
//ChaseEvent chaseEvent = new ChaseEvent();
//chaseEvent.gameObject = owner.gameObject;
//chaseEvent.eventDescription = "Chasing Enemy";
//chaseEvent.audioSpeaker = audioSpeaker;

//EventSystem.Current.FireEvent(chaseEvent);
#endregion