using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Friendly/Shop Base State")]
public class ShopBaseState : FriendlyBaseState
{
    protected Shop Owner;
    protected GameObject player;
    [SerializeField] protected float DistanceFromPlayerToActivate = 5f;
    private GameObject test;
    protected bool TimeLeft = true;
    private bool timerStarted = false;
    protected GameObject ShopTimer;
    protected GameObject ShopWindow;

    public override void InitializeState(StateMachine owner)
    {
        this.Owner = (Shop)owner;
        CacheComponents();
    }

    private void CacheComponents()
    {
        player = Owner.GetPlayer();
        test = Owner.GetText();
        ShopWindow = Owner.GetShopWindow();

    }

    public override void EnterState()
    {
        if (timerStarted == false)
        {
            ShopTimer = new GameObject("Timer");
            ShopTimer.AddComponent<Timer>().RunCountDown(Owner.GetShopTime(), RemoveShop);
            timerStarted = true;
        }
    }

    RaycastHit hit;
    Ray ray;
    public override void ToDo()
    {
        if (DistanceFromPlayer() <= DistanceFromPlayerToActivate && TimeLeft)
        {
            test.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                ActivateShop();
            }
            else if (Input.GetMouseButtonDown(0))
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.gameObject == Owner.gameObject)
                    {
                         ActivateShop();
                    }
                }
            }
        }
        else
        {
            test.SetActive(false);
        }
    }
    protected void RemoveShop()
    {
        Owner.ChangeState<ShopTimeFinishedState>();
    }
    private void ActivateShop()
    {
        Owner.ChangeState<ShopSellingState>();
    }
    protected float DistanceFromPlayer()
    {

        return Vector3.Distance(Owner.transform.position, player.transform.position);
    }

}
