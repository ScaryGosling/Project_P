//Author: Emil Dahl
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GenericTimer))]
public class Explosion : MonoBehaviour
{
    #region designer vars
    [SerializeField] private float destroyAfter = 0.5f;
    [SerializeField] private float damage = 25f;
    [SerializeField] private float animSpeed = 1.5f;
    [SerializeField] private int explosionRadius = 2;

    #endregion
    #region geometry
    private Vector3 baseScale;
    private Vector3 targetScale;
    private Quaternion targetQuaternion;

    #endregion
    #region components
    private GameObject player;
    private CapsuleCollider capsuleCollider;
    #endregion

    private void Start()
    {
        player = Player.instance.gameObject;
        capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
        baseScale = transform.localScale;
        targetScale = baseScale * explosionRadius;
        targetQuaternion = Quaternion.Euler(-90, 180, 0);

        explosionRadius = Mathf.Clamp(explosionRadius, 2, 4);
    }

    private void Update()
    {
        Ignite();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DealDamage();
        }
    }

    private void DealDamage()
    {
        player.GetComponent<Player>().HealthProp = -damage;
    }

    /// <summary>
    /// Controls Animation, Damage delay and Lifetime of explosion
    /// </summary>
    private void Ignite()
    {
        if (transform.localScale != targetScale)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, animSpeed * Time.deltaTime);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetQuaternion, animSpeed * Time.deltaTime);
        }
        else
        {
            //Set Animation Here
            capsuleCollider.enabled = true;
            Destroy(gameObject, destroyAfter);
        }
    }


}
