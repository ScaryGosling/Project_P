using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class Pickup : MonoBehaviour
{

    protected enum ResourceType { Mana, Rage, Repair, Health };
    [SerializeField] protected ResourceType resType;
    [SerializeField] protected float fillAmount;
    [SerializeField] protected float despawnTime = 10f;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private GameObject particles;
    protected BoxCollider colliderB;
    protected GiveResource giveResource;
    protected Player player;

    protected virtual void Start()
    {
        player = Player.instance;
        colliderB = gameObject.GetComponent<BoxCollider>();
        colliderB.isTrigger = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<Player>())
            {
                other.GetComponent<Player>().PlayAudio(pickupSound);

                if (particles != null)
                    Instantiate(particles, player.transform.position, Quaternion.identity, player.transform);

            }
            DoSomething();
        }
    }
    
    protected void DestroyThis()
    {
        Destroy(gameObject, despawnTime);
    }


    protected virtual void DoSomething() { }
}
