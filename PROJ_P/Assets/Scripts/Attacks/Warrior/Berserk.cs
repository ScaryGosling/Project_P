using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Warrior/Berserk")]
public class Berserk : PlayerAttack
{

    Player player;

    public override void OnEquip()
    {
        base.OnEquip();
        player = Player.instance;
    }

    public override void RunAttack()
    {
        base.RunAttack();
        player.Resource.IncreaseResource(1);
        player.activeStats.resistanceMultiplier = 0;
        player.activeStats.attackSpeed = 1.2f;
    }
}
