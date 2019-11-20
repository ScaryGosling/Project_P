//Author: Emil Dahl
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerousZone : MonoBehaviour
{
    [SerializeField] protected float damage = 25f;
    [SerializeField] protected float destroyAfter = 0.5f;

    protected AudioSource source;
    protected GameObject player;
    protected CapsuleCollider capsuleCollider;
    protected AudioClip fuse, explosion;

    protected virtual void Start()
    {
        player = Player.instance.gameObject;
        capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
        source = gameObject.GetComponent<AudioSource>();
    }
    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DealDamage();
        }
    }


    //protected void Update()
    //{
    //    EngageArea();
    //}

    public virtual void EngageArea()
    {
        Debug.Log(capsuleCollider.enabled);
        capsuleCollider.enabled = true;
        source.clip = explosion;
        source.Play();
        DestroyZone();
    }

    protected void DealDamage()
    {
        player.GetComponent<Player>().HealthProp = -damage;
    }

    public virtual void DestroyZone()
    {
        Destroy(gameObject, destroyAfter);
    }


    public void SetupSounds(AudioClip tickSound, AudioClip climaxSound)
    {
        if (source && tickSound && climaxSound)
        {
            this.explosion = climaxSound;
            this.fuse = tickSound;
            source.clip = tickSound;
            source.Play();
        }
    }
}
