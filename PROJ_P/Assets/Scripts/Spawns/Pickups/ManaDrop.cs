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
    /// <summary>
    /// This method is run when something is picked up, or collected. Performs completely different tasks depending on situation. 
    /// </summary>
    protected override void DoSomething()
    {
        player.Resource.IncreaseResource(fillAmount);
    }

    /// <summary>
    /// Changes the appearance of specific item.
    /// </summary>
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
