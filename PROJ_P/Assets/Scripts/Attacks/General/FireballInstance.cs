using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class FireballInstance : MonoBehaviour
{
    public float ExplosionRadius { get; set; }
    public float Damage { get; set; }


    public void OnTriggerEnter(Collider other)
    {
        Explode(other);
    }

    public void Explode(Collider other)
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, ExplosionRadius);

        foreach(Collider col in hitColliders)
        {
            if (col.CompareTag("Enemy"))
            {
                Debug.Log(col.gameObject.name);
                col.GetComponent<Unit>().currentState.TakeDamage(Damage);
            }
        }
        StartCoroutine(ExplosionAnimation());
    }

    //Remove this and add real animation instead
    public IEnumerator ExplosionAnimation() {

        while (transform.lossyScale.x < ExplosionRadius) {

            transform.localScale += new Vector3(1, 1, 1);
            yield return null;
        }
        Destroy(gameObject);

    }

}
