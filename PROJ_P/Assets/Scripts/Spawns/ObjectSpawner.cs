//Author: Emil Dahl
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(GenericTimer))]
public class ObjectSpawner : MonoBehaviour
{
    [HideInInspector] public enum ObjectToSpawn { ManaPotion, HealthPotion, Reward }
    LinkedList<GameObject> listOfObjects = new LinkedList<GameObject>();
    public GameObject[] locations { get; private set; }

    private GameObject objectToSpawn;
    [SerializeField] private GameObject[] objectTypes;
    private ObjectToSpawn item = ObjectToSpawn.ManaPotion;
    public bool hasAbsolutePosition { get; set; } = false;
    private int amount = 2;
    private Vector3 position;

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
                    if (objectToSpawn != null && position != null && listOfObjects.Count < amount)
                        listOfObjects.AddFirst(Instantiate(objectToSpawn, position, Quaternion.identity));
                }
            }
        }
        else
        {
            position = locations[0].gameObject.transform.position;
            Instantiate(objectToSpawn, position, Quaternion.identity);
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
                objectToSpawn = objectTypes[0];
                break;
            case ObjectToSpawn.HealthPotion:
                objectToSpawn = objectTypes[1];
                break;
            case ObjectToSpawn.Reward:
                objectToSpawn = objectTypes[2];
                break;
            default:
                break;
        }
    }

    public void TerminateSpawner()
    {
        foreach (GameObject i in listOfObjects)
            Destroy(i.gameObject);
        Destroy(gameObject, 2f);
    }
}
