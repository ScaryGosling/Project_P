using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Potions/Repair Kit")]
public class RepairKit : Potion
{

    [SerializeField] private int durabilityPointsToRestore;
    [SerializeField] private int potionCost;
    private MeleeHack meleeHack;

    public override void BuyPotion(int cost)
    {
        meleeHack.IncreaseDurability(durabilityPointsToRestore /100.0f );
        Player.instance.GoldProp -= cost;
    }

    public override int GetPotionCost()
    {
        return potionCost;
    }
    public void SetMeleeHack(MeleeHack melee)
    {
        meleeHack = melee;
    }
    public override int GetResourceHandler()
    {
        return durabilityPointsToRestore;
    }
}
