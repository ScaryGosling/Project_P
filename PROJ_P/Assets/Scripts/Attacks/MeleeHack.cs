using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Melee Hack")]
public class MeleeHack : PlayerAttack
{

    public AnimationClip slash;
    private Animation animation;
    private Sword attackBox;


    public override void Execute()
    {
        base.Execute();
        animation.AddClip(slash, "Slash");
        animation.Play("Slash");
        attackBox.DamageEnemies(damage);
    }

    public override void OnEquip()
    {
        base.OnEquip();
        Player player = Player.instance;
        attackBox = player.attackBox.GetComponent<Sword>();
        animation = player.weapon.GetComponent<Animation>();

    }




}
