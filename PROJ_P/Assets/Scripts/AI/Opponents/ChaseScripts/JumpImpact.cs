//Main Author: Emil Dahl


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Hostile/Boomer/JumpImpact")]
public class JumpImpact : AbilityBase
{
    private GameObject jumpTimer, graphicalPrefab;
    private float jumpWindupTime = 3f;
    private float jumpSpeed = 3f, jumpHeight = 4f;
    private Vector3 playerPositionalDelay;
    private Vector3 direction;
    private Vector3 movement;
    private float maxHeight = 10f;
    private float currentHeight;
    private bool jumping;
    private float startDistance, jumpThreshhold, distance;

    public override void EnterState()
    {
        base.EnterState();
        owner.agent.ResetPath();
        //Try changing to stopped

        //Instantiate(graphicalPrefab, playerPositionalDelay, Quaternion.identity);
        //Don't forget to check so target is player 
        playerPositionalDelay = owner.target.transform.position;
        startDistance = Vector3.Distance(owner.agent.transform.position, playerPositionalDelay);
        //Likely to land too far above the ground // This is wrong, says unit should jump from above where it is currently standing
        //direction = playerPositionalDelay.forward * -1;



        Debug.Log("Jumping");
    }

    public override void ToDo()
    {
        base.ToDo();
        if (jumping)
            Jump();
    }

    protected override void Die()
    {
        base.Die();
        owner.ChangeState<BoomerDeath>();
    }

    public override void TakeDamage(float damage, float magnitude)
    {
        base.TakeDamage(damage, magnitude);

        float oldHealth = owner.Health;
        owner.Health -= damage;
        owner.ui.ChangeHealth(owner.InitialHealth, owner.Health);
    }

    protected override void ExecuteAbility()
    {
        base.ExecuteAbility();
        jumpTimer = Instantiate(new GameObject("JumpTimer"));
        jumpTimer.AddComponent<Timer>().RunCountDown(jumpWindupTime, ActivateJump, Timer.TimerType.DELAY);
    }

    private void ActivateJump()
    {
        jumping = true;
    }
    private void Jump()
    {
        Mathf.Clamp(currentHeight, 0, maxHeight);
        distance = Vector3.Distance(owner.agent.transform.position, playerPositionalDelay);
        owner.agent.transform.LookAt(playerPositionalDelay);
        owner.agent.SetDestination(new Vector3(playerPositionalDelay.x, playerPositionalDelay.y + maxHeight, playerPositionalDelay.z));

        if(distance <= (startDistance / 2))
        {
        owner.agent.ResetPath();
        owner.agent.SetDestination(playerPositionalDelay);
        }

        if (Vector3.Distance(owner.transform.position, playerPositionalDelay) <= 2f)
            owner.ChangeState<BoomerChase>();
    }

    public override void ExitState()
    {
        base.ExitState();
        jumping = false;
    }

    IEnumerator windUp()
    {
        yield return new WaitForSeconds(jumpWindupTime);
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