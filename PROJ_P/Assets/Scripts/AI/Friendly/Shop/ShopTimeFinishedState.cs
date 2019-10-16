using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Friendly/Shop End of Time State")]
public class ShopTimeFinishedState : ShopBaseState
{
    public override void EnterState()
    {
        Debug.Log("Shopkeeper will leave now");

        if (Vector3.Distance(Owner.SpawnPoint, Owner.transform.position) > 1.5f)
        {

        NavMeshAgent.SetDestination(Owner.SpawnPoint);
        }
    }

    public override void ToDo()
    {

        if (Vector3.Distance(Owner.SpawnPoint, Owner.transform.position)<1.5f)
        {
            Owner.gameObject.SetActive(false);
        }
    }

}
