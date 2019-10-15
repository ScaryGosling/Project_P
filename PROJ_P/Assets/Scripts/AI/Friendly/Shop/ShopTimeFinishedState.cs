using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Friendly/Shop End of Time State")]
public class ShopTimeFinishedState : ShopBaseState
{
    public override void EnterState()
    {
        TimeLeft = false;
        Debug.Log("Shopkeeper will leave now");
    }

}
