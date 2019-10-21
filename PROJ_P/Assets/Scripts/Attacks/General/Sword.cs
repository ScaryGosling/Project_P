using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{

    private float damage;

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            State state = (HostileBaseState)other.gameObject.GetComponent<Unit>().currentState;
            state.TakeDamage(damage);

        }
    }


}
