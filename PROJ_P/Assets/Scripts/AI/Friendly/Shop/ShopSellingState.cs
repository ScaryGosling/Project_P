using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Friendly/Shop Selling State")]
public class ShopSellingState : ShopBaseState
{

    public override void EnterState()
    {
        ShopWindow.SetActive(true);
    }


    public override void ToDo()
    {
        if (DistanceFromPlayer() > DistanceFromPlayerToActivate)
        {
            DeactivateShop();
        }
    }
    private void DeactivateShop()
    {
        Owner.ChangeState<ShopBaseState>();
    }

    public override void ExitState()
    {
        ShopWindow.SetActive(false);
    }
}
