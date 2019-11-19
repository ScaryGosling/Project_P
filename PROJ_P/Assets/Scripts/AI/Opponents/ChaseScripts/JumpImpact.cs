//Main Author: Emil Dahl


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Hostile/Boomer/JumpImpact")]
public class JumpImpact : AbilityBase
{
    private GameObject jumpTimer, graphicalPrefab;
    [SerializeField] private float jumpWindupTime = 3f;
    private float jumpSpeed = 3f, jumpHeight = 10f;
    private Vector3 playerPositionalDelay;
    private Vector3 direction;
    private Vector3 movement;
    private Vector3 initialPosition;
    private float currentHeight;
    private bool jumping;
    private float startDistance, jumpThreshhold, distance;
    private Transform mesh;
    private const float upwardsMomentum = 0.03f, downwardsMomentum = 0.1f;

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
        mesh = owner.agent.transform.GetChild(4);
        initialPosition = owner.agent.transform.position;
        Debug.Log(mesh);
    }

    public override void ToDo()
    {
        base.ToDo();
        if (jumping)
        {
            Jump();
            CheckForDamage();
        }
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
        if (!jumpTimer)
        {
        jumpTimer = Instantiate(new GameObject("JumpTimer"));
        jumpTimer.AddComponent<Timer>().RunCountDown(jumpWindupTime, ActivateJump, Timer.TimerType.DELAY);
        }
    }

    private void ActivateJump()
    {
        jumping = true;
    }

    private void Jump()
    {

        distance = Vector3.Distance(owner.agent.transform.position, playerPositionalDelay);
        //Debug.Log("distance" + " " + distance);
        owner.agent.SetDestination(playerPositionalDelay);

        //if(distance >= 10f)

        mesh.transform.position = Vector3.Lerp(owner.agent.transform.position, new Vector3(owner.agent.transform.position.x, owner.agent.transform.position.y + 20, owner.agent.transform.position.z), Time.deltaTime * 2f);


        if (distance <= 3f)
        {
            mesh.transform.position = Vector3.Lerp(mesh.transform.position, new Vector3(mesh.transform.position.x, owner.capsuleCollider.radius, mesh.transform.position.z), Time.deltaTime * 2f);
            owner.ChangeState<BoomerChase>();
        }
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