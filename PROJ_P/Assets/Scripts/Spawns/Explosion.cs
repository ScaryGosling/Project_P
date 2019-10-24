using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GenericTimer))]
public class Explosion : MonoBehaviour
{
    [SerializeField] private float explosionDuration = 1f;
    [SerializeField] private float damage = 25f;
    private bool explosionComplete;
    private GameObject player;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Destroy(gameObject, explosionDuration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DealDamage();
        }
    }

    private void DealDamage()
    {
        player.GetComponent<Player>().HealthProp = -damage;
    }

}
