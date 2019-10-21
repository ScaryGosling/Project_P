using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Blink")]
public class Blink : PlayerAttack
{
    private Transform player;

    public override void RunAttack()
    {
        base.RunAttack();
        player.position += player.TransformDirection(Vector3.forward) * 10;
        

    }

    public override void OnEquip()
    {
        base.OnEquip();
        player = Player.instance.transform;
    }
}
