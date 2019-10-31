using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Attacks/Warrior/Hack")]
public class MeleeHack : PlayerAttack
{
    [Header("Ability Specific")]
    [SerializeField] private AnimationClip slash;
    private Animation animation;
    private Sword sword;
    private Resource durability;
    [SerializeField] private int durabilityDecrease = 1;
    [SerializeField] private Text durabilityText;
    [SerializeField] [Range(0, 1)] [Tooltip ("Percentage of damage to be applied when durability is 0")] 
    private float zeroDurabilityDamagePercentage = 0.2f;

    public override void OnEnable()
    {
        base.OnEnable();
        durability = CreateInstance<Resource>();
    }
    public override void Execute()
    {
        if (durability.Value == 0)
        {
            sword.CacheComponents(damage * zeroDurabilityDamagePercentage, this);
        }
        else
        {
            sword.CacheComponents(damage, this);
        }
        base.Execute();
    }

    public override void RunAttack()
    {
        sword.GetComponent<Collider>().enabled = true;
        animation.Play("Slash");
        Timer timer = new GameObject("Timer").AddComponent<Timer>();
        timer.RunCountDown(slash.length, ResetSword, Timer.TimerType.DELAY);
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
    public void DecreaseDurability()
    {
        durability.DrainResource(durabilityDecrease);
        durabilityText.text = (durability.Value * 100).ToString();
    }
    public void IncreaseDurability(float increase)
    {
        Debug.Log(increase);
        durability.IncreaseResource(increase);
        durabilityText.text = (durability.Value * 100).ToString();
        Debug.Log(durability.Value);
    }
    public void ResetSword()
    {
        sword.GetComponent<Collider>().enabled = false; 
    }

    public void SetDurabilityTextObject(Text text)
    {
        durabilityText = text;
        durabilityText.text = (durability.Value * 100).ToString();
    }

}
