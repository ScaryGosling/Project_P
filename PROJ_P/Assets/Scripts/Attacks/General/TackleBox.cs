using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TackleBox : MonoBehaviour
{

    public bool DealDamageOnCollision { get; set; }

    public void OnTriggerEnter(Collider other)
    {
        if (DealDamageOnCollision && other.CompareTag("Enemy"))
        {
            other.GetComponent<Unit>().currentState.TakeDamage(Player.instance.damage, Player.instance.magnitude);
        }
    }
}
