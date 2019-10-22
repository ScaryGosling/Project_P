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
    }
}
