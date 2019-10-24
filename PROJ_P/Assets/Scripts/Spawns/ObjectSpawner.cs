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
    private GameObject[] locations;
    private GameObject objectToSpawn;
    [SerializeField] private GameObject[] objectTypes;
    public ObjectToSpawn item { get; set; } = ObjectToSpawn.ManaPotion;
    public int Amount { get; set; } = 2;
    public bool hasAbsolutePosition { get; set; } = false;

    public void PopulateList()
    {
        Vector3 position;

        SelectType();

        if (!hasAbsolutePosition)
        {
            while (listOfObjects.Count < Amount && listOfObjects.Count < locations.Length)
            {
                foreach (GameObject location in locations)
                {
                    position = location.transform.position;
                    if (objectToSpawn != null && position != null && listOfObjects.Count < Amount)
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
                break;
            default:
                break;
        }
    }

    public void TerminateSpawner()
    {
        foreach (GameObject item in listOfObjects)
            Destroy(item.gameObject);
        Destroy(gameObject, 2f);
    }
}
