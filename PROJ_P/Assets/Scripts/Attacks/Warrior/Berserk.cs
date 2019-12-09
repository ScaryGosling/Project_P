using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Warrior/Berserk")]
public class Berserk : PlayerAttack
{

    [Header("Ability Specific")]
    [SerializeField] private float resistance;
    private float attackSpeed;
    [SerializeField] private float movementSpeed;
    private float duration;
    [SerializeField] private List<BerserkUpgrades> berserkUpgrades = new List<BerserkUpgrades>();
    private GameObject particlesystem;

    [System.Serializable]
    private struct BerserkUpgrades
    {
        public float duration;
        public float attackSpeed;
    }

    public override void OnEquip()
    {
        base.OnEquip();
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

        particlesystem = BowoniaPool.instance.GetFromPool(PoolObject.BERSERK_PARTICLE);
        particlesystem.transform.SetParent(player.transform);
        particlesystem.transform.localPosition = new Vector3(0,0,0);

        Timer timer = BowoniaPool.instance.GetFromPool(PoolObject.TIMER).GetComponent<Timer>();
        timer.RunCountDown(duration, Reset, Timer.TimerType.DELAY);
    }

    public void Reset() {

        player.ResetStats();
        BowoniaPool.instance.AddToPool(PoolObject.BERSERK_PARTICLE, particlesystem);
    }

}
