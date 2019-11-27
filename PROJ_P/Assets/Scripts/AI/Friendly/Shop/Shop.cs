using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : StateMachine
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject text;
    [SerializeField] private GameObject shopWindow;
    [SerializeField] private float shopTime = 40;
    private float activeShopTime;
    [SerializeField] private int costOfPotion;
    private GameObject shopTimer;
    private ToggleArrowEvent toggleArrow = new ToggleArrowEvent();
    [SerializeField] private GameObject timerText;
    [SerializeField] private Color32 arrowColor = Color.white;
    public Vector3 spawnPoint { get; private set; }
    [SerializeField] private float distanceFromPlayerToActivate = 10f;
    public float DistanceFromPlayerToActivate { get { return distanceFromPlayerToActivate; } private set { distanceFromPlayerToActivate = value; } }
    private bool infiniteTimeUsed;
    private float tutorialTime = 600;
    [SerializeField] private Texture2D shopHand;
    private void Start()
    {
        spawnPoint = transform.position;
    }
    private void OnEnable()
    {
        spawnPoint = transform.position;
        ChangeState<ShopBaseState>();
        if (infiniteTimeUsed == false)
        {
            activeShopTime = tutorialTime;
            infiniteTimeUsed = true;
        }
        else if (Player.instance.GetSettings().UseExtraShopTime)
        {
            activeShopTime = shopTime * 2;
        }
        else
        {
            activeShopTime = shopTime; 
        }
        shopTimer = BowoniaPool.instance.GetFromPool(PoolObject.TIMER);
        shopTimer.GetComponent<Timer>().RunCountDown(activeShopTime, RemoveShop, Timer.TimerType.DELAY);
        timerText.gameObject.SetActive(true);
        timerText.GetComponent<ShopTimer>().SetTimer(shopTimer.GetComponent<Timer>());
        toggleArrow.goal = gameObject;
        toggleArrow.toggle = true;
        toggleArrow.arrowColor = arrowColor;
        EventSystem.Current.FireEvent(toggleArrow);
    }
    private void OnDisable()
    {

        try
        {
            toggleArrow.toggle = false;

            EventSystem.Current.FireEvent(toggleArrow);

        }
        catch (Exception e) { }
        shopTimer = null;

    }
    public void RemoveShop()
    {
        if (shopWindow.activeSelf)
        {
            shopWindow.SetActive(false);
        }
        timerText.SetActive(false);
        //Destroy(shopTimer);
        ChangeState<ShopTimeFinishedState>();
    }

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
        return activeShopTime;
    }
    public Texture2D GetCursor()
    {
        return shopHand;
    }
    public void RefillPotions()
    {
        if (Player.instance.GoldProp >= costOfPotion)
        {
            Player.instance.Resource.IncreaseResource(1f);
            Player.instance.HealthProp = 100;
            Player.instance.GoldProp -= 10;
        }

    }


}