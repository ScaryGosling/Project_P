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
    [SerializeField] private GameObject[] objectTypes;
    [SerializeField] private ObjectToSpawn item = ObjectToSpawn.ManaPotion;
    [SerializeField] private int amount = 2;

    void Start()
    {

        locations = GameObject.FindGameObjectsWithTag("ItemSpawners");

        switch (item)
        {
            case ObjectToSpawn.ManaPotion:
                PopulateList(objectTypes[0]);
                break;
            case ObjectToSpawn.HealthPotion:
                PopulateList(objectTypes[1]);
                break;
            case ObjectToSpawn.Reward:
                break;
            default:
                break;
        }
    }

    public void PopulateList(GameObject type)
    {
        Vector3 position;

        while (listOfObjects.Count < amount && listOfObjects.Count < locations.Length)
        {
            foreach (GameObject location in locations)
            {
                position = location.transform.position;
                listOfObjects.AddFirst(Instantiate(type, position, Quaternion.identity));
            }
        }
    }

    public void TerminateSpawner()
    {
        foreach (GameObject item in listOfObjects)
            Destroy(item.gameObject);
        Destroy(gameObject, 2f);
    }
}
