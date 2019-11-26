using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowoniaPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public PoolObject poolObject;
        public GameObject prefab;
    }

    private Dictionary<PoolObject, GameObject> poolObjectTranslation = new Dictionary<PoolObject, GameObject>();
    public static BowoniaPool instance { get; private set; }
    private Dictionary<PoolObject, Queue<GameObject>> poolDictionary = new Dictionary<PoolObject, Queue<GameObject>>();
    GameObject tempPoolObject;
    [SerializeField] private List<Pool> pools;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        PopulateTranslation();
    }

    private void PopulateTranslation()
    {
        foreach (Pool pool in pools)
        {
            poolObjectTranslation.Add(pool.poolObject, pool.prefab);
            Queue<GameObject> objectPool = new Queue<GameObject>();
            poolDictionary.Add(pool.poolObject, objectPool);
        }
    }

    private void GrowPool(PoolObject poolObject)
    {
        tempPoolObject = Instantiate(poolObjectTranslation[poolObject]);
        if (poolObject is PoolObject.TIMER)
        {
            tempPoolObject.transform.SetParent(gameObject.transform);
        }
        AddToPool(poolObject, tempPoolObject);
    }

    public GameObject GetFromPool(PoolObject poolObject)
    {

        if (!poolDictionary.ContainsKey(poolObject) || poolDictionary[poolObject].Count == 0)
        {
            GrowPool(poolObject);
        }
        tempPoolObject = poolDictionary[poolObject].Dequeue();
        tempPoolObject.SetActive(true);
        return tempPoolObject;
    }
    public void AddToPool(PoolObject poolObject, GameObject instanceToAdd)
    {
        instanceToAdd.SetActive(false);
        poolDictionary[poolObject].Enqueue(instanceToAdd);
    }
}


public enum PoolObject
{
    FANATIC, ZOOMER, BOOMER, TIMER, WAND, LIGHTNING, FIREBALL, ICE_NOVA
}