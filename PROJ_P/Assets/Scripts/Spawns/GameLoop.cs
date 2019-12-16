﻿//Author: Emil Dahl
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This class takes care of everything that has to do with wave behavior. 
/// </summary>
public class GameLoop : MonoBehaviour
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
    private Player player;
    private float pauseTime = 10f;
    private int spawned = 0;
    private int unitsKilled = 0;
    private int waveIndex = 1;
    private bool shopOpen = false;
    private Queue locationQueue = new Queue();
    private float bonusHealth = 0f;
    public float HealthManagement { get { return bonusHealth; } }
    public static GameLoop instance;
    private float bonusDmg = 0f;
    public float DamageManagenent { get { return bonusDmg; } }
    private bool questGenerated = false;
    private int random;
    public Quest QuestProp { get; set; }

    [SerializeField] private int showRemaining = 3;
    [SerializeField] private bool debugMode;
    [SerializeField] private float playerSpeedScale = 1.5f;
    [Header("Wave Scaling")]
    [SerializeField] private float healthPerLevel = 5f;
    [SerializeField] private float damagePerLevel = 0f;
    [SerializeField] private int maximumCapacity = 200;
    [SerializeField] private float expectedGrowth = 1.20f;
    [SerializeField] private float growthDeclinePer = 5f;
    [SerializeField] private int expected = 1;
    [Header("Time")]
    [SerializeField] private float timeAfterShopkeeperLeavesForEnemiesToSpawn = 5;
    [SerializeField] private float spawnTime = 0.5f;
    [Header("Designer no touchy!")]
    [SerializeField] private GameObject[] spawns;
    [SerializeField] private GameObject[] unitPrefabs;
    [SerializeField] private GameObject[] pickUp;
    [SerializeField] private GameObject shopKeeper;
    [SerializeField] private GameObject ObjectSpawnerPrefab;
    [Header("Resources")]
    [SerializeField] private int goldPerKill = 10;
    [SerializeField] private int goldPerWave = 100;
    [Tooltip("The higher the base the steeper the curve of the logarithm. This means that of the base is higher, the amount of money gained will decrease slower.")]
    [SerializeField] [Range(2,10)] private int logBase = 10;
    [Header("Items")]
    [SerializeField] private int ItemAmount = 7;
    [SerializeField] private float chanceOfDrop = 0.4f;
    [SerializeField] private EliasPlayer eliasPlayer;
    [SerializeField] private EliasSetLevel setLevel;
    [SerializeField] private GameObject unitManagerObject;

    private bool waveCompleted;
    private GameObject objectSpawnerObject;
    private ObjectSpawner objectSpawner;
    private UnitManager unitManager;

    private NewWaveEvent newWaveEvent = new NewWaveEvent();
    private UnitsRemaining unitsRemaining = new UnitsRemaining();
    //private GameObject newPotion;
    private GameObject absoluteUnit;
    [SerializeField] private QuestHandler questHandler;
    [SerializeField] [Range(0, 100)] private int questChance = 50;


    private Coroutine waveTimer;
    private float waveTime;
    private List<GameObject> activeSpawns;
    private GameObject absoluteSpawner;
    private Vector3 spawnAtPosition;
    private int index = 0;

    public bool GetShopOpen() { return shopOpen; }

    private void Start()
    {
        unitManager = unitManagerObject.GetComponent<UnitManager>();
        player = Player.instance;
        instance = this;
        pauseTime = shopKeeper.GetComponent<Shop>().GetShopTime();
        EventSystem.Current.RegisterListener<UnitDeath>(UnitDeath);
        Mathf.Clamp(expectedGrowth, 1, 3);
        Mathf.Clamp(growthDeclinePer, 1, 100);
        growthDeclinePer = growthDeclinePer / 100;
        if (!debugMode)
        {
            StartCoroutine(Spawner());
            waveTimer = StartCoroutine(WaveTimer());
        }
   
        setLevel = eliasPlayer.customPreset;
        if (debugMode)
        {
           // TestAlgorithm();
        }
    }
    private void TestAlgorithm()
    {
        int expected = this.expected;
        int maximumCapacity = this.maximumCapacity;
        float expectedGrowth = this.expectedGrowth;
        int wave = 0;
        while (expected < maximumCapacity && expectedGrowth >= 1f)
        {
            wave++;
            Debug.Log("Expexted: " + expected + " expectedGrowth: " + expectedGrowth + " wave: " + wave);
            expected = (int)Mathf.FloorToInt(expected * expectedGrowth);
            expectedGrowth -= growthDeclinePer;
        }
    }

    /// <summary>
    /// Function that handles unit death events.
    /// </summary>
    /// <param name="death"></param>
    private void UnitDeath(UnitDeath death)
    {
        if (!death.isBoss)
        {
            unitsKilled += 1;
            HandleRemaining();
            player.GoldProp += goldPerKill;
            CheckForDrop(death.enemyObject.transform.position);
        }
    }

    /// <summary>
    /// Drops health potions where different units die. 
    /// </summary>
    /// <param name="location"></param>
    private void CheckForDrop(Vector3 location)
    {
        float temp = Random.Range(0f, 1f);
        if (chanceOfDrop >= temp)
            BowoniaPool.instance.GetFromPool(PoolObject.HEALTH_DROP, location, Quaternion.identity);
    }

    /// <summary>
    /// Resets everything, and progresses the game. Increasing the difficulty. 
    /// </summary>
    private void ResetWave()
    {
        waveCompleted = false;
        spawned = 0;
        unitsKilled = 0;
        objectSpawnerObject = null;
        waveIndex++;
        if (waveIndex <= 30)
        {
           expected = Mathf.RoundToInt(5.396047527f * Mathf.Pow(10, -6) * Mathf.Pow(waveIndex, 6) - 4.521048495f * Mathf.Pow(10, -4) * Mathf.Pow(waveIndex, 5) + 1.467687904f* Mathf.Pow(10, -2) * Mathf.Pow(waveIndex, 4) - 2.311597764f * Mathf.Pow(10, -1) * Mathf.Pow(waveIndex, 3) + 1.848912098f * Mathf.Pow(waveIndex, 2) - 2.135095198f * waveIndex + 5.503108386f);
        }
        else
        {
            expected = maximumCapacity;
        }
        Debug.Log("Next Round! " + "\t" + "Total Amount of Enemies: " + expected + "\t" + " Wave: " + waveIndex);
        //Debug.Log("Expexted: " + expected + " expectedGrowth: " + expectedGrowth);
        //random = Random.Range(18, 21);
        ChangeEliasLevel(19);

        bonusHealth += healthPerLevel;
        bonusDmg += damagePerLevel;
        waveTimer = StartCoroutine(WaveTimer());
        player.originalStats.movementSpeed /= playerSpeedScale;
        player.ResetSpeed();
        Prompt.instance.RunMessage("Speed has been reset", MessageType.INFO);
    }

    /// <summary>
    /// Checks if a quest should be generated this wave. 
    /// </summary>
    private void GenerateQuest()
    {
        if (!questGenerated)
        {
            if (QuestProp != null)
            {
                QuestProp.EndQuest();
                QuestProp = null;
            }
            if (Random.Range(0, 99) < questChance)
            {
                QuestProp = questHandler.GetRandomQuest();
                if (QuestProp is ProtectionQuest)
                {
                    QuestProp.StartQuest();

                }
                QuestProp.QuestDialogue();
            }
            else
            {
                QuestProp = null;
            }
        }
        questGenerated = true;
    }

    private IEnumerator WaveTimer()
    {
        waveTime = 0;
        while (true)
        {
            yield return new WaitForSeconds(1);
            waveTime++;
        }
    }

    /// <summary>
    /// Checks if wave is completed, and then runs relevant functions. 
    /// </summary>
    /// <returns></returns>
    private float CheckWaveCompletion()
    {
        if (unitsKilled >= expected)
        {
            if (!waveCompleted)
            {
                if (waveTimer != null)
                    StopCoroutine(waveTimer);
                player.GoldProp += (int)(goldPerWave / Mathf.Log(waveTime, logBase));
                waveCompleted = true;
                player.originalStats.movementSpeed *= playerSpeedScale;
                player.ResetSpeed();
                Prompt.instance.RunMessage("Gained: " + playerSpeedScale + "x speed!", MessageType.BONUS);
            }   
            SpawnShopKeeper();
            GenerateQuest();

            if (objectSpawnerObject != null)
            {
                objectSpawner.TerminateSpawner();
            }

            if (shopKeeper.activeInHierarchy)
            {
                return 0;
            }

            ResetWave();
            UpdateWaveCounter();

            return timeAfterShopkeeperLeavesForEnemiesToSpawn;
        }
        return spawnTime / 2;
    }

    /// <summary>
    /// Updates wave information.
    /// </summary>
    private void UpdateWaveCounter()
    {
        newWaveEvent.waveIndex = waveIndex;
        EventSystem.Current.FireEvent(newWaveEvent);
    }

    /// <summary>
    /// Calculates how many enemies are left, this information is then used to inform the player. 
    /// </summary>
    private void HandleRemaining()
    {
        int remaining = expected - unitsKilled;
        unitsRemaining.remaining = remaining;
        if (unitsKilled % showRemaining == 0 && remaining != 0 && unitsKilled != 0)
        {
  
            EventSystem.Current.FireEvent(unitsRemaining);
        }

    }


    private void SpawnShopKeeper()
    {
        if (!shopOpen)
        {
            //random = Random.Range(4,15);
            ChangeEliasLevel(10);
            shopKeeper.transform.position = NearbyRandomPosition();
            shopKeeper.gameObject.SetActive(true);
            shopOpen = true;
        }
    }

    /// <summary>
    /// Positions shopkeeper at a random spawner in the vicinity of the player.
    /// </summary>
    /// <returns></returns>
    private Vector3 NearbyRandomPosition()
    {
        spawnAtPosition = Vector3.zero;
        while (spawnAtPosition == Vector3.zero)
        {
            absoluteSpawner = spawns[Random.Range(0, spawns.Length)];

            if (absoluteSpawner.GetComponent<SpawnArea>().isActive)
                spawnAtPosition = absoluteSpawner.transform.position;
        }
        return spawnAtPosition;
    }
    private void ChangeEliasLevel(int level)
    {
        setLevel.level = level;
        setLevel.themeName = "Dawn of battle";
        eliasPlayer.QueueEvent(setLevel.CreateSetLevelEvent(eliasPlayer.Elias));
    
    }
    /// <summary>
    /// This enumerator represents the recursive gameloop.
    /// </summary>
    /// <returns></returns>
    IEnumerator Spawner()
    {
        float time;
        while (true)
        {
            if (spawned < expected)
            {
                if (objectSpawnerObject == null)
                {
                    objectSpawnerObject = BowoniaPool.instance.GetFromPool(PoolObject.OBJECT_SPAWNER);
                    objectSpawner = objectSpawnerObject.GetComponent<ObjectSpawner>();
                    objectSpawner.PopulateList(ItemAmount, ObjectSpawner.ObjectToSpawn.ManaPotion);
                }
                time = spawnTime / spawns.Length;
                shopOpen = false;
                if (questGenerated)
                {
                    if (QuestProp != null && !(QuestProp is ProtectionQuest))
                    {
                        QuestProp.StartQuest();
                    }
                    questGenerated = false;
                }
                foreach (GameObject spawnObject in spawns)
                {
                    
                    if (spawned < expected && spawnObject.GetComponent<SpawnArea>().isActive)
                    {
                        absoluteUnit = unitManager.ManageUnit(waveIndex, spawned);
                        absoluteUnit.transform.position = spawnObject.transform.GetChild(0).transform.position;
                        absoluteUnit.transform.rotation = Quaternion.identity;
                        absoluteUnit.gameObject.SetActive(true);
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

#region GameLoopLegacy
                        //absoluteUnit.GetComponent<NavMeshAgent>().Warp(spawnObject.transform.GetChild(0).transform.position);
                        //Debug.LogError(absoluteUnit.GetInstanceID()) ;
                        //Debug.Log(absoluteUnit.GetInstanceID());
                        //loop -> levande fiender
                        //absoluteUnit.GetComponent<NavMeshAgent>().pathStatus != NavMeshPathStatus.PathComplete -> do stuff
        //switch (currentType)
        //{
        //    case 0:
        //        absoluteUnit = unitPrefabs[currentType].gameObject;
        //        break;
        //    case 1:
        //        absoluteUnit = unitPrefabs[currentType].gameObject;
        //        break;
        //    case 2:
        //        absoluteUnit = unitPrefabs[currentType].gameObject;
        //        break;
        //    default:
        //        break;
        //}

        //if (expected < maximumCapacity && expectedGrowth >= 1f)
        //{
        //    expected = (int)Mathf.FloorToInt(expected * expectedGrowth);
        //    expectedGrowth -= growthDeclinePer;
        //Debug.Log("Next Round! " + "\t" + "Total Amount of Enemies: " + expected + "\t" + " Wave: " + waveIndex);

        //}        

    //private GameObject UnitController()
    //{
    //    if (waveIndex % 2 == 0 && spawned % 3 == 0)
    //    {
    //        return BowoniaPool.instance.GetFromPool(PoolObject.ZOOMER);
    //    }
    //    else if (waveIndex % 3 == 0 && spawned % 5 == 0)
    //    {
    //        return BowoniaPool.instance.GetFromPool(PoolObject.BOOMER);
    //    }
    //    else
    //    {
    //        return BowoniaPool.instance.GetFromPool(PoolObject.FANATIC);
    //    }
    //}

    /// <summary>
    /// During which waves, and when during those waves should each unit spawn.
    /// </summary>
    //private void CheckUnitType()
    //{

    //    if (waveIndex % 2 == 0 && spawned % 3 == 0)
    //    {
    //        currentType = 1;
    //    }
    //    else if (waveIndex % 3 == 0 && spawned % 5 == 0)
    //    {
    //        currentType = 2;
    //    }
    //    else
    //    {
    //        currentType = 0;
    //    }
    //}
#endregion