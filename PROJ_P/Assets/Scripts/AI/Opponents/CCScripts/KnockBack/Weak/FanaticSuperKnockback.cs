//Author: Emil Dahl
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Hostile/Fanatic/FanaticSuperKnockback")]
public class FanaticSuperKnockback : CCBase
{
    private Vector3 direction;
    private Vector3 movement;
    private Quaternion initialRotation;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float knockBackMagnitude = 2f;

    GameObject knockBackTimer;

    public override void EnterState()
    {
        base.EnterState();

        direction = owner.agent.transform.forward * -1;
    }

    public override void ToDo()
    {
        base.ToDo();
        if (owner.target != null && owner.agent.enabled)
            ApplyCC();
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
    
    }

    protected override void ApplyCC()
    {
        if (owner.agent.enabled)
            owner.agent.ResetPath();

        owner.transform.GetChild(2).transform.LookAt(Player.instance.gameObject.transform.position);

        movement = direction * speed * Time.deltaTime;
        owner.agent.Move(movement);

        if (knockBackTimer == null)
        {
            knockBackTimer = BowoniaPool.instance.GetFromPool(PoolObject.TIMER);
            knockBackTimer.GetComponent<Timer>().RunCountDown(knockBackMagnitude, EndKnockBack, Timer.TimerType.DELAY);
        }
    }

    protected void EndKnockBack()
    {
        if (owner.Health >0)
        {
            ResetOrientation();
            owner.ChangeState<FanaticChase>();
        }

    }

    protected void ResetOrientation()
    {
        initialRotation = owner.agent.transform.rotation;

        if (owner.agent.enabled)
            owner.agent.ResetPath();

        owner.transform.GetChild(2).transform.rotation = initialRotation;
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