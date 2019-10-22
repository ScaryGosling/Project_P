using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : ScriptableObject
{

    [SerializeField] private float castCost;
    [SerializeField] protected float damage;
    [SerializeField] protected Sprite attackImage;
    [SerializeField] protected float cooldown;
    private GameObject timer;
    protected bool cooldownActive;
    [SerializeField] protected List<int> upgradeCost = new List<int>();
    public int CurrentLevel { get; protected set; }
    [SerializeField] protected List<UpgradeCost> upgradeCosts = new List<UpgradeCost>();



    [System.Serializable]
    protected struct UpgradeCost
    {
        public string levelName;
        public int upgradeCost;
        public int newDamage;
    }


    public int GetNextLevelCost(int level)
    {
        return upgradeCosts[level].upgradeCost;
    }

    public void UpgradeAttack()
    {
        damage = upgradeCosts[CurrentLevel].newDamage;
        CurrentLevel++;

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
            RunAttack();
            cooldownActive = true;

            timer = new GameObject("Timer");
            timer.AddComponent<Timer>().RunCountDown(cooldown, ResetCooldown);
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
