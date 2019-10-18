using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Friendly/Shop End of Time State")]
public class ShopTimeFinishedState : ShopBaseState
{
    public override void EnterState()
    {
        Debug.Log("Shopkeeper will leave now");
        pressEtext.SetActive(false);
        Player.instance.SetHover(false);
        if (Vector3.Distance(Owner.SpawnPoint, Owner.transform.position) > 1.5f)
        {

            NavMeshAgent.SetDestination(Owner.SpawnPoint);
        }
    }

    public override void ToDo()
    {

        if (Vector3.Distance(new Vector3(Owner.SpawnPoint.x, 0, Owner.SpawnPoint.z), new Vector3(Owner.transform.position.x, 0, Owner.transform.position.z)) <1.5f)
        {
            Owner.gameObject.SetActive(false);
        }
    }

}
