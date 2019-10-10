﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resource : ScriptableObject
{
    public int Value { get; protected set; } = 1;
    protected Image resourceImage;

    public virtual void DrainResource(PlayerAttack activeAttack) {

        resourceImage.fillAmount -= activeAttack.GetCastCost() / 100;

    }

    public virtual void CacheComponents(Image resourceImage)
    {
        this.resourceImage = resourceImage;

    }

    public virtual void IncreaseResource(float resource)
    {

        resourceImage.fillAmount += resource;
    }
}
