//Main Author: Emil Dahl

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// All other states inherit from this one. It keeps track of some basic things such as the rigidbody. Some of the code is quite outdated, or should be moved.
/// </summary>
[RequireComponent(typeof(CapsuleCollider))]
public class HostileBaseState : State
{
    // Attributes
    private Action executable;
    [SerializeField] protected Material material;
    //Will be moved to player, same as other unit stats. //Emil
    //This will be removed soon. Dumb decision based on the fact that I wanted all enemies to be states, using a singular prefab. Will be removed. //Emil
    [SerializeField] protected float staggerCD = 0.5f;
    //Does this have to be here? Move to unit if possible. No need to set in every state. //Emil 
    [SerializeField] protected GameObject bloodParticle;

    #region components
    protected CapsuleCollider capsuleCollider;
    protected Unit owner;
    protected UnitDeath death;
    protected GameObject timer;
    #endregion
    protected Rigidbody rigidbody;
    protected Vector3 heading;
    protected const float rotationalSpeed = 0.035f;
    protected float actualDamage;
    protected float distanceToTarget;
    protected bool damaged = false;
    //protected bool timerRunning = false;
    protected bool attacking = false;
    protected Animator animator;
    protected bool timeTaskOn;
    private Action ExecutableMethod, CancellationMethod;
    private float time;
    // Methods


    public override void EnterState()
    {
        base.EnterState();
        owner.renderer.material = material;
        owner.agent.speed = owner.SpeedProp;
    }


    public override void InitializeState(StateMachine owner)
    {
        this.owner = (Unit)owner;
        animator = this.owner.GetAnimator();
    }

    public override void ToDo()
    {
        distanceToTarget = Vector3.Distance(owner.target.transform.position, owner.agent.transform.position);
        if (timeTaskOn)
            RunTimeTask();
    }

    /// <summary>
    /// Generic time task for methods. Perform action for given duration. Can use cancellation method. 
    /// </summary>
    /// <param name="ExecutableWhile"></param>
    /// <param name="AtCancellation"></param>
    /// <param name="time"></param>
    /// <returns></returns>'

    protected void TimeTask(Action ExecutableWhile, Action AtCancellation, float time)
    {
        if (!timeTaskOn)
        {
            this.ExecutableMethod = ExecutableWhile;
            this.CancellationMethod = AtCancellation;
            this.time = time;
        }

        timeTaskOn = true;
    }

    /// <summary>
    /// Ticks current timer through update. 
    /// </summary>
    private void RunTimeTask()
    {
        ExecutableMethod?.Invoke();

        time -= Time.deltaTime;

        if (CancellationMethod != null && time < 0)
        {
            CancellationMethod.Invoke();
            timeTaskOn = false;
            return;
        }
        else if (time < 0)
        {
            timeTaskOn = false;
            return;
        }
    }


}
#region EnemyBaseLegacy
// lightTreshold = owner.LightThreshold;
//     spreadAngle = Quaternion.AngleAxis(lightField.spotAngle, owner.agent.velocity);
//// protected float lightAngle;
// //private Quaternion spreadAngle;
//private float dotProduct;
//protected enum Behaviors { STAGGER, KNOCKBACK }
//[SerializeField] protected Behaviors controlBehaviors = Behaviors.STAGGER;
#endregion