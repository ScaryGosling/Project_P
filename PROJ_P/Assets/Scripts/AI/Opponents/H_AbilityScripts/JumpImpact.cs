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

    private RaycastHit downHit;
    private JumpState jumpState;
    private bool newCode = true;

    private enum JumpState
    {
        JUMP, LAND, HOVER, NULL
    }
    public override void EnterState()
    {
        base.EnterState();
        owner.agent.ResetPath();

        //Don't forget to check so target is player 
        playerPositionalDelay = owner.target.transform.position;
        startDistance = Vector3.Distance(owner.agent.transform.position, playerPositionalDelay);
        mesh = owner.transform.GetChild(4);
        jumpState = JumpState.NULL;
        warningArea = null;
    }

    public override void ToDo()
    {
        base.ToDo();

        //Debug.Log(jumpState);
        if (newCode)
        {
            switch (jumpState)
            {
                case JumpState.JUMP:
                    StartJump();
                    break;
                case JumpState.HOVER:
                    Hover();
                    TestGoal();
                    break;
                case JumpState.LAND:
                    EndJump();
                    break;
            }
        }
        else
        {
            if (jumping)
                Jump();
        }


    }


    private void MoveUpwards()
    {

        distance = Vector3.Distance(owner.transform.position, playerPositionalDelay);

        owner.agent.SetDestination(playerPositionalDelay);


        if (mesh.transform.position.y < owner.transform.position.y + owner.agent.height/2)
        {
            mesh.transform.position = new Vector3(owner.transform.position.x, mesh.transform.position.y + Time.deltaTime*2, owner.transform.position.z);

        }
    }

    public void StartJump()
    {
        MoveUpwards();


        if (mesh.transform.position.y >= owner.transform.position.y + owner.agent.height/2)
        {
            jumpState = JumpState.HOVER;
        }
    }
    private void Hover()
    {
        distance = Vector3.Distance(owner.transform.position, playerPositionalDelay);
        owner.agent.SetDestination(playerPositionalDelay);
    }
    private void EndJump()
    {
        if (BoomerLanded())
        {
            owner.ChangeState<BoomerChase>();
        }

    }

    private bool BoomerLanded()
    {
        if (mesh.transform.position.y > owner.agent.transform.position.y - owner.agent.height / 2)
        {
            mesh.transform.position = new Vector3(owner.transform.position.x, mesh.transform.position.y - Time.deltaTime * 5, owner.transform.position.z);
            return false;
        }
        else
        {
            mesh.transform.position = new Vector3(owner.agent.transform.position.x, owner.agent.transform.position.y - owner.agent.height / 2, owner.agent.transform.position.z);
            warningArea.GetComponent<BoomerLanding>().EngageArea();
            jumpState = JumpState.NULL;
            return true;
        }

    }

    private void TestGoal()
    {
        CastDown();

        if (distance <= 3f)
        {
            jumpState = JumpState.LAND;
        }
    }



    protected override void Die()
    {
        base.Die();
        warningArea.GetComponent<BoomerLanding>().DestroyProp = 0.01f;
        warningArea.GetComponent<BoomerLanding>().DestroyZone();
        owner.ChangeState<BoomerDeath>();
    }



    protected override void ExecuteAbility()
    {
        base.ExecuteAbility();

        if (!jumpTimer && !warningArea)
        {

            TimeTask(null, ActivateJump, jumpWindupTime);

            warningArea = BowoniaPool.instance.GetFromPool(PoolObject.BOOMER_WARNINGAREA);
            warningArea.transform.position = playerPositionalDelay;
            warningArea.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        }
    }
    private void ActivateJump()
    {
        jumping = true;
        jumpState = JumpState.JUMP;
    }

    protected override void CancelState()
    {
        warningArea.GetComponent<BoomerLanding>().DestroyProp = 0.01f;
        warningArea.GetComponent<BoomerLanding>().DestroyZone();
        owner.ChangeState<BoomerChase>();
    }



    private void Jump()
    {
        MoveUpwards();

        CastDown();
        if (distance <= 3f)
        {
            mesh.transform.position = Vector3.Lerp(mesh.transform.position, new Vector3(owner.agent.transform.position.x, downHit.transform.position.y, owner.agent.transform.position.z), Time.deltaTime * jumpSmoother * 2);
            warningArea.GetComponent<BoomerLanding>().EngageArea();
            owner.ChangeState<BoomerChase>();
        }
    }

    private bool CastDown()
    {
        return Physics.Raycast(mesh.transform.position, mesh.transform.up * -1, out downHit, Mathf.Infinity, owner.visionMask * -1);
    }



    public override void ExitState()
    {
        base.ExitState();
        while (!BoomerLanded())
        {

        }
        mesh.transform.position = new Vector3(owner.agent.transform.position.x, owner.agent.transform.position.y - owner.agent.height / 2, owner.agent.transform.position.z);

        jumpState = JumpState.NULL;
        jumping = false;
        jumpTimer = null;;
        //warningArea = null;
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