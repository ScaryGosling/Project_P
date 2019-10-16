using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mana : Resource
{
    public override void DrainResource(PlayerAttack activeAttack)
    {
        base.DrainResource(activeAttack);
    }

    public override void CacheComponents(Image resourceImage)
    {
        base.CacheComponents(resourceImage);
        resourceMeter = new Color(0,0,255,1);
        SetResourceColor();
    }

}
