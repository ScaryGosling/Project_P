using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Mystic/Blink")]
public class Blink : PlayerAttack
{
    private Transform player;

    [Header("Ability Specific")]
    private float range;
    private float speedBoost;
    [SerializeField] private float speedBoostDuration;
    private float safeDistance = 1.5f;
    [SerializeField] private List<BlinkUpgrades> blinkUpgrades = new List<BlinkUpgrades>();

    [System.Serializable]
    private struct BlinkUpgrades
    {
        public float range;
        public float speedboost;
        public float cooldown;
    }

    public override void UpgradeAttack()
    {
        base.UpgradeAttack();
        range = blinkUpgrades[CurrentLevel].range;
        speedBoost = blinkUpgrades[CurrentLevel].speedboost;
        cooldown = blinkUpgrades[CurrentLevel].cooldown;
    }
    public override void RunAttack()
    {
        base.RunAttack();


        //Raycast and look for obstacles
        RaycastHit hit;

        if (Physics.Raycast(player.position, player.TransformDirection(Vector3.forward), out hit, range))
        {
            if (hit.collider.CompareTag("Environment"))
            {
                Debug.Log("Hit environment");
                player.position = hit.point - player.TransformDirection(Vector3.forward) * safeDistance;
            }
            else
            {
                player.position += player.TransformDirection(Vector3.forward) * range;
            }
        }
        else
        {
            player.position += player.TransformDirection(Vector3.forward) * range;
        }


        player.GetComponent<Player>().activeStats.movementSpeed = speedBoost;

        Timer timer = BowoniaPool.instance.GetFromPool(PoolObject.TIMER).GetComponent<Timer>();
        timer.RunCountDown(speedBoostDuration, Player.instance.GetComponent<Player>().ResetStats, Timer.TimerType.DELAY);
    }

    public override void OnEquip()
    {
        base.OnEquip();
        player = Player.instance.transform;
    }
}
