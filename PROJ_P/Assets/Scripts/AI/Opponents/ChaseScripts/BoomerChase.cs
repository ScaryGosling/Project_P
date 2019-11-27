//Main Author: Emil Dahl


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Hostile/Boomer/BoomerChase")]
public class BoomerChase : ChaseBase
{
    private const float jumpCooldown = 30f, jumpMinDistance = 5f, jumpMaxDistance = 10f;
    private GameObject jumpCooldownTimer;
    private bool canJump = true;
    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ToDo()
    {
        base.ToDo();
        if (owner.target != null && owner.agent.enabled)
        {
            Chase();
            CheckForDamage();
            JumpDistance();
        }
    }

    protected override void Die()
    {
        base.Die();
        owner.ChangeState<BoomerDeath>();
    }


    private void JumpDistance()
    {
        if (Vector3.Distance(owner.agent.transform.position, owner.target.transform.position) > jumpMinDistance && Vector3.Distance(owner.agent.transform.position, owner.target.transform.position) < jumpMaxDistance && owner.target.CompareTag("Player"))
        {
            if (!jumpCooldownTimer && canJump)
            {
                jumpCooldownTimer = BowoniaPool.instance.GetFromPool(PoolObject.TIMER);
                jumpCooldownTimer.GetComponent<Timer>().RunCountDown(jumpCooldown, EnableAbility, Timer.TimerType.DELAY);

                canJump = false;
                //owner.ChangeState<JumpImpact>();
            }
        }
    }

    private void EnableAbility()
    {
        canJump = true;
    }
    public override void ExitState()
    {
        base.ExitState();
        jumpCooldownTimer = null;
    }

}

#region ChaseLegacy
//Stagger(magnitude);
//protected override void OperateHesitation()
//{
//    base.OperateHesitation();
//    if (Vector3.Distance(owner.gameObject.transform.position, owner.target.gameObject.transform.position) <= hesitationDistance)
//    {
//        owner.ChangeState<FanaticHesitate>();
//    }
//}
// lightAngle = lightField.spotAngle;
//ChaseEvent chaseEvent = new ChaseEvent();
//chaseEvent.gameObject = owner.gameObject;
//chaseEvent.eventDescription = "Chasing Enemy";
//chaseEvent.audioSpeaker = audioSpeaker;

//EventSystem.Current.FireEvent(chaseEvent);
#endregion