﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : ScriptableObject
{

    [SerializeField] private float castCost;
    [SerializeField] protected List<float> damage;
    [SerializeField] protected Sprite AttackImage;
    [SerializeField] protected float cooldown;
    private GameObject timer;
    protected bool cooldownActive;
    [SerializeField] protected List<int> UpgradeCost = new List<int>();
    public int CurrentLevel { get; protected set; }

    public int GetNextLevelCost(int level)
    {
        return UpgradeCost[level];
    }

    public void UpgradeAttack()
    {
        CurrentLevel++;
    }
    public void ResetLevel()
    {
        CurrentLevel = 0;
    }

    public bool UpgradePossible()
    {
        return CurrentLevel+1 < UpgradeCost.Count;
    }

    public virtual void Execute() {

        if (!cooldownActive) {
            RunAttack();
            cooldownActive = true;

            timer = new GameObject("Timer");
            timer.AddComponent<Timer>().RunCountDown(cooldown, ResetCooldown);
        }
        
    }

    public void ResetCooldown() {


        cooldownActive = false;
        Destroy(timer);
    }

    public virtual void RunAttack() {


        Player.instance.Resource.DrainResource(this);
        

    }

    public float GetCastCost() { return castCost; }

    public Sprite GetImage()
    {
        return AttackImage;
    }

    public virtual void OnEquip() {


    }

    

}
