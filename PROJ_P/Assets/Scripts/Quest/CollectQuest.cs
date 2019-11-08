using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectQuest : Quest
{
    // Start is called before the first frame update
    private int gathered = 0;
    [SerializeField] private int gatherSum = 3;
    private GameObject objectSpawner;
    private GameObject objectSpawnerPrefab;
    [SerializeField] private int quantity;
    void Start()
    {
        if(objectSpawner != null)
        {
            objectSpawner = Instantiate(objectSpawnerPrefab, transform.position, Quaternion.identity);
            objectSpawner.GetComponent<ObjectSpawner>().PopulateList(quantity, ObjectSpawner.ObjectToSpawn.Reward);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void StartQuest()
    {
        base.StartQuest();

    }
}
