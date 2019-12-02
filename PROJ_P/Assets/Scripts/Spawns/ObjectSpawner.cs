//Author: Emil Dahl
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// This script takes care of object spawning around the map. This includes everything from potions to quest items. 
/// </summary>
[RequireComponent(typeof(GenericTimer))]
public class ObjectSpawner : MonoBehaviour
{
    [HideInInspector] public enum ObjectToSpawn { ManaPotion, HealthPotion, Reward }
    LinkedList<GameObject> listOfObjects = new LinkedList<GameObject>();
    public GameObject[] locations { get; private set; }

    private GameObject genericObject;
    [SerializeField] private GameObject[] objectTypes;
    private ObjectToSpawn item = ObjectToSpawn.ManaPotion;
    public bool hasAbsolutePosition { get; set; } = false;
    private int amount = 2;
    private Vector3 position;
    private PoolObject poolObject;

    /// <summary>
    /// Creates said amount of items, then positions them around the world. 
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="item"></param>
    public void PopulateList(int amount, ObjectToSpawn item)
    {
        this.amount = amount;
        this.item = item;

        SelectType();

        if (!hasAbsolutePosition)
        {
            while (listOfObjects.Count < amount && listOfObjects.Count < locations.Length)
            {
                foreach (GameObject location in locations)
                {
                    position = location.transform.position;
                    if (genericObject != null && position != null && listOfObjects.Count < amount)
                    {
                        genericObject = BowoniaPool.instance.GetFromPool(poolObject);
                        genericObject.transform.position = position;
                        genericObject.transform.rotation = Quaternion.identity;
                        //listOfObjects.AddFirst(Instantiate(genericObject, position, Quaternion.identity));
                        listOfObjects.AddFirst(genericObject);
                    }

                }
            }
        }
        else
        {
            position = locations[0].gameObject.transform.position;
            genericObject = BowoniaPool.instance.GetFromPool(poolObject);
            genericObject.transform.position = position;
            genericObject.transform.rotation = Quaternion.identity;
            //Instantiate(genericObject, position, Quaternion.identity);
        }
    }

    private void SelectType()
    {

        if (item == ObjectToSpawn.ManaPotion)
            locations = GameObject.FindGameObjectsWithTag("ItemSpawners");
        else
            locations = GameObject.FindGameObjectsWithTag("SpecialPositions");

        switch (item)
        {
            case ObjectToSpawn.ManaPotion:
                poolObject = PoolObject.MANA_POTION;
                genericObject = objectTypes[0];
                break;
            case ObjectToSpawn.HealthPotion:
                poolObject = PoolObject.HEALTH_DROP;
                genericObject = objectTypes[1];
                break;
            case ObjectToSpawn.Reward:
                poolObject = PoolObject.QUEST_COLLECTABLE;
                genericObject = objectTypes[2];
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Removes all items when they are no longer needed. 
    /// </summary>
    public void TerminateSpawner()
    {
        foreach (GameObject i in listOfObjects)
        {
            if (i.activeInHierarchy)
            {
            BowoniaPool.instance.AddToPool(poolObject, i);

            }
        }
        listOfObjects.Clear();
        BowoniaPool.instance.AddToPool(PoolObject.OBJECT_SPAWNER, gameObject, 2f);
    }
}
