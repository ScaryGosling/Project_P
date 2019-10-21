using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Friendly/Shop Base State")]
public class ShopBaseState : FriendlyBaseState
{
    protected new Shop owner;
    protected GameObject player;
    [SerializeField] protected float distanceFromPlayerToActivate = 5f;
    protected GameObject pressEtext;
    protected bool timeLeft = true;
    private bool timerStarted = false;
    protected GameObject shopWindow;
    protected Vector3 spawnPoint;
    protected NavMeshAgent navMeshAgent;

    public override void InitializeState(StateMachine owner)
    {
        this.owner = (Shop)owner;
        CacheComponents();
    }

    private void CacheComponents()
    {
        player = owner.GetPlayer();
        pressEtext = owner.GetText();
        shopWindow = owner.GetShopWindow();
        navMeshAgent = owner.GetComponent<NavMeshAgent>();
    }

    public override void EnterState()
    {
        timeLeft = true;

        Vector3 destination = Random.insideUnitSphere * 3;
        destination.y = 0;
        navMeshAgent.SetDestination(owner.transform.position + destination);
    }

    RaycastHit hit;
    Ray ray;
    public override void ToDo()
    {
        if (DistanceFromPlayer() <= distanceFromPlayerToActivate && timeLeft)
        {
            pressEtext.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                ActivateShop();
            }
            else if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("truw");
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {

                    if (hit.transform.gameObject == owner.gameObject)
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
        owner.ChangeState<ShopSellingState>();
    }
    protected float DistanceFromPlayer()
    {

        return Vector3.Distance(owner.transform.position, player.transform.position);
    }

}
