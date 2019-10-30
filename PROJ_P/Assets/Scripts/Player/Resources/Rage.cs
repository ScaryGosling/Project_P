using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rage : Resource
{
    private float rageGeneration = 0.05f;
    
    public override void DrainResource(PlayerAttack activeAttack)
    {
        base.DrainResource(activeAttack);

        if (activeAttack is MeleeHack)
        {
            IncreaseResource(rageGeneration);
            ((MeleeHack)activeAttack).DecreaseDurability();
        }
    }

    public override void CacheComponents(Image resourceImage) {

        base.CacheComponents(resourceImage);
        resourceMeter = new Color(200, 200, 0, 1);
        resourceImage.fillAmount = 0;
        Value = 0;
        SetResourceColor();


    }
   

    
  
}
