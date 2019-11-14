//Author: Emil Dahl
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaDrop : Pickup
{
    [SerializeField] private Material[] materials;
    [SerializeField] private MeshRenderer meshRenderer;
        Material[] tempMaterial;
    
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
        tempMaterial = meshRenderer.materials;
        if (player.playerClass == PlayerClass.WIZARD)
        {
            tempMaterial[1] = materials[0];
            meshRenderer.materials = tempMaterial;
        }
        else
        {
            tempMaterial[1] = materials[1];
            meshRenderer.materials = tempMaterial;
        }
    }

    private void SetParticles(){ }
}
