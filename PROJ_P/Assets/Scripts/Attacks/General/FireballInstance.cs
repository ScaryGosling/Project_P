using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class FireballInstance : MonoBehaviour
{
    [SerializeField] private float explosionRadius;
    private float damage;

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void OnCollisionEnter(Collision other)
    {
        Explode(other.collider);
    }

    public void Explode(Collider other)
    {

        Collider[] hitColliders = Physics.OverlapSphere(other.transform.position, explosionRadius);

        foreach(Collider collider in hitColliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                collider.GetComponent<Unit>().currentState.TakeDamage(damage);
            }
        }
        Destroy(gameObject);
    }
}
