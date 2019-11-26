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
    private const float jumpSmoother = 1f, fallDownDistance = 3f;
    private GameObject warningArea;
    [SerializeField] private GameObject warningAreaPrefab;

    public override void EnterState()
    {
        base.EnterState();
        owner.agent.ResetPath();

        //Don't forget to check so target is player 
        playerPositionalDelay = owner.target.transform.position;
        startDistance = Vector3.Distance(owner.agent.transform.position, playerPositionalDelay);
        mesh = owner.agent.transform.GetChild(4);
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
        if (!jumpTimer && !warningArea)
        {
            jumpTimer = BowoniaPool.instance.GetFromPool(PoolObject.TIMER);
            jumpTimer.GetComponent<Timer>().RunCountDown(jumpWindupTime, ActivateJump, Timer.TimerType.DELAY);
            warningArea = BowoniaPool.instance.GetFromPool(PoolObject.BOOMER_WARNINGAREA);
            warningArea.transform.position = playerPositionalDelay;
            warningArea.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        }
    }

    protected override void CancelState()
    {
        warningArea.GetComponent<BoomerLanding>().DestroyProp = 0.01f;
        warningArea.GetComponent<BoomerLanding>().DestroyZone();
        owner.ChangeState<BoomerChase>();

        //while (mesh.transform.position.y != owner.capsuleCollider.center.y)
        //{
        //    mesh.transform.position = Vector3.Lerp(mesh.transform.position, new Vector3(mesh.transform.position.x, owner.capsuleCollider.radius, mesh.transform.position.z), Time.deltaTime * jumpSmoother * 2);
        //}

    }

    private void ActivateJump()
    {
        jumping = true;
    }

    private void Jump()
    {

        distance = Vector3.Distance(owner.agent.transform.position, playerPositionalDelay);

        owner.agent.SetDestination(playerPositionalDelay);

        mesh.transform.position = Vector3.Lerp(owner.agent.transform.position, new Vector3(owner.agent.transform.position.x, owner.agent.transform.position.y + 20, owner.agent.transform.position.z), Time.deltaTime * jumpSmoother);

        if (distance <= 3f)
        {
            mesh.transform.position = Vector3.Lerp(mesh.transform.position, new Vector3(mesh.transform.position.x, owner.capsuleCollider.radius, mesh.transform.position.z), Time.deltaTime * jumpSmoother * 2);
            //warningArea.GetComponent<BoomerLanding>().DestroyZone();
            warningArea.GetComponent<BoomerLanding>().EngageArea();
            owner.ChangeState<BoomerChase>();
        }
    }



    public override void ExitState()
    {
        base.ExitState();
        jumping = false;
        jumpTimer = null;
        warningArea = null;
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