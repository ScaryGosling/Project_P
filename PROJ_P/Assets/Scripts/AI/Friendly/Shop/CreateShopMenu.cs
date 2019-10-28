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
    [SerializeField]private GameObject offensivesContent; 
    [SerializeField]private GameObject defensivesContent; 
    [SerializeField]private GameObject utilitiesContent; 
    [SerializeField]private GameObject potionsContent; 

    private void OnEnable()
    {
        if (rowsCreated == false)
        {
            //CreateAbilityRows();
            CreateColumns();
            rowsCreated = true;
        }
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
            
            switch (abilityUpgrade.GetAbilityCat()) 
            {
                case AbilityCat.OFFENSIVE:
                    abilityUpgrade.transform.parent = offensivesContent.transform;
                    break;
                case AbilityCat.DEFENSIVE:
                    abilityUpgrade.transform.parent = defensivesContent.transform;
                    break;
                case AbilityCat.UTILITY:
                    abilityUpgrade.transform.parent = utilitiesContent.transform;
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
                abilityUpgrade.transform.parent = potionsContent.transform;
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
