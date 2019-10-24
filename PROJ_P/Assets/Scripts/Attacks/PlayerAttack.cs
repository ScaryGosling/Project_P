using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : ScriptableObject
{

    [SerializeField] private float castCost;
    protected float damage;
    [SerializeField] protected Sprite attackImage;
    [SerializeField] protected float cooldown;
    private GameObject timer;
    protected bool cooldownActive;
    public int CurrentLevel { get; protected set; }
    [Tooltip("Element 0 == Unlock cost & default damage, the rest are upgrades")]
    [SerializeField] protected List<UpgradeCost> upgradeCosts = new List<UpgradeCost>();
    [SerializeField] private bool lockedAbility = false;

    public float GetCooldown() { return cooldown; }
    public bool GetCooldownActive() { return cooldownActive; }
    [SerializeField] private AbilityCat abilityCat;
    public AbilityCat AbilityCatProp { get; private set; }
    private int startLevel = 0;

    public void OnEnable()
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

        } catch(Exception e) { }
    }

    [System.Serializable]
    protected struct UpgradeCost
    {
        public int upgradeCost;
        public int newDamage;
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

    public void UpgradeAttack()
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

        if (!cooldownActive)
        {
            Player.instance.RunAttackCooldown();
            RunAttack();
            cooldownActive = true;
            timer = new GameObject("Timer");
            timer.AddComponent<Timer>().RunCountDown(cooldown / Player.instance.activeStats.attackSpeed, ResetCooldown);
        }

    }

    public void ResetCooldown()
    {
        cooldownActive = false;
        Destroy(timer);
    }

    public virtual void RunAttack()
    {

        
        Player.instance.Resource.DrainResource(this);


    }

    public float GetCastCost() { return castCost; }

    public Sprite GetImage()
    {
        return attackImage;
    }

    public virtual void OnEquip()
    {


    }



}
public enum AbilityCat
{
    POTION, OFFENSIVE, DEFENSIVE, UTILITY
}