//Author: Emil Dahl

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CapsuleCollider))]
public class FriendlyBaseState : State
{
    // Attributes
    #region CurrentlyUnusedVars
    //[SerializeField] private float hp = 100f;
    //[SerializeField] private bool specialDeath;
    //private float dotProduct;
    //private Vector3 heading;
    //protected float deathTimer;
    //[SerializeField] protected float enemyBaseDamage = 0.5f;
    //private const float rotationalSpeed = 0.035f;
    //protected const float damageDistance = 2.5f;
    //private UnitDeath death;
    //protected bool alive = true;
    //protected float actualDamage;
    //protected float distanceToPlayer;

    #endregion
    [SerializeField] protected Material material;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float enemyHealth { get { return owner.Health; }  }
    [SerializeField] private Vector3 scale;
    private CapsuleCollider capsuleCollider;

    protected Unit owner;
    



    // Methods
    public override void EnterState()
    {
        base.EnterState();
        owner.Renderer.material = material;
        owner.agent.speed = moveSpeed;
        owner.transform.localScale = scale;
        capsuleCollider = owner.GetComponent<CapsuleCollider>();
    }


    public override void InitializeState(StateMachine owner)
    {
        this.owner = (Unit)owner;
    }

    public override void ToDo()
    {

    }
    


}
#region EnemyBaseLegacy
// lightTreshold = owner.LightThreshold;
//     spreadAngle = Quaternion.AngleAxis(lightField.spotAngle, owner.agent.velocity);
//// protected float lightAngle;
// //private Quaternion spreadAngle;
#endregion