using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : ScriptableObject
{
    [SerializeField] private string abilityName;

    public string GetAbilityName()
    {
        return abilityName;
    }
}
