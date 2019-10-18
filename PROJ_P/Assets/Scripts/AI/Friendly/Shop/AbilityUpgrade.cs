using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityUpgrade : MonoBehaviour , IPointerClickHandler
{
    [SerializeField] private Image abilityImage;
    [SerializeField] private Text abilityName;
    [SerializeField] private Text nextUpgradeCost;
    private int currentAbilityLevel;
    [SerializeField] private Text currentAbilityLevelText;
    [SerializeField] private PlayerAttack ability;
    

    public void EmptyRow()
    {
        abilityImage.sprite = null;
        abilityName.text = "";
        nextUpgradeCost.text = "-";
        currentAbilityLevelText.text = "-";
        ability = null;
    }

    public void SetElements(PlayerAttack ability)
    {
        this.ability = ability;
        InstantiateRow();
    }
    void InstantiateRow()
    {
        abilityName.text = ability.ToString();
        abilityImage.sprite = ability.GetImage();
        ability.ResetLevel();
    }

    void Update()
    {
        if (ability != null)
        {
            currentAbilityLevel = ability.CurrentLevel;
            currentAbilityLevelText.text = currentAbilityLevel + "";
            nextUpgradeCost.text = ability.GetNextLevelCost(currentAbilityLevel) + "$";
        }





    }
    int nextLevelCost;
    public void OnPointerClick(PointerEventData eventData)
    {
        nextLevelCost = ability.GetNextLevelCost(currentAbilityLevel);
        if (ability != null && ability.UpgradePossible())
        {
            if (Player.instance.GoldProp >= nextLevelCost)
            {
                ability.UpgradeAttack();
                Player.instance.GoldProp -= nextLevelCost;
            }
        }
    }
}
