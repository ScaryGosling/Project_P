using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Warrior/Shield")]
public class Shield : PlayerAttack
{
    [SerializeField] private GameObject shieldPrefab;
    private float duration;
    private GameObject shield;
    [SerializeField] private List<float> durationPerLevel = new List<float>();

    public override void RunAttack()
    {
        base.RunAttack();
        shield = Instantiate(shieldPrefab, Player.instance.transform);
        shield.transform.position += new Vector3(0,0.5f,0);
        Player.instance.activeStats.resistanceMultiplier = 0;
        BowoniaPool.instance.GetFromPool(PoolObject.TIMER).GetComponent<Timer>().RunCountDown(duration, RemoveShield, Timer.TimerType.DELAY);
    }
    public override void UpgradeAttack()
    {
        base.UpgradeAttack();
        duration = durationPerLevel[CurrentLevel];
    }

    public void RemoveShield()
    {
        Player.instance.ResetStats();
        Destroy(shield);
    }

}
