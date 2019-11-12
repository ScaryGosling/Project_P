using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rage : Resource
{
    private float rageGeneration = 0.07f;
    private float drainDelay = 3;

    private float drainSpeed = 4;
    private Player player;
    
    public override void DrainResource(PlayerAttack activeAttack)
    {
        base.DrainResource(activeAttack);

        if (activeAttack is MeleeHack)
        {
            IncreaseResource(rageGeneration);
            ((MeleeHack)activeAttack).DecreaseDurability();
            if(player.RageTap != null)
                player.StopCoroutine(player.RageTap);
            player.RageTap = player.StartCoroutine(TapRage());
        }
    }

    public IEnumerator TapRage()
    {
        yield return new WaitForSeconds(drainDelay);

        while (Value > 0)
        {
            DrainResource(drainSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public override void CacheComponents(Image resourceImage) {

        name = "Rage";
        base.CacheComponents(resourceImage);
        resourceMeter = new Color(200, 0, 0, 1);
        resourceImage.fillAmount = 0;
        Value = 0;
        SetResourceColor();
        player = Player.instance;


    }
   

    
  
}
