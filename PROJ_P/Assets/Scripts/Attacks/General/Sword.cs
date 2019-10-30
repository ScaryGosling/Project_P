using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{

    private float damage;
    private PlayerAttack hack;
    public void CacheComponents(float damage, PlayerAttack hack)
    {
        this.damage = damage;
        this.hack = hack;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Player.instance.Resource.DrainResource(hack);
            State state = (HostileBaseState)other.gameObject.GetComponent<Unit>().currentState;
            if (state)
            {
                state.TakeDamage(damage);
            }

        }
    }



}
