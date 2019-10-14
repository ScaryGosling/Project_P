using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaDrop : Pickup
{
    
    protected override void DoSomething()
    {
        player.Resource.IncreaseResource(fillAmount);
        GameObject.Destroy(gameObject);
    }
}
