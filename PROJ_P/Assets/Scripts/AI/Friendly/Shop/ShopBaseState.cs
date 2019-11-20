using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Friendly/Shop Base State")]
public class ShopBaseState : FriendlyBaseState
{
    protected Shop shopKeeper;
    protected GameObject player;
    protected GameObject pressEGameObject;
    protected bool timeLeft = true;
    private bool timerStarted = false;
    protected GameObject shopWindow;
    protected Vector3 spawnPoint;
    protected NavMeshAgent navMeshAgent;
    private Text pressETextObject;

    public override void InitializeState(StateMachine owner)
    {
        this.shopKeeper = (Shop)owner;
        CacheComponents();
    }

    private void CacheComponents()
    {
        player = shopKeeper.GetPlayer();
        pressEGameObject = shopKeeper.GetText();
        shopWindow = shopKeeper.GetShopWindow();
        navMeshAgent = shopKeeper.GetComponent<NavMeshAgent>();
        pressETextObject = pressEGameObject.GetComponent<Text>();
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
            pressEGameObject.SetActive(true);
            pressETextObject.text = "Press " + Player.instance.GetSettings().GetBindString(KeyFeature.TOGGLE_SHOP);
            if (Input.GetKeyDown(Player.instance.GetSettings().GetBind(KeyFeature.TOGGLE_SHOP)))
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
            pressEGameObject.SetActive(false);
        }
    }

    private void ActivateShop()
    {
        if (!shopWindow.activeSelf)
        {
            pressEGameObject.SetActive(false);
            Cursor.SetCursor(shopKeeper.GetCursor(), Vector2.zero, CursorMode.Auto);
            shopKeeper.ChangeState<ShopSellingState>();
        }
    }
    protected float DistanceFromPlayer()
    {

        return Vector3.Distance(shopKeeper.transform.position, player.transform.position);
    }

}
