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
        if (Vector3.Distance(owner.spawnPoint, owner.transform.position) > 1.5f)
        {

            navMeshAgent.SetDestination(owner.spawnPoint);
        }
    }

    public override void ToDo()
    {

        if (Vector3.Distance(new Vector3(owner.spawnPoint.x, 0, owner.spawnPoint.z), new Vector3(owner.transform.position.x, 0, owner.transform.position.z)) <1.5f)
        {
            owner.gameObject.SetActive(false);
        }
    }

}
