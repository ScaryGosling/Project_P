//Author: Emil Dahl
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaDrop : Pickup
{
    [SerializeField] private Material[] materials;
    protected override void Start()
    {
        base.Start();
        SetMaterials();
    }
    protected override void DoSomething()
    {
        player.Resource.IncreaseResource(fillAmount);
        GameObject.Destroy(gameObject);
    }

    private void SetMaterials()
    {
        if (player.GetComponent<Player>().playerClass == PlayerClass.WIZARD)
            gameObject.GetComponent<MeshRenderer>().material = materials[0];
        else
            gameObject.GetComponent<MeshRenderer>().material = materials[1];
    }
}
