using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : ScriptableObject
{

    [SerializeField] private float castCost;
    [SerializeField] protected float damage;

    public virtual void Execute(Transform spawnPoint) {
    }

    public float GetCastCost() { return castCost; }


}
