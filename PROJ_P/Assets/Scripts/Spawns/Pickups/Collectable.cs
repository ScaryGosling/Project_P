using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collectable : Pickup
{
    CollectionEvent collected;
    protected override void DoSomething()
    {
        base.DoSomething();
        collected = new CollectionEvent();
        collected.pickUpValue = Mathf.FloorToInt(fillAmount);
        EventSystem.Current.FireEvent(collected);
    }
}
