﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(AudioSource))]
public class FireballInstance : MonoBehaviour
{
    public float ExplosionRadius { get; set; }
    public float Damage { get; set; }
    public float Magnitude { get; set; }

    private AudioSource source;
    private GameObject impact;
    [SerializeField] private AudioClip impactSound;
    private Collider[] hitColliders;

    public void Start()
    {
        source = GetComponent<AudioSource>();
        if (impactSound != null)
            source.clip = impactSound;
    }
    private void OnEnable()
    {
        transform.localScale = Vector3.one;
        GetComponent<Collider>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
        transform.GetChild(0).GetComponent<Renderer>().enabled = true;
        transform.GetChild(1).gameObject.SetActive(true);
        GetComponent<Collider>().enabled = true;
        transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
    }
    public void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.CompareTag("Environment"))
        {

            Explode(collision.collider, transform.position);
        }
        else
        {
            Explode(collision.collider, new Vector3(transform.position.x, 30.9f, transform.position.z));

        }

    }

    public void Explode(Collider other, Vector3 impactPosition)
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;

        impact = BowoniaPool.instance.GetFromPool(PoolObject.FIREBALL_IMPACT);
        impact.transform.position = impactPosition;
        BowoniaPool.instance.AddToPool(PoolObject.FIREBALL_IMPACT, impact, 5);

        if(impactSound != null && Player.instance.GetSettings().UseSFX)
            source.Play();

        hitColliders = Physics.OverlapSphere(transform.position, ExplosionRadius);

        StartCoroutine(DealDamageOverTime());
       
    }

    public IEnumerator DealDamageOverTime()
    {
        //int maxEnemiesPerFrame = 10;


        for (int i = 0; i < hitColliders.Length; i++)
        {
            if ( hitColliders[i] != null && hitColliders[i].CompareTag("Enemy"))
            {
                hitColliders[i].GetComponent<Unit>().currentState.TakeDamage(Damage, Magnitude);
            }
            yield return null;
        }
            StartCoroutine(KillTimer(impactSound.length));

    }


    public IEnumerator KillTimer(float time)
    {
        transform.GetChild(0).GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;

        for(int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<ParticleSystem>().Stop();
        }

        yield return new WaitForSeconds(time);
        BowoniaPool.instance.AddToPool(PoolObject.FIREBALL, gameObject);

    }

}
