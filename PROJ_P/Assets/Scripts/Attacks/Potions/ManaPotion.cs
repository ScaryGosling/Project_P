using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Potions/Mana Potion")]
public class ManaPotion : Potion
{
    [SerializeField] private int manaPointsToRestore;
    [SerializeField] private int potionCost;
    public override void BuyPotion(int cost)
    {
        Player.instance.Resource.IncreaseResource((float)manaPointsToRestore / 100);
        Player.instance.GoldProp -= cost;
    }

    public override int GetPotionCost()
    {
        return potionCost;
    }

    public override int GetResourceHandler()
    {
        return manaPointsToRestore;
    }

}
