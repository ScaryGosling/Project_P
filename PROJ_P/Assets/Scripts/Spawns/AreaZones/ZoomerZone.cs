using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomerZone : DangerousZone
{
    private BoxCollider boxCollider;
    // Start is called before the first frame update
    protected override void Start()
    {
        boxCollider = gameObject.GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
    }

    //private void OnEnable()
    //{
    //    gameObject.GetComponent<Unit>().HitSomething = false;
    //}

    //// Update is called once per frame
    //protected override void OnTriggerEnter(Collider other)
    //{
    //    base.OnTriggerEnter(other);
    //    if (CompareTag("Player"))
    //        DealDamage();
    //    else
    //        gameObject.GetComponent<Unit>().HitSomething = true;

    //    gameObject.SetActive(false);
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    gameObject.GetComponent<Unit>().HitSomething = false;
    //}

    public override void DestroyZone() { }
}
