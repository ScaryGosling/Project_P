﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrop : Pickup
{

    protected override void Start()
    {
        base.Start();
        DestroyThis();
    }
    protected override void DoSomething()
    {
        player.HealthProp = fillAmount;
    }
}
