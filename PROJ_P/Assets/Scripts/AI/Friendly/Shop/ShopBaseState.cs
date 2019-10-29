using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Friendly/Shop Base State")]
public class ShopBaseState : FriendlyBaseState
{
    protected Shop shopKeeper;
    protected GameObject player;
    protected GameObject pressEtext;
    protected bool timeLeft = true;
    private bool timerStarted = false;
    protected GameObject shopWindow;
    protected Vector3 spawnPoint;
    protected NavMeshAgent navMeshAgent;

    public override void InitializeState(StateMachine owner)
    {
        this.shopKeeper = (Shop)owner;
        CacheComponents();
    }

    private void CacheComponents()
    {
        player = shopKeeper.GetPlayer();
        pressEtext = shopKeeper.GetText();
        shopWindow = shopKeeper.GetShopWindow();
        navMeshAgent = shopKeeper.GetComponent<NavMeshAgent>();
    }

    public override void EnterState()
    {
        timeLeft = true;

        Vector3 destination = Random.insideUnitSphere * 3;
        destination.y = 0;
        navMeshAgent.SetDestination(shopKeeper.transform.position + destination);
    }

    RaycastHit hit;
    Ray ray;
    public override void ToDo()
    {
        if (DistanceFromPlayer() <= shopKeeper.DistanceFromPlayerToActivate && timeLeft)
        {
            pressEtext.SetActive(true);
            if (Input.GetKeyDown(Player.instance.GetKeybindSet().GetBind(KeyFeature.TOGGLE_SHOP)))
            {
                ActivateShop();
            }
            else if (Input.GetMouseButtonDown(0))
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {

                    if (hit.transform.gameObject == shopKeeper.gameObject)
                    {
                         ActivateShop();
                    }
                }
            }
        }
        else
        {
            pressEtext.SetActive(false);
        }
    }

    private void ActivateShop()
    {
        if (!shopWindow.activeSelf)
        {
            pressEtext.SetActive(false);
        shopKeeper.ChangeState<ShopSellingState>();
        }
    }
    protected float DistanceFromPlayer()
    {

        return Vector3.Distance(shopKeeper.transform.position, player.transform.position);
    }

}
