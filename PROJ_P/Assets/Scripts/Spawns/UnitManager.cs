//Author: Emil Dahl

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [Header("Spawn frequency")]
    [SerializeField] private int boomerSpawnFrequency = 5;
    [SerializeField] private int zoomerSpawnFrequency = 3;
    [Header("Wave threshold")]
    [SerializeField] private int boomerThreshold = 3;
    [SerializeField] private int zoomerThreshold = 5;

    [Header("Level frequency")]
    [SerializeField] private int boomerLevelsBetween = 2;
    [SerializeField] private int zoomerLevelsBetween = 2;
    private PoolObject poolObjects;

    /// <summary>
    /// Method for controlling how specific enmies should spawn.
    /// </summary>
    /// <param name="waveIndex"></param>
    /// <param name="spawned"></param>
    /// <returns></returns>
    public GameObject ManageUnit(int waveIndex, int spawned)
    {
        if (waveIndex >= boomerThreshold && waveIndex % boomerLevelsBetween == 0 && spawned % boomerSpawnFrequency == 0)
            return BowoniaPool.instance.GetFromPool(PoolObject.BOOMER);
        else if (waveIndex >= zoomerThreshold && waveIndex % zoomerLevelsBetween == 0 && spawned % zoomerSpawnFrequency == 0)
            return BowoniaPool.instance.GetFromPool(PoolObject.ZOOMER);
        else
            return BowoniaPool.instance.GetFromPool(PoolObject.FANATIC);
    }
}
