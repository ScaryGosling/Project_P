using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : ScriptableObject
{
    [SerializeField] private string abilityName;
    [SerializeField] private string abilityDescription = "No description available";

    public string GetAbilityName()
    {
        return abilityName;
    }
    public string GetAbilityDescription()
    {
        return abilityDescription;
    }
}
