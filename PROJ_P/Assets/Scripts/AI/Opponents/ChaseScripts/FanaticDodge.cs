//Author: Emil Dahl
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles the dodge behavior unique to the fanatic enemy type.
/// </summary>
[CreateAssetMenu(menuName = "Hostile/Fanatic/FanaticDodge")]
public class FanaticDodge : ChaseBase
{
    private Vector3 direction;
    private Vector3 movement;
    private Quaternion initialRotation;
    [SerializeField] private float dodgeSpeed = 8f;
    [SerializeField] private float dodgeMagnitude = 0.3f;
    private float diceResult;
    GameObject dodgeTimer;

    public override void EnterState()
    {
        base.EnterState();

        diceResult = Random.Range(0f, 1f);

        if (diceResult <= 0.5)
            direction = owner.agent.transform.right;
        else
            direction = owner.agent.transform.right * -1;
    }

    public override void ToDo()
    {
        base.ToDo();
        if (owner.target != null && owner.agent.enabled)
            Dodge();
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

        SetCrowdControl(magnitude);
    }

    protected void Dodge()
    {
        if (owner.agent.enabled)
            owner.agent.ResetPath();

        owner.transform.GetChild(2).transform.LookAt(Player.instance.gameObject.transform.position);

        if (!owner.agent.isPathStale)
        {
            movement = direction * dodgeSpeed * Time.deltaTime;
            owner.agent.Move(movement);

        }
        if (dodgeTimer == null)
        {
            dodgeTimer = new GameObject("Dodge Timer");
            dodgeTimer.AddComponent<Timer>().RunCountDown(dodgeMagnitude, EndDodge, Timer.TimerType.DELAY);
        }
    }

    protected override void SetCrowdControl(float magnitude)
    {
        switch (weight.Compare(magnitude))
        {
            case 1:
                owner.ChangeState<FanaticStagger>();
                break;
            case 2:
                owner.ChangeState<FanaticKnockback>();
                break;
            case 3:
                owner.ChangeState<FanaticSuperKnockback>();
                break;
            default:
                break;
        }
    }

    protected void EndDodge()
    {
        ResetOrientation();
        owner.ChangeState<FanaticChase>();
    }

    /// <summary>
    /// Makes sure the mesh watches the player when it leaves this state.
    /// </summary>
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