using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Attacks/Warrior/LifeLeech")]
public class LifeLeech : PlayerAttack
{
    [Header("Ability Specific")]
    [SerializeField] private AnimationClip forwardSlash;
    [SerializeField] private AnimationClip backSlash;
    [SerializeField] private int iterations = 5;
    [SerializeField] private float regenerationValue = 1;
    [SerializeField] private float duration = 1;
    private Animation animation;
    private Sword sword;
    [SerializeField] private List<float> regenerationPerLevel = new List<float>();
    private GameObject leechParticles;
    private Timer timer;

    public override void OnEnable()
    {
        base.OnEnable();
    }
    public override void Execute()
    {
        base.Execute();
    }
    public override void UpgradeAttack()
    {
        base.UpgradeAttack();
        regenerationValue = regenerationPerLevel[CurrentLevel];
    }

    public override void RunAttack()
    {
        sword.GetComponent<Collider>().enabled = true;
        
    }

    public override void OnEquip()
    {
        base.OnEquip();
        sword = player.weapon.GetComponent<Sword>();
        sword.ActivateLifeLeech(iterations, regenerationValue);
        timer = BowoniaPool.instance.GetFromPool(PoolObject.TIMER).GetComponent<Timer>();
        timer.RunCountDown(duration, ResetSword, Timer.TimerType.DELAY);
    }


    public void ResetSword()
    {
        sword.DeactivateLifeleech();
        BowoniaPool.instance.AddToPool(PoolObject.TIMER, timer.gameObject);
    }


}
