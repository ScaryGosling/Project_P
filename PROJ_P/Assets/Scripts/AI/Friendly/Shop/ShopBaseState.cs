using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    protected Vector3 SpawnPoint;
    protected NavMeshAgent NavMeshAgent;

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
        NavMeshAgent = Owner.GetComponent<NavMeshAgent>();
    }

    public override void EnterState()
    {
        TimeLeft = true;
        ShopTimer = new GameObject("Timer");
        ShopTimer.AddComponent<Timer>().RunCountDown(Owner.GetShopTime(), RemoveShop);
        Vector3 destination = Random.insideUnitSphere * 3;
        destination.y = 0;
        NavMeshAgent.SetDestination(Owner.transform.position + destination);
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
        test.SetActive(false);
        Player.instance.SetHover(false);
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
