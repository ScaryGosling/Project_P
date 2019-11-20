using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityUI : MonoBehaviour
{
    public PlayerAttack attack;
    public Image icon;
    public TextMeshProUGUI nameText;
    private CharacterSelection cs;
    public void Start()
    {
        cs = CharacterSelection.instance;
    }

    public void SetDescription()
    {
        string description = attack.GetAbilityName() + "\n\n" + attack.GetAbilityDescriptionForClassChooser();
        cs.SetAbilityDescription(description);
    }

    public void ResetDescription() { cs.SetAbilityDescription(""); }
}
