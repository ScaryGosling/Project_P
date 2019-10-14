using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrop : Pickup
{
    protected override void DoSomething()
    {
        player.healthProp += fillAmount;
        GameObject.Destroy(gameObject);
    }
}
