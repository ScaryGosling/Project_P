//Author: Emil Dahl
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rotating warning image, and damaging explosion.
/// </summary>
[RequireComponent(typeof(GenericTimer))]
public class BoomerLanding : DangerousZone
{
    #region designer vars
    [SerializeField] private float animSpeed = 1.5f;
    [SerializeField] private float explosionRadius = 2;

    #endregion
    #region geometry
    private Vector3 baseScale;
    private Vector3 targetScale;
    private Quaternion targetQuaternion;

    #endregion
    #region components

    #endregion

    protected override void Start()
    {
        base.Start();
        //tempTime = destroyAfter;
        //destroyAfter = Mathf.Infinity;
    }


    public override void DestroyZone()
    {
        base.DestroyZone();
        Destroy(gameObject, destroyAfter) ;
        //BowoniaPool.instance.AddToPool(PoolObject.BOOMER_WARNINGAREA, gameObject, destroyAfter);
    }

    public override void EngageArea()
    {
        destroyAfter = 0.1f;
        base.EngageArea();
    }

    protected void FadeOut() { }

    /// <summary>
    /// Controls Animation, Damage delay and Lifetime of explosion
    /// </summary>
  


}
