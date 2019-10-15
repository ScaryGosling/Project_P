using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : StateMachine
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject text;
    [SerializeField] private GameObject shopWindow;
    [SerializeField] private float shopTime = 40;
    [SerializeField] private int costOfPotion;

    public GameObject GetShopWindow()
    {
        return shopWindow;
    }
    public GameObject GetPlayer()
    {
        return player;
    }
    public GameObject GetText()
    {
        return text;
    }
    public float GetShopTime()
    {
        return shopTime;
    }

    public void RefillPotions()
    {
        if (Player.instance.GoldProp >= costOfPotion)
        {
            Player.instance.Resource.IncreaseResource(1f);
            Player.instance.healthProp += 1;
            Player.instance.GoldProp -= 10;
        }

    }

}
