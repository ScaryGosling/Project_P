using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Wizard/Blink")]
public class Blink : PlayerAttack
{
    private Transform player;
    [SerializeField] private float range;
    [SerializeField] private float speedBoost;
    [SerializeField] private float speedBoostDuration;


    public override void RunAttack()
    {
        base.RunAttack();
        player.position += player.TransformDirection(Vector3.forward) * range;
        player.GetComponent<Player>().activeStats.movementSpeed = speedBoost;

        Timer timer = new GameObject("Timer").AddComponent<Timer>();
        timer.RunCountDown(speedBoostDuration, Player.instance.GetComponent<Player>().ResetStats);
    }

    public override void OnEquip()
    {
        base.OnEquip();
        player = Player.instance.transform;
    }
}
