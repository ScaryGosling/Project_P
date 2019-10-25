using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Warrior/Hack")]
public class MeleeHack : PlayerAttack
{
    [Header("Ability Specific")]
    [SerializeField] private AnimationClip slash;
    private Animation animation;
    private Sword sword;


    public override void Execute()
    {
        base.Execute();
        
    }

    public override void RunAttack()
    {
        sword.GetComponent<Collider>().enabled = true;
        animation.Play("Slash");
        Timer timer = new GameObject("Timer").AddComponent<Timer>();
        timer.RunCountDown(slash.length, ResetSword);
    }

    Player player;
    public override void OnEquip()
    {
        base.OnEquip();
        player = Player.instance;
        sword = player.weapon.GetComponent<Sword>();
        animation = player.weapon.GetComponent<Animation>();
        animation.AddClip(slash, "Slash");
        sword.CacheComponents(damage, this);
    }

    public void ResetSword()
    {
        sword.GetComponent<Collider>().enabled = false; 
    }




}
