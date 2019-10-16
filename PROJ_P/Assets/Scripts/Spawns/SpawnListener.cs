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

    //enum UnitType { TYPE_00, TYPE_01, TYPE_02 }
    //UnitType currentType;
    private int currentType;
    private float pauseTime = 10f;
    private const float spawnTime = 1f;
    private int expected = 10;
    private int spawned = 0;
    private int unitsKilled = 0;
    private int waveIndex = 1;
    private GameObject absoluteUnit;
  
    [SerializeField] private float chanceOfDrop = 0.4f;
    [SerializeField] private bool debugMode;
    [SerializeField] private float spawnRate = 1f;
    [SerializeField] private GameObject[] spawns;
    [SerializeField] private GameObject[] UnitPrefabs;
    [SerializeField] private GameObject[] pickUp;
    [SerializeField] private GameObject shopKeeper;
    private bool shopKeeperActivated = false;


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
    }

    private void CheckForDrop(Vector3 location)
    {

        int temp = (int)Mathf.Floor(Random.Range(0f, 9f));
        
        if(temp <= 0)
        Instantiate(pickUp[temp], new Vector3(location.x, location.y / 2, location.z), Quaternion.identity);
        else if(temp == 1)
        Instantiate(pickUp[temp], new Vector3(location.x, location.y / 2, location.z), Quaternion.identity);
    }

    private void ResetWave()
    {
        spawned = 0;
        unitsKilled = 0;
        expected = (int)Mathf.Floor(expected * 1.20f);
        waveIndex++;
        Debug.Log("Next Round! " + "\t" + "Total Amount of Enemies: " + expected + "\t" + " Wave: " + waveIndex);
    }
    private bool KilledAllSpawned()
    {
        return unitsKilled >= expected;
    }

    private float CheckWaveCompletion()
    {
        if (KilledAllSpawned())
        {
            ResetWave();
            return pauseTime;
        }
        return spawnTime / 2;
    }

    private void UnitController()
    {
        CheckUnitType();

        switch (currentType)
        {
            case 0:
                absoluteUnit = UnitPrefabs[currentType].gameObject;
                break;
            case 1:
                absoluteUnit = UnitPrefabs[currentType].gameObject;
                break;
            case 2:
                absoluteUnit = UnitPrefabs[currentType].gameObject;
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

    IEnumerator Spawner()
    {
        while (true)
        {
            float time;

            if (spawned < expected)
            {
                shopKeeperActivated = false;
                time = spawnRate;
                UnitController();
                Instantiate(absoluteUnit, spawns[Random.Range(0, spawns.Length)].transform.position, Quaternion.identity);
                spawned++;
                yield return new WaitForSeconds(time);

            }
            else if (!shopKeeperActivated && KilledAllSpawned())
            {
                shopKeeper.transform.position = spawns[Random.Range(0, spawns.Length)].transform.position;
                shopKeeper.gameObject.SetActive(true);
                shopKeeperActivated = true;
            }
            else
            {
                time = CheckWaveCompletion();
                yield return new WaitForSeconds(time);
            }
        }
    }

}

