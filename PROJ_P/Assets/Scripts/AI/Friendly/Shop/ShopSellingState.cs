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
        if (DistanceFromPlayer() > shopKeeper.DistanceFromPlayerToActivate || Input.GetKeyDown(Player.instance.GetKeybindSet().GetBind(KeyFeature.TOGGLE_SHOP)))
        {
            DeactivateShop();
        }
        if (!shopWindow.activeInHierarchy)
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
        Cursor.SetCursor(Player.instance.PlayerCursor, Vector2.zero, CursorMode.Auto);
    }
}
