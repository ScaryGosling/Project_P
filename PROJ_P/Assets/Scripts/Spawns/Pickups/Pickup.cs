using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
[CreateAssetMenu(menuName = "Enemy/FatFoe")]
public class Pickup : MonoBehaviour
{

    protected enum ResourceType { Mana, Rage, Repair, Health };
    [SerializeField] protected ResourceType resType;
    [SerializeField] protected float fillAmount;
    [SerializeField] protected float despawnTime = 10f;
    protected BoxCollider colliderB;
    protected GiveResource giveResource;
    protected Player player;

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        colliderB = gameObject.GetComponent<BoxCollider>();
        colliderB.isTrigger = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DoSomething();
        }
    }
    
    protected void DestroyThis()
    {
        Destroy(gameObject, despawnTime);
    }


    protected virtual void DoSomething() { }
}
