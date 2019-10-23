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
    public GameObject type;
    [SerializeField] private GameObject[] objectTypes;
    public ObjectToSpawn item { get; set; } = ObjectToSpawn.ManaPotion;
    public int amount { get; set; } = 2;

    public void PopulateList()
    {
        Vector3 position;
        locations = GameObject.FindGameObjectsWithTag("ItemSpawners");

        while (listOfObjects.Count < amount && listOfObjects.Count < locations.Length)
        {
            foreach (GameObject location in locations)
            {
                position = location.transform.position;
                if(type != null && position != null)
                listOfObjects.AddFirst(Instantiate(type, position, Quaternion.identity));
            }
        }
    }

    private void SelectType()
    {
        switch (item)
        {
            case ObjectToSpawn.ManaPotion:
                type = objectTypes[0];
                break;
            case ObjectToSpawn.HealthPotion:
                type = objectTypes[1];
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
