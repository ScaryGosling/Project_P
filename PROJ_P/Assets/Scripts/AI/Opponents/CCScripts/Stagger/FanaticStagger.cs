//Author: Emil Dahl
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Hostile/Fanatic/FanaticStagger")]
public class FanaticStagger : CCBase
{
    private Vector3 direction;
    private Vector3 movement;
    private Quaternion initialRotation;
    [SerializeField] private float speed = 4f;
    [SerializeField] private float staggerMagnitude = 0.1f;

    private GameObject staggerTimer;

    public override void EnterState()
    {
        base.EnterState();
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

        owner.agent.SetDestination(owner.agent.transform.position);

        if (staggerTimer == null)
        {
            staggerTimer = BowoniaPool.instance.GetFromPool(PoolObject.TIMER);
            staggerTimer.GetComponent<Timer>().RunCountDown(staggerMagnitude, EndStagger, Timer.TimerType.DELAY);
        }
    }

    protected void EndStagger()
    {
        ResetOrientation();
        owner.ChangeState<FanaticChase>();
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