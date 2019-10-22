using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Wizard/Blink")]
public class Blink : PlayerAttack
{
    private Transform player;

    public override void RunAttack()
    {
        base.RunAttack();
        player.position += player.GetComponent<Rigidbody>().velocity * 100;
        

    }

    public override void OnEquip()
    {
        base.OnEquip();
        player = Player.instance.transform;
    }
}
