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

    [SerializeField] private GameObject particles;
    [SerializeField] private GameObject lifeLeechparticles;

    [SerializeField] private GameObject[] weaponUpgrades;


    [SerializeField] private float comboMultiplier;
    [SerializeField] private float comboTime;
    private Action methodToRun;
    private IEnumerator coroutineToRun;
    private int combo = -1;
    private Coroutine comboCoroutine;
    private bool resourceDrained;
    private GameObject particlesInUse;
    private int lifeleechIterations;
    private float lifeleechRegen;

    public void Start()
    {
        particlesInUse = particles;
    }

    public void ActivateLifeLeech(int iterations, float regen)
    {
        particles.SetActive(false);
        particlesInUse = lifeLeechparticles;
        lifeleechIterations = iterations;
        lifeleechRegen = regen;
        particlesInUse.SetActive(true);
    }


    public GameObject[] GetWeaponUpgrades() { return weaponUpgrades; }

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

    public void ToggleParticles(bool toggle)
    {
        if (toggle)
            particlesInUse.SetActive(true);
        else
            particlesInUse.SetActive(false);
    }

    public void ResetDrained()
    {
        resourceDrained = false;
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
            if (!resourceDrained)
            {
            Player.instance.Resource.DrainResource(hack);
                resourceDrained = true;
            }
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
            if(impactSound != null && Player.instance.GetSettings().UseSFX)
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


            if(lifeleechIterations > 0)
            {
                Player.instance.HealthProp = lifeleechRegen;
                lifeleechIterations--;
            }
            else
            {
                DeactivateLifeleech();
            }

        }
    }

    public void DeactivateLifeleech()
    {
        lifeleechIterations = 0;
        lifeLeechparticles.SetActive(false);
        particlesInUse = particles;
    }



}
