using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
[CreateAssetMenu(menuName = "Enemy/FatFoe")]
public class Pickup : MonoBehaviour
{

    [SerializeField] private float fillAmount;
    private BoxCollider colliderB;
    private GiveResource giveResource;
    private float distance;
    // Start is called before the first frame update
    void Start()
    {
        colliderB = gameObject.GetComponent<BoxCollider>();
        colliderB.isTrigger = true;
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DoSomething();
        }
    }

    protected virtual void DoSomething()
    {
        //görs generisk sen
        giveResource = new GiveResource();
        giveResource.fillAmount = fillAmount;
        EventSystem.Current.FireEvent(giveResource);
        Destroy(gameObject);
    }
}
