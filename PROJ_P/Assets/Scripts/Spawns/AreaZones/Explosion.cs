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
    private bool rotating;

    protected override void Start()
    {
        base.Start();
        baseScale = transform.localScale;
        targetScale = baseScale * explosionRadius;
        targetQuaternion = Quaternion.Euler(-90, 180, 0);
        explosionRadius = Mathf.Clamp(explosionRadius, 1f, 6f);
        rotating = false;
        EngageArea();
    }

    private void Update()
    {
        Rotation();
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
            Debug.Log("FinishedRotation");
            base.EngageArea();
            //Set Animation Here
        }
    }




}
