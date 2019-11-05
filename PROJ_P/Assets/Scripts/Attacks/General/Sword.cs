using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{

    private float damage;
    private float magnitude;
    private PlayerAttack hack;
    private AudioSource source;
    [SerializeField] private AudioClip impactSound;

    public void CacheComponents(float damage,float magnitude, PlayerAttack hack)
    {
        this.damage = damage;
        this.hack = hack;
        this.magnitude = magnitude;
        source = GetComponent<AudioSource>();
        if (impactSound != null)
            source.clip = impactSound;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Player.instance.Resource.DrainResource(hack);
            State state = (HostileBaseState)other.gameObject.GetComponent<Unit>().currentState;
            if (state)
            {
                state.TakeDamage(damage, magnitude);
            }
            if(impactSound != null)
            {
                source.Play();
            }

        }
    }



}
