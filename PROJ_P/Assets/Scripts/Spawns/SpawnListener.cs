//Author: Emil Dahl
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnListener : MonoBehaviour
{
    #region smh
    //[SerializeField] private int unitsToSpawn = 10;
    //[SerializeField] private GameObject unitPrefab;
    //[SerializeField] private UnitType type;
    //private GameObject[] spawnLocation;
    //private float timer, currentTime;
    //private int waveIndex = 0;
    //public int mappedUnits { get; set; }
    #endregion

    private int currentType;
    private const int maximumCapacity = 200;
     
    private float pauseTime = 10f;
    [SerializeField] private float spawnTime = 0.5f;
    [SerializeField] private int expected = 1;
    private int spawned = 0;
    private int unitsKilled = 0;
    private int waveIndex = 1;
    private bool shopOpen = false;
    private Queue locationQueue = new Queue();
    private float bonusHealth = 0f;
    [SerializeField] private float healthPerLevel = 5f;
    public float HealthManagement { get { return bonusHealth; } }

    private float bonusDmg = 0f;
    [SerializeField] private float damagePerLevel = 0f;
    public float DamageManagenent { get { return bonusDmg; } }
    [SerializeField] private float timeAfterShopkeeperLeavesForEnemiesToSpawn = 5;

    [SerializeField] private float chanceOfDrop = 0.4f;
    [SerializeField] private bool debugMode;
    [SerializeField] private GameObject[] spawns;
    [SerializeField] private GameObject[] unitPrefabs;
    [SerializeField] private GameObject[] pickUp;
    [SerializeField] private GameObject shopKeeper;
    [SerializeField] private GameObject ObjectSpawnerPrefab;
    private GameObject ObjectSpawner;


    //private GameObject newPotion;
    private GameObject absoluteUnit;


    private void Start()
    {
        pauseTime = shopKeeper.GetComponent<Shop>().GetShopTime();
        EventSystem.Current.RegisterListener<UnitDeath>(UnitDeath);
        if (!debugMode)
            StartCoroutine(Spawner());
    }

    private void UnitDeath(UnitDeath death)
    {
        unitsKilled += 1;
        CheckForDrop(death.enemyObject.transform.position);
        Debug.Log("UnitsKilled++");
    }

    private void CheckForDrop(Vector3 location)
    {

        int temp = (int)Mathf.Floor(Random.Range(0, 1));

        if(temp <= chanceOfDrop)
            Instantiate(pickUp[1], new Vector3(location.x, location.y, location.z), Quaternion.identity);
        //if (temp <= chanceOfDrop)
        //    Instantiate(pickUp[temp], new Vector3(location.x, location.y, location.z), Quaternion.identity);
        //else if (temp > chanceOfDrop)
        //    Instantiate(pickUp[temp], new Vector3(location.x, location.y, location.z), Quaternion.identity);

        Debug.Log("SpawnedPotion");
    }

    private void ResetWave()
    {
        spawned = 0;
        unitsKilled = 0;
        if(expected <= maximumCapacity)
        {
            expected = (int)Mathf.Floor(expected * 1.20f);
        }
        waveIndex++;
        bonusHealth += healthPerLevel;
        bonusDmg += damagePerLevel;
        Debug.Log("Next Round! " + "\t" + "Total Amount of Enemies: " + expected + "\t" + " Wave: " + waveIndex);
    }


    private float CheckWaveCompletion()
    {
        if (unitsKilled >= expected)
        {
            SpawnShopKeeper();

            if (ObjectSpawner != null)
                ObjectSpawner.GetComponent<ObjectSpawner>().TerminateSpawner();
            if (shopKeeper.activeInHierarchy)
            {
                return 0;
            }
            ResetWave();
            return timeAfterShopkeeperLeavesForEnemiesToSpawn;
        }
        return spawnTime / 2;
    }

    private void UnitController()
    {
        CheckUnitType();

        switch (currentType)
        {
            case 0:
                absoluteUnit = unitPrefabs[currentType].gameObject;
                break;
            case 1:
                absoluteUnit = unitPrefabs[currentType].gameObject;
                break;
            case 2:
                absoluteUnit = unitPrefabs[currentType].gameObject;
                break;
            default:
                break;
        }
    }

    private void CheckUnitType()
    {
        if (waveIndex % 2 == 0 && spawned % 3 == 0)
        {
            currentType = 1;
        }
        else if (waveIndex % 3 == 0 && spawned % 5 == 0)
        {
            currentType = 2;
        }
        else
        {
            currentType = 0;
        }
    }

    private void SpawnShopKeeper()
    {
        if (!shopOpen)
        {
            shopKeeper.transform.position = spawns[Random.Range(0, spawns.Length)].transform.position;
            shopKeeper.gameObject.SetActive(true);
            shopOpen = true;
        }
    }

    IEnumerator Spawner()
    {
        while (true)
        {
            float time;

            if (spawned < expected)
            {
                if(ObjectSpawner == null)
                {
                    ObjectSpawner =  Instantiate(ObjectSpawnerPrefab);
                    ObjectSpawner.GetComponent<ObjectSpawner>().Amount = 4;
                    ObjectSpawner.GetComponent<ObjectSpawner>().item = global::ObjectSpawner.ObjectToSpawn.ManaPotion;
                    ObjectSpawner.GetComponent<ObjectSpawner>().PopulateList();
                }
                time = spawnTime / spawns.Length;
                shopOpen = false;
                foreach (GameObject spawnObject in spawns)
                {
                    UnitController();
                    if (spawned < expected && spawnObject.GetComponent<SpawnArea>().isActive)
                    {
                        Instantiate(absoluteUnit, spawnObject.transform.position, Quaternion.identity);
                        spawned++;
                    }
                    yield return new WaitForSeconds(time);
                }
            }
            else
            {
                time = CheckWaveCompletion();
                yield return new WaitForSeconds(time);
            }
        }
    }

}

