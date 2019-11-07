using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Warrior/Shield")]
public class Shield : PlayerAttack
{
    [SerializeField] private GameObject shieldPrefab;
    [SerializeField] private float duration;
    private GameObject shield;

    public override void RunAttack()
    {
        base.RunAttack();
        shield = Instantiate(shieldPrefab, Player.instance.transform);
        shield.transform.position += new Vector3(0,0.5f,0);
        Player.instance.activeStats.resistanceMultiplier = 0;
        shield.AddComponent<Timer>().RunCountDown(duration, RemoveShield, Timer.TimerType.DELAY);
    }


    public void RemoveShield()
    {
        Player.instance.ResetStats();
    }

}
