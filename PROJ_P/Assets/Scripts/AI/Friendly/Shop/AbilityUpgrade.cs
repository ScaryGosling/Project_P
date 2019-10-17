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
    [SerializeField]private PlayerAttack ability;
    

    void Start()
    {

    }
    void InstantiateRow()
    {
        // abilityImage = ability.Image
        // abilityName = ability.Name
    }

    void Update()
    {
        currentAbilityLevel = ability.CurrentLevel;
        currentAbilityLevelText.text = currentAbilityLevel + "";
        nextUpgradeCost.text = ability.GetNextLevelCost(currentAbilityLevel) + "$";


        abilityName.text = ability.ToString();
        abilityImage.sprite = ability.GetImage();

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //ability.ResetLevel();
        
        if (ability.UpgradePossible())
        {

        ability.UpgradeAttack();
        }
    }
}
