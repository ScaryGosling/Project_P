﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : ScriptableObject
{
    [SerializeField] private string abilityName;
    [SerializeField] [TextArea][Tooltip("This description is for shop only")] private string abilityDescription = "No description available";
    [SerializeField] [TextArea] [Tooltip("This description is for class chooser only")] private string abilityDescriptionForClassChooser = "No description available";

    public string GetAbilityName()
    {
        return abilityName;
    }
    public string GetAbilityDescription()
    {
        return abilityDescription;
    }
    public string GetAbilityDescriptionForClassChooser()
    {
        return abilityDescriptionForClassChooser;
    }
}
