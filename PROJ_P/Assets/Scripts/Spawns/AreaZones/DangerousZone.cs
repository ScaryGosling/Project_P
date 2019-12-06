//Author: Emil Dahl
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerousZone : MonoBehaviour
{
    [SerializeField] protected float damage = 25f;
    [SerializeField] protected float destroyAfter = 0.5f;
    public float DestroyProp { get { return destroyAfter; } set { destroyAfter = value; } }
    protected AudioSource source;
    protected GameObject player;
    protected CapsuleCollider capsuleCollider;
    protected AudioClip fuse, explosion;
    protected float tempTime;

    private bool playerInRange = false;

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
            playerInRange = true;
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }



    public virtual void EngageArea()
    {
        capsuleCollider.enabled = true;
        if(playerInRange)
            DealDamage();
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
        playerInRange = false;
        //StartCoroutine(DeactivateAfter());
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

    public IEnumerator DeactivateAfter()
    {
        yield return new WaitForSeconds(destroyAfter);
        playerInRange = false;
        //capsuleCollider.enabled = false;
    }
}
