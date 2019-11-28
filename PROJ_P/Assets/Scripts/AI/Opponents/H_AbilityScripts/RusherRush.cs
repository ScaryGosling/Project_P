using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Hostile/Rusher/RusherRush")]
public class RusherRush : AbilityBase
{
    private Vector3 direction;
    private Player player;
    private float rushSpeed = 12f, standardAngularSpeed, impactDamage = 25f;
    private GameObject rushTimer, rusherEndTimer;
    private bool rushing = false;
    private Vector3 targetPosition;
    private Quaternion lookAt;
    private float startRushTimer, endRushTimer;

    public override void EnterState()
    {
        base.EnterState();

        owner.agent.autoBraking = false;
        standardAngularSpeed = owner.agent.angularSpeed;
        owner.agent.angularSpeed = rushSpeed / 2;

        player = Player.instance;
        owner.agent.ResetPath();
        //rushTimer = BowoniaPool.instance.GetFromPool(PoolObject.TIMER);
        //rushTimer.GetComponent<Timer>().RunCountDown(1f, StartRush, Timer.TimerType.DELAY);
        startRushTimer = 1;
        endRushTimer = 3;
    }


    public override void ToDo()
    {
        base.ToDo();
        if (rushing)
            Rush();
        CheckForDamage();
        if (endRushTimer < 0 && rushing)
        {
            EndRush();
        }
        if (startRushTimer < 0 && !rushing)
        {
            rushing = true;
            targetPosition = player.transform.position;
        }
        if (rushing)
        {
            endRushTimer -= Time.deltaTime;
        }

            startRushTimer -= Time.deltaTime;

    }

    protected override void ExecuteAbility()
    {
        base.ExecuteAbility();
        if (!rushTimer)
        {

        }
    }
    private void StartRush()
    {


        if (!rusherEndTimer)
        {
            targetPosition = player.transform.position;
            rusherEndTimer = BowoniaPool.instance.GetFromPool(PoolObject.TIMER);
            rusherEndTimer.GetComponent<Timer>().RunCountDown(3, EndRush, Timer.TimerType.DELAY);
        }
    }

    protected override void CancelState()
    {
        owner.ChangeState<RusherChase>();
    }

    private void EndRush()
    {
        rushing = false;
        CancelState();
    }

    private void Rush()
    {
        direction = player.transform.position - owner.transform.position;
        owner.agent.Move(owner.agent.transform.forward * Time.deltaTime * rushSpeed);
        lookAt = Quaternion.LookRotation(direction);
        owner.agent.transform.rotation = Quaternion.Slerp(owner.agent.transform.rotation, lookAt, Time.deltaTime * rushSpeed / 12f);

        if (distanceToTarget <= (owner.GetAttackRange / 5) * 4)
        {
            Player.instance.HealthProp = -impactDamage;

            //rusherEndTimer.GetComponent<Timer>().CancelMethod();
            EndRush();
        }
    }
    protected override void Die()
    {
        base.Die();
        owner.ChangeState<RusherDeath>();
    }


    public override void ExitState()
    {
        if (rushTimer!=null)
        {
            rushTimer.GetComponent<Timer>().CancelMethod();
        }
        if (rusherEndTimer != null)
        {

        rusherEndTimer.GetComponent<Timer>().CancelMethod();
        }
        rushTimer = null;
        rusherEndTimer = null;
        owner.agent.angularSpeed = standardAngularSpeed;
        rushing = false;
        base.ExitState();
    }
}
