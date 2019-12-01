//Author: Emil Dahl
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rotating warning image, and damaging explosion.
/// </summary>
[RequireComponent(typeof(GenericTimer))]
public class Explosion : DangerousZone
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
    private Quaternion originalTransformRotation;
    private Vector3 originalTransformScale;
    private bool rotating;
    public bool rotationProp { get { return rotating; } set { rotating = value; } }
    protected override void Start()
    {
        base.Start();
        baseScale = transform.localScale;
        targetScale = baseScale * explosionRadius;
        targetQuaternion = Quaternion.Euler(-90, 180, 0);
        explosionRadius = Mathf.Clamp(explosionRadius, 1f, 6f);
        rotating = false;
        originalTransformRotation = gameObject.transform.localRotation;
        originalTransformScale = gameObject.transform.localScale;
    }

    private void Update()
    {
        Rotation();

        //Debug.Log(rotating);
    }

    /// <summary>
    /// Controls Animation, Damage delay and Lifetime of explosion
    /// </summary>
    private void Rotation()
    {
        if (transform.localScale != targetScale)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, animSpeed * Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetQuaternion, animSpeed * Time.deltaTime);
        }
        else
        {
            EngageArea();
            rotationProp = false;
            Debug.Log("Aids");
            //Set Animation Here
        }
    }

    public override void EngageArea()
    {
        base.EngageArea();
        rotationProp = false;
    }

    public override void DestroyZone()
    {
        base.DestroyZone();
        BowoniaPool.instance.AddToPool(PoolObject.EXPLOSION, gameObject, destroyAfter);
        gameObject.transform.localScale = originalTransformScale;
        gameObject.transform.localRotation = originalTransformRotation;
    }




}
