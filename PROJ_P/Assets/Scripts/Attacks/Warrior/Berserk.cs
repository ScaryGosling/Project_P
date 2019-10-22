using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Warrior/Berserk")]
public class Berserk : PlayerAttack
{

    private Player player;

    [SerializeField] private float resistance;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float movementSpeed;

    public override void OnEquip()
    {
        base.OnEquip();
        player = Player.instance;
    }

    public override void RunAttack()
    {
        base.RunAttack();
        player.Resource.IncreaseResource(1);
        player.activeStats.resistanceMultiplier = resistance;
        player.activeStats.attackSpeed = attackSpeed;
        player.activeStats.movementSpeed = movementSpeed;


        Timer timer = new GameObject("Timer").AddComponent<Timer>();
        timer.RunCountDown(5, player.ResetStats);
    }

}
