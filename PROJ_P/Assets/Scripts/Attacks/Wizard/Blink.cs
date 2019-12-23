using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Mystic/Blink")]
public class Blink : PlayerAttack
{
    private Transform playerTransform;

    [Header("Ability Specific")]
    private float range;
    private float speedBoost;
    [SerializeField] private float speedBoostDuration;
    private float safeDistance = 1.5f;
    [SerializeField] private List<BlinkUpgrades> blinkUpgrades = new List<BlinkUpgrades>();
    private GameObject blinkParticle;

    [System.Serializable]
    private struct BlinkUpgrades
    {
        public float range;
        public float speedboost;
        public float cooldown;
    }

    protected override void SetTooltipText()
    {
        tooltip = "Range: " + blinkUpgrades[CurrentLevel].range + "->" +
            blinkUpgrades[CurrentLevel + 1].range.ToString() +"\n" +
           "Speed boost: " + blinkUpgrades[CurrentLevel].speedboost + "->" +
            blinkUpgrades[CurrentLevel + 1].speedboost.ToString() + "\n" +
           "Cooldown: " + blinkUpgrades[CurrentLevel].cooldown + "->" +
            blinkUpgrades[CurrentLevel + 1].cooldown.ToString();
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

        if (Physics.Raycast(playerTransform.position, playerTransform.TransformDirection(Vector3.forward), out hit, range))
        {
            if (hit.collider.CompareTag("Environment"))
            {
                playerTransform.position = hit.point - playerTransform.TransformDirection(Vector3.forward) * safeDistance;
            }
            else
            {
                playerTransform.position += playerTransform.TransformDirection(Vector3.forward) * range;
            }
        }
        else
        {
            playerTransform.position += playerTransform.TransformDirection(Vector3.forward) * range;
        }


        player.activeStats.movementSpeed = speedBoost;

        blinkParticle = BowoniaPool.instance.GetFromPool(PoolObject.BLINK_PARTICLE);
        blinkParticle.transform.SetParent(player.transform);
        blinkParticle.transform.localPosition = new Vector3(0,0,0);

        Timer timer = BowoniaPool.instance.GetFromPool(PoolObject.TIMER).GetComponent<Timer>();
        timer.RunCountDown(speedBoostDuration, Reset, Timer.TimerType.DELAY);
    }

    public void Reset()
    {
        player.ResetStats();
        BowoniaPool.instance.AddToPool(PoolObject.BLINK_PARTICLE, blinkParticle);
    }

    public override void OnEquip()
    {
        base.OnEquip();
        playerTransform = player.transform;
    }
}
