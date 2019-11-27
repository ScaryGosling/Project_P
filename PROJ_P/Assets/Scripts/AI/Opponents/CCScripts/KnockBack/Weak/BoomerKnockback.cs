//Author: Emil Dahl
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Currently does nothing.
/// </summary>
[CreateAssetMenu(menuName = "Hostile/Boomer/BoomerKnockback")]
public class BoomerKnockback : CCBase
{
    private Vector3 direction;
    private Vector3 movement;
    private Quaternion initialRotation;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float knockBackMagnitude = 0.3f;

    GameObject knockBackTimer;

    public override void EnterState()
    {
        base.EnterState();

        direction = owner.agent.transform.forward * -1;
    }

    public override void ToDo()
    {
        base.ToDo();

    }

    protected override void Die()
    {
        base.Die();
        owner.ChangeState<BoomerDeath>();
    }



    protected void EndKnockBack()
    {
        ResetOrientation();
        owner.ChangeState<BoomerChase>();
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