using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Potions/Mana")]
public class ManaRefill : Potion
{
    [SerializeField] private float hundredPercentCost = 100;
    public override void BuyPotion(int cost)
    {
        Player.instance.Resource.IncreaseResource(1);
        Player.instance.GoldProp -= cost;
        
    }

    public override int GetPotionCost()
    {
        return (int)((1-Player.instance.Resource.Value) * hundredPercentCost);
    }

}
