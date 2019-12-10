using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoEquipAbility : MonoBehaviour
{
    [SerializeField] private AbilityDropHandler[] abilitySlots = new AbilityDropHandler[3];

    public static AutoEquipAbility instance;
    private void Awake()
    {
        instance = this;
    }

    public void AutoEquip(PlayerAttack ability)
    {
        foreach (AbilityDropHandler slot in abilitySlots)
        {
            if (!slot.GetAbility() && !AlreadyEquipped(ability) && !BasicAttack(ability))
            {
                slot.SwapAbility(ability);
                break;
            }
        }
    }
    bool basicAttack;
    private bool BasicAttack(PlayerAttack ability)
    {
        basicAttack = false;
        if (ability is Projectile || ability is MeleeHack)
        {
            basicAttack = true;
        }
        return basicAttack;
    }

    bool equipped;
    private bool AlreadyEquipped(PlayerAttack ability)
    {
        equipped = false;
        foreach (AbilityDropHandler slot in abilitySlots)
        {
            if (slot.GetAbility() && ability == slot.GetAbility())
            {
                equipped = true;
                break;
            }
        }
        return equipped;
    }
}
