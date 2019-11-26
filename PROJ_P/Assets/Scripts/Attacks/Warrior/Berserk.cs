using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Warrior/Berserk")]
public class Berserk : PlayerAttack
{

    private Player player;

    [Header("Ability Specific")]
    [SerializeField] private float resistance;
    private float attackSpeed;
    [SerializeField] private float movementSpeed;
    private float duration;
    [SerializeField] private List<BerserkUpgrades> berserkUpgrades = new List<BerserkUpgrades>();

    [System.Serializable]
    private struct BerserkUpgrades
    {
        public float duration;
        public float attackSpeed;
    }

    public override void OnEquip()
    {
        base.OnEquip();
        player = Player.instance;
    }

    public override void Execute()
    {
        base.Execute();
    }
    public override void UpgradeAttack()
    {
        base.UpgradeAttack();
        duration = berserkUpgrades[CurrentLevel].duration;
        attackSpeed = berserkUpgrades[CurrentLevel].attackSpeed;
    }
    public override void RunAttack()
    {
        base.RunAttack();
        player.Resource.IncreaseResource(1);
        player.activeStats.resistanceMultiplier = resistance;
        player.activeStats.attackSpeed = attackSpeed;
        player.activeStats.movementSpeed = movementSpeed;


        Timer timer = BowoniaPool.instance.GetFromPool(PoolObject.TIMER).GetComponent<Timer>();
        timer.RunCountDown(duration, player.ResetStats, Timer.TimerType.DELAY);
    }

}
