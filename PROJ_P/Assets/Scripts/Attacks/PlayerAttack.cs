using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : ScriptableObject
{

    [SerializeField] private float castCost;
    [SerializeField] protected float damage;
    [SerializeField] protected Sprite AttackImage;

    public virtual void Execute() {
        Debug.Log(name + " executed, " + Player.instance.Resource + " drained by: " + castCost);
        Player.instance.Resource.DrainResource(this);
        
    }

    public float GetCastCost() { return castCost; }

    public Sprite GetImage()
    {
        return AttackImage;
    }

    

}
