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

        if (animation.IsPlaying(forwardSlash.name))
        {
            animation.AddClip(backSlash, backSlash.name);
            animation.Play(backSlash.name);
        }
        else
        {
            animation.AddClip(forwardSlash, forwardSlash.name);
            animation.Play(forwardSlash.name);
        }

        Timer timer = new GameObject("Timer").AddComponent<Timer>();
        timer.RunCountDown(forwardSlash.length, ResetSword, Timer.TimerType.DELAY);
    }

    Player player;
    public override void OnEquip()
    {
        base.OnEquip();
        player = Player.instance;
        sword = player.weapon.GetComponent<Sword>();
        sword.CacheComponents(damage, magnitude, this);
    }
    public void DecreaseDurability()
    {
        durability.DrainResource(durabilityDecrease);
        durabilityText.text = ((int)(durability.Value * 100)).ToString();
    }
    public void IncreaseDurability(float increase)
    {
        durability.IncreaseResource(increase);
        durabilityText.text = ((int)(durability.Value * 100)).ToString();
    }
    public void ResetSword()
    {
        sword.GetComponent<Collider>().enabled = false; 
    }

    public void SetDurabilityTextObject(Text text)
    {
        durabilityText = text;
        durabilityText.text = ((int)(durability.Value * 100)).ToString();
    }

}
