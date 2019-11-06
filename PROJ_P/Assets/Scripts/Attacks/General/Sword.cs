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
    [SerializeField] private float comboMultiplier;
    [SerializeField] private float comboTime;

    private int combo = -1;
    private Coroutine comboCoroutine;

    public void CacheComponents(float damage,float magnitude, PlayerAttack hack)
    {
        this.damage = damage;
        this.hack = hack;
        this.magnitude = magnitude;
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

        }
    }



}
