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

    public override void EnterState()
    {
        base.EnterState();

        owner.agent.autoBraking = false;
        standardAngularSpeed = owner.agent.angularSpeed;
        owner.agent.angularSpeed = rushSpeed / 2;

        player = Player.instance;
        owner.agent.ResetPath();
    }


    public override void ToDo()
    {
        base.ToDo();
        if (rushing)
            Rush();
    }

    protected override void ExecuteAbility()
    {
        base.ExecuteAbility();
        if (!rushTimer)
        {
            rushTimer = new GameObject("RushTimer");
            rushTimer.AddComponent<Timer>().RunCountDown(1f, StartRush, Timer.TimerType.DELAY);
        }
    }

    private void StartRush()
    {
        rushing = true;
        if (!rusherEndTimer)
        {
            targetPosition = player.transform.position;
            rusherEndTimer = new GameObject("RushEndTimer");
            rusherEndTimer.AddComponent<Timer>().RunCountDown(3, EndRush, Timer.TimerType.DELAY);
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

        if (distanceToTarget <= 2f)
        {
            player.HealthProp -= impactDamage;
            Destroy(rusherEndTimer);
            EndRush();
        }
    }



    public override void ExitState()
    {
        owner.agent.angularSpeed = standardAngularSpeed;
        rushing = false;
        base.ExitState();
    }
}
