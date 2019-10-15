using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : ScriptableObject
{

    [SerializeField] private float castCost;
    [SerializeField] protected float damage;
    [SerializeField] protected Sprite AttackImage;
    [SerializeField] protected float cooldown;
    private GameObject timer;
    protected bool cooldownActive;

    public virtual void Execute() {

        if (!cooldownActive) {
            RunAttack();
            cooldownActive = true;

            timer = new GameObject("Timer");
            timer.AddComponent<Timer>().RunCountDown(cooldown, ResetCooldown);
        }
        
    }

    public void ResetCooldown() {

        Debug.Log("Cooldown reset");
        cooldownActive = false;
        Destroy(timer);
    }

    public virtual void RunAttack() {

        Debug.Log(name + " executed, " + Player.instance.Resource + " drained by: " + castCost);
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
