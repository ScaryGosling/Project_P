using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateShopMenu : MonoBehaviour
{
    [SerializeField] private GameObject abilityRows;
    [SerializeField] private Player player;
    [SerializeField] private GameObject abilityRow;
    [SerializeField] private GameObject[] rows;
    private bool rowsCreated = false;


    private void OnEnable()
    {
        if (rowsCreated == false)
        {
            CreateAbilityRows();
            rowsCreated = true;
        }
    }
    private void CreateAbilityRows()
    {
        AbilityUpgrade abilityUpgrade;
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
