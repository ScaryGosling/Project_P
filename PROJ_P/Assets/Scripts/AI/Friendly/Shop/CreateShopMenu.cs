using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateShopMenu : MonoBehaviour
{
    [SerializeField] private GameObject abilityRows;
    [SerializeField] private Player player;
    [SerializeField] private GameObject abilityRow;
    [SerializeField] private GameObject[] rows;
    private bool rowsCreated = false;
    [SerializeField] private GameObject offensivesContent; 
    [SerializeField] private GameObject defensivesContent; 
    [SerializeField] private GameObject utilitiesContent; 
    [SerializeField] private GameObject potionsContent;
    [SerializeField] private Text tooltipText;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Text hotkey2, hotkey3, hotkey4;

    private void OnEnable()
    {
        if (rowsCreated == false)
        {
            //CreateAbilityRows();
            CreateColumns();
            rowsCreated = true;
        }
        hotkey2.text = Player.instance.GetSettings().GetBindString(KeyFeature.ABILITY_1);
        hotkey3.text = Player.instance.GetSettings().GetBindString(KeyFeature.ABILITY_2);
        hotkey4.text = Player.instance.GetSettings().GetBindString(KeyFeature.ABILITY_3);
        Player.instance.SetHover(true);
    }

    private void OnDisable()
    {
        
        Player.instance.SetHover(false);
    }

    private void CreateColumns()
    {
        for (int i = 0; i < player.attackSet.list.Length; i++)
        {
            abilityUpgrade = Instantiate(abilityRow).GetComponent<AbilityUpgrade>();
            abilityUpgrade.SetElements(player.attackSet.list[i]);
            abilityUpgrade.SetTooltip(tooltipText);

            switch (abilityUpgrade.GetAbilityCat()) 
            {
                case AbilityCat.OFFENSIVE:
                    abilityUpgrade.transform.SetParent(offensivesContent.transform);
                    break;
                case AbilityCat.DEFENSIVE:
                    abilityUpgrade.transform.SetParent(defensivesContent.transform);
                    break;
                case AbilityCat.UTILITY:
                    abilityUpgrade.transform.SetParent(utilitiesContent.transform);
                    break;
            }
            abilityUpgrade.transform.localScale = Vector3.one;
        }
        if (player.attackSet.potionList.Length != 0)
        {
            for (int i = 0; i < player.attackSet.potionList.Length; i++)
            { 
                abilityUpgrade = Instantiate(abilityRow).GetComponent<AbilityUpgrade>();
                abilityUpgrade.SetPotion(player.attackSet.potionList[i]);       
                abilityUpgrade.SetTooltip(tooltipText);
                abilityUpgrade.transform.SetParent(potionsContent.transform);
                abilityUpgrade.transform.localScale = Vector3.one;
            }
        }


    }
    private AbilityUpgrade abilityUpgrade;
    private void CreateAbilityRows()
    {
        for (int i = 0; i < rows.Length; i++)
        {
            abilityUpgrade = Instantiate(abilityRow, rows[i].transform).GetComponent<AbilityUpgrade>();

            if (player.attackSet.list[i] != null)
            {
                abilityUpgrade.SetElements(player.attackSet.list[i]);
            }
            else
            {
                abilityUpgrade.EmptyRow();

            }

        }
    }
}
