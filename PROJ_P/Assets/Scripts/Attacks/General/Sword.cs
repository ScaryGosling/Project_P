using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Sword : MonoBehaviour
{

    private float damage;
    private float magnitude;
    private PlayerAttack hack;
    private AudioSource source;
    [SerializeField] private AudioClip impactSound;
    [SerializeField] private float comboMultiplier;
    [SerializeField] private float comboTime;
    private Action methodToRun;
    private IEnumerator coroutineToRun;

    private int combo = -1;
    private Coroutine comboCoroutine;

    public void CacheComponents(float damage,float magnitude, PlayerAttack hack, Action methodToRun = null, IEnumerator coroutineToRun = null)
    {
        this.damage = damage;
        this.hack = hack;
        this.magnitude = magnitude;
        this.methodToRun = methodToRun;
        this.coroutineToRun = coroutineToRun;
        source = GetComponent<AudioSource>();
        if (impactSound != null)
            source.clip = impactSound;
    }


    public IEnumerator Combo()
    {
        combo++;
        if (combo > 0)
            Prompt.instance.RunMessage("Combo attack: " + combo, MessageType.BONUS);
        yield return new WaitForSeconds(comboTime);
        combo = -1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Player.instance.Resource.DrainResource(hack);
            State state = (HostileBaseState)other.gameObject.GetComponent<Unit>().currentState;
            if (state)
            {
                if (combo > 2)
                {
                    state.TakeDamage(damage * comboMultiplier, magnitude);
                    combo = -1;
                }
                else
                {
                    state.TakeDamage(damage, magnitude);
                    if (comboCoroutine != null)
                    {
                        StopCoroutine(comboCoroutine);
                    }
                    comboCoroutine = StartCoroutine(Combo());
                }

                
            }
            if(impactSound != null)
            {
                source.Play();
            }

            if(methodToRun != null)
            {
                methodToRun();
                methodToRun = null;
            }

            if(coroutineToRun != null)
            {
                StartCoroutine(coroutineToRun);
                coroutineToRun = null;
            }

        }
    }



}
