using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class Pickup : MonoBehaviour
{

    [SerializeField] protected float fillAmount;
    [SerializeField] protected float despawnTime = 10f;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private GameObject particles;
    [SerializeField] private PoolObject type;
    protected BoxCollider colliderB;
    protected GiveResource giveResource;
    protected Player player;
    private GameObject instantiated;

    [SerializeField] private AudioSource source;

    protected virtual void Start()
    {
        player = Player.instance;
        colliderB = gameObject.GetComponent<BoxCollider>();
        colliderB.isTrigger = true;
        source.clip = pickupSound;
    }

    /// <summary>
    /// Logic for when some object is interacted with.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.gameObject.layer != 12) //12 is the layer id of "Weapon"
        {
            if (player.GetSettings().UseSFX)
                source.Play();

            if (type != PoolObject.NULL)
            {
                instantiated = BowoniaPool.instance.GetFromPool(type);
                instantiated.transform.SetParent(player.transform);
                instantiated.transform.position = player.transform.position;
            }

            DoSomething();

            transform.GetChild(0).gameObject.SetActive(false);

            GetComponent<Collider>().enabled = false;
            Destroy(gameObject, source.clip.length);
        }
    }

    /// <summary>
    /// Some items must be terminated during the wave, but after some time. Such as health drops.
    /// </summary>
    protected void DestroyThis()
    {
        Destroy(gameObject, despawnTime);
    }

    /// <summary>
    /// This method is run when something is picked up, or collected. Performs completely different tasks depending on situation. 
    /// </summary>
    protected virtual void DoSomething() { }
}
#region region
//protected enum ResourceType { Mana, Rage, Repair, Health };
//[SerializeField] protected ResourceType resType;
#endregion
