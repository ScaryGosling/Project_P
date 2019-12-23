using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Attacks/Warrior/Hack")]
public class MeleeHack : PlayerAttack
{
    [Header("Ability Specific")]
    [SerializeField] private AnimationClip forwardSlash;
    [SerializeField] private AnimationClip backSlash;
    private Animation animation;
    private Sword sword;
    private Resource durability;
    [SerializeField] private int durabilityDecrease = 1;
    [SerializeField] private Text durabilityText;
    [SerializeField] [Range(0, 1)] [Tooltip ("Percentage of damage to be applied when durability is 0")] 
    private float zeroDurabilityDamagePercentage = 0.2f;
    private Image durabilityImage;
    private Collider swordCollider;

    

    public override void OnEnable()
    {
        base.OnEnable();
        durability = CreateInstance<Resource>();
    }
    public override void Execute()
    {
        if (durability.Value == 0)
        {
            sword.CacheComponents(damage * zeroDurabilityDamagePercentage, magnitude, this);
        }
        else
        {
            sword.CacheComponents(damage, magnitude, this);
        }
        base.Execute();
    }

    public override void RunAttack()
    {
        sword.GetComponent<Collider>().enabled = true;
        animation = sword.GetComponent<Animation>();

        animation.AddClip(forwardSlash, forwardSlash.name);
        animation.Play(forwardSlash.name);

        Timer timer = BowoniaPool.instance.GetFromPool(PoolObject.TIMER).GetComponent<Timer>();
        timer.RunCountDown(forwardSlash.length, ResetSword, Timer.TimerType.DELAY);
    }

    public override void OnEquip()
    {
        base.OnEquip();
        sword = player.weapon.GetComponent<Sword>();
        sword.CacheComponents(damage, magnitude, this);
        sword.ToggleParticles(true);
        swordCollider = sword.GetComponent<Collider>();
    }

    public void ResetSword()
    {
        swordCollider.enabled = false;
        sword.ToggleParticles(false);
        sword.ResetDrained();
    }


    public override void UpgradeAttack()
    {
        base.UpgradeAttack();
        GameObject[] upgrades = sword.GetWeaponUpgrades();

        if(upgrades.Length > CurrentLevel)
        {
            for (int i = 0; i < upgrades.Length; i++)
            {
                upgrades[i].SetActive(false);

            }
            upgrades[CurrentLevel].SetActive(true);
        }

        
    }
}
