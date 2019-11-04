using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePotion : Potion
{
    [SerializeField] private int maxResourcePotion = 3;
    [SerializeField] private int potionCost;

    public override void BuyPotion(int cost)
    {
        if (Player.instance.ResourcePotionsProp < maxResourcePotion)
        {
            Player.instance.ResourcePotionsProp += 1;
            Player.instance.GoldProp -= cost;
        }
    }

    public override int GetPotionCost()
    {
        return potionCost;
    }

    public override int GetResourceHandler()
    {
        return Player.instance.ResourcePotionsProp;
    }
}
