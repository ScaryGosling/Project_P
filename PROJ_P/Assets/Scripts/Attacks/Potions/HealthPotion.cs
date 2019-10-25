using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(menuName = "Attacks/Potions/Health")]
public class HealthPotion : Potion
{
    [SerializeField] private int maxHealthPotions = 3;
    [SerializeField] private int potionCost;

    public override void BuyPotion(int cost)
    {
        if (Player.instance.HealthPotions < maxHealthPotions)
        {
            Player.instance.HealthPotions += 1;
            Player.instance.GoldProp -= cost;
        }
    }

    public override int GetPotionCost()
    {
        return potionCost;
    }
}
