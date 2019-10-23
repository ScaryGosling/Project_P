using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Friendly/Shop End of Time State")]
public class ShopTimeFinishedState : ShopBaseState
{
    public override void EnterState()
    {
        pressEtext.SetActive(false);
        Player.instance.SetHover(false);
        if (Vector3.Distance(shopKeeper.spawnPoint, shopKeeper.transform.position) > 1.5f)
        {

            navMeshAgent.SetDestination(shopKeeper.spawnPoint);
        }
    }

    public override void ToDo()
    {

        if (Vector3.Distance(new Vector3(shopKeeper.spawnPoint.x, 0, shopKeeper.spawnPoint.z), new Vector3(shopKeeper.transform.position.x, 0, shopKeeper.transform.position.z)) <1.5f)
        {
            shopKeeper.gameObject.SetActive(false);
        }
    }

}
