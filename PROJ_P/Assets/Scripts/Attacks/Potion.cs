using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Potion : ScriptableObject
{
    
    [SerializeField] protected Sprite potionImage;
    public AbilityCat AbilityCatProp { get; private set; }
    private void OnEnable()
    {
        AbilityCatProp = AbilityCat.POTION;
    }
    public Sprite GetImage()
    {
        return potionImage;
    }
    public abstract void BuyPotion(int cost);
    public abstract int GetPotionCost();
}
