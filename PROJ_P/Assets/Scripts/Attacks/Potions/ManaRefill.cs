using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Potions/Mana")]
public class ManaRefill : Potion
{
    [SerializeField] private float hundredPercentCost = 100;
    float    manaIncrease;
    float costToFillAll;
    public override void BuyPotion(int cost)
    {
        manaIncrease = (costToFillAll == cost ? 1 : (cost / hundredPercentCost));
        Player.instance.Resource.IncreaseResource(manaIncrease);
        Player.instance.GoldProp -= cost;
        
    }
    public override int GetPotionCost()
    {
        costToFillAll = (manaNeeded * hundredPercentCost);
        return (costToFillAll > Player.instance.GoldProp ? Player.instance.GoldProp : Mathf.RoundToInt(costToFillAll));
    }
    float manaNeeded;
    public override int GetResourceHandler()
    {
        manaNeeded = ((1 - Player.instance.Resource.Value));
        return (costToFillAll <= Player.instance.GoldProp ? (int)(manaNeeded *100) : (int)(Player.instance.GoldProp/(hundredPercentCost/100)));
    }
}
