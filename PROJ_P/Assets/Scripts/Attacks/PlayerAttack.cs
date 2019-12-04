using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : Ability
{
    [Header("Info")]
    [SerializeField] [TextArea] [Tooltip("This description is for class chooser only")] private string abilityDescriptionForClassChooser = "No description available";
    [SerializeField] private float castCost;
    public float castTime;
    protected float damage;
    [SerializeField] protected float magnitude;
    [SerializeField] protected Sprite attackImage;
    [SerializeField] protected float cooldown;
    public bool cooldownActive;
    public int CurrentLevel { get; protected set; }
    [Tooltip("Element 0 == Unlock cost & default damage, the rest are upgrades")]
    [SerializeField] protected List<UpgradeCost> upgradeCosts = new List<UpgradeCost>();
    [SerializeField] private bool lockedAbility = false;

    [Header("Effects")]
    [SerializeField] protected GameObject particles;
    [SerializeField] protected AudioClip sound;
    [SerializeField] protected AudioClip cooldownClip;
    [SerializeField] protected AudioClip chargeUpClip;



    public AudioClip GetChargeUpSound() { return chargeUpClip; }
    public float GetCooldown() { return cooldown; }
    public bool GetCooldownActive() { return cooldownActive; }
    [SerializeField] private AbilityCat abilityCat;
    public AbilityCat AbilityCatProp { get; private set; }
    private int startLevel = 0;
    protected Player player = Player.instance;



    [Header("Movement Slow")]
    [Tooltip("The amount of seconds to slow player")]
    [SerializeField] private float slowTime;
    [Tooltip("Multiplies the player speed, 0-1 for slow effects")]
    [SerializeField] private float speedMultiplier;

    public float GetSpeedMultiplier() { return speedMultiplier; }
    public float GetSlowTime() { return slowTime; }

    [System.Serializable]
    protected struct UpgradeCost
    {
        public int upgradeCost;
        public int newDamage;
    }

    public virtual void OnEnable()
    {
        AbilityCatProp = abilityCat;
        damage = upgradeCosts[0].newDamage;
        if (lockedAbility == true)
        {
            CurrentLevel = -1;
            startLevel = CurrentLevel;
        }
        try
        {

            damage = upgradeCosts[0].newDamage;

        }
        catch (Exception e) { }
    }


    public bool IsLocked()
    {
        return lockedAbility;
    }
    public int GetNextLevelCost(int level)
    {
        if (upgradeCosts.Count == 0) //if upgradeCost List == null
        {
            return -2;
        }
        else if (upgradeCosts.Count == level + 1) //if maxlevel
        {
            return -2;
        }
        return upgradeCosts[level + 1].upgradeCost;
    }

    public virtual void UpgradeAttack()
    {
        CurrentLevel++;
        damage = upgradeCosts[CurrentLevel].newDamage;
        if (lockedAbility)
        {
            lockedAbility = false;
        }
    }
    public void ResetLevel()
    {
        CurrentLevel = 0;
    }

    public bool UpgradePossible()
    {
        return CurrentLevel + 1 < upgradeCosts.Count;
    }

    public virtual void Execute()
    {

        if (player.Audio != null && sound != null && player.GetSettings().UseSFX)
        {
            player.PlayAudio(sound);

        }

        RunAttack();
        player.RunAttackCooldown(this);
    }

    public string GetAbilityDescriptionForClassChooser()
    {
        return abilityDescriptionForClassChooser;
    }

    public void ResetSlow()
    {
        player.ResetSpeed();
    }

    public void ResetCooldown()
    {
        cooldownActive = false;
    }

    public virtual void RunAttack()
    {


        player.Resource.DrainResource(this);


    }

    public float GetCastCost() { return castCost; }

    public Sprite GetImage()
    {
        return attackImage;
    }

    public virtual void OnEquip()
    {
        player = Player.instance;

    }



}
public enum AbilityCat
{
    POTION, OFFENSIVE, DEFENSIVE, UTILITY
}