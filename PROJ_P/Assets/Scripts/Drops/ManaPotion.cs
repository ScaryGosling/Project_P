using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPotion : MonoBehaviour
{

    public float mana;


    public void OnTriggerEnter(Collider other)
    {
 
        if (other.gameObject.CompareTag("Player")) {
            //Access resource and increase it
            Destroy(gameObject);
        }
    }

}
