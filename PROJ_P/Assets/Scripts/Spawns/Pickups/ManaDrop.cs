//Author: Emil Dahl
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaDrop : Pickup
{
    [SerializeField] private Material[] materials;
    [SerializeField]
    protected override void Start()
    {
        base.Start();
        SetMaterials();
    }
    protected override void DoSomething()
    {
        if (player.GetComponent<Player>().playerClass == PlayerClass.WIZARD)
        {
            player.Resource.IncreaseResource(fillAmount);

        }
        else
        {
            ((MeleeHack)(player.attackSet.list[0])).IncreaseDurability(fillAmount);
        }
    }

    private void SetMaterials()
    {
        if (player.GetComponent<Player>().playerClass == PlayerClass.WIZARD)
        {
            gameObject.GetComponent<MeshRenderer>().material = materials[0];
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().material = materials[1];
        }

    }
}
