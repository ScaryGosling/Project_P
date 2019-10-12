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

    private const float pauseTime = 10f;
    private const float spawnTime = 1f;
    private int expected = 10;
    private int spawned = 0;
    private int unitsKilled = 0;
    private int waveIndex = 1;
    [SerializeField] private bool debugMode;
    [SerializeField] private GameObject[] spawns;
    [SerializeField] private GameObject unitPrefab;

    private void Start()
    {
        EventSystem.Current.RegisterListener<UnitDeath>(UnitDeath);
        if(!debugMode)
        StartCoroutine(Spawner());

    }

    private void UnitDeath(UnitDeath death)
    {
            unitsKilled += 1;
    }

    private void ResetWave()
    {
        spawned = 0;
        unitsKilled = 0;
        expected = (int) Mathf.Floor(expected * 1.4f);
        waveIndex++;
        Debug.Log("Next Round! " + "\t" + "Total Amount of Enemies: " + expected + "\t" + " Wave: " + waveIndex);
     

    }


    private float CheckWaveCompletion()
    {
        if (unitsKilled >= expected)
        {
            ResetWave();
            return pauseTime;
        }
        return spawnTime / 2;
    }


    IEnumerator Spawner()
    {
        while (true)
        {
            float time;

            if (spawned < expected)
            {
                time = 1f;
                foreach (GameObject spawnObject in spawns)
                {
                    Instantiate(unitPrefab, spawnObject.transform.position, Quaternion.identity);
                    spawned++;
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


enum UnitType
{
    TYPE_00, TYPE_01, TYPE_02
}
