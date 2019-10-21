using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Friendly/Shop Selling State")]
public class ShopSellingState : ShopBaseState
{

    public override void EnterState()
    {
        shopWindow.SetActive(true);
    }


    public override void ToDo()
    {
        if (DistanceFromPlayer() > distanceFromPlayerToActivate)
        {
            DeactivateShop();
        }
    }
    private void DeactivateShop()
    {
        shopKeeper.ChangeState<ShopBaseState>();
    }

    public override void ExitState()
    {
        shopWindow.SetActive(false);
    }
}
