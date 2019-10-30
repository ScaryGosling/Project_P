using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Potions/Mana")]
public class ManaRefill : Potion
{
    [SerializeField] private float hundredPercentCost = 100;
    float manaIncrease;
    int costToFillAll;
    float crystalsThatCanBeUsed;
    float manaNeeded;

    public override void BuyPotion(int cost)
    {
        manaIncrease = (costToFillAll == cost ? 1 : (cost / hundredPercentCost));
        Player.instance.Resource.IncreaseResource(manaIncrease);
        Player.instance.GoldProp -= cost;
        
    }
    public override int GetPotionCost()
    {
        costToFillAll = (int)(manaNeeded * hundredPercentCost);
        crystalsThatCanBeUsed = (int)(Player.instance.GoldProp / hundredPercentCost * 100) / 100.0f * hundredPercentCost;
        return (costToFillAll > Player.instance.GoldProp ? (int)crystalsThatCanBeUsed : Mathf.RoundToInt(costToFillAll));
    }
    public override int GetResourceHandler()
    {
        manaNeeded = ((1 - Player.instance.Resource.Value));
        return (costToFillAll <= Player.instance.GoldProp ? (int)(manaNeeded *100) : (int)(Player.instance.GoldProp/(hundredPercentCost/100)));
    }
}
