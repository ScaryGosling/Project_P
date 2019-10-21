using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Warrior/Hack")]
public class MeleeHack : PlayerAttack
{

    public AnimationClip slash;
    private Animation animation;
    private Sword sword;


    public override void Execute()
    {
        base.Execute();
        
    }

    public override void RunAttack()
    {
        base.RunAttack();
        sword.SetDamage(damage[CurrentLevel]);
        animation.AddClip(slash, "Slash");
        animation.Play("Slash");
    }

    public override void OnEquip()
    {
        base.OnEquip();
        Player player = Player.instance;
        sword = player.weapon.GetComponent<Sword>();
        animation = player.weapon.GetComponent<Animation>();

    }




}
