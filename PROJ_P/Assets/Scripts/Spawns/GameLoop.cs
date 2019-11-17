//Author: Emil Dahl
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
    [Header("Items")]
    [SerializeField] private int ItemAmount = 7;
    [SerializeField] private float chanceOfDrop = 0.4f;
    [SerializeField] private EliasPlayer eliasPlayer;
    [SerializeField] private EliasSetLevel setLevel;

    private bool waveCompleted;
    private GameObject objectSpawnerObject;
    private ObjectSpawner objectSpawner;

    private NewWaveEvent newWaveEvent = new NewWaveEvent();
    private UnitsRemaining unitsRemaining = new UnitsRemaining();
    //private GameObject newPotion;
    private GameObject absoluteUnit;
    [SerializeField] private QuestHandler questHandler;
    [SerializeField] [Range(0, 100)] private int questChance = 50;
    [SerializeField] private EliasSetLevelOnTrack setLevelOnTrack;

    private Coroutine waveTimer;
    private float waveTime;

    private void Start()
    {
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
        }
   
        setLevel = eliasPlayer.customPreset;

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
            Instantiate(pickUp[1], new Vector3(location.x, location.y, location.z), Quaternion.identity);
    }

    /// <summary>
    /// Resets everything, and progresses the game. Increasing the difficulty. 
    /// </summary>
    private void ResetWave()
    {
        waveCompleted = false;
        spawned = 0;
        unitsKilled = 0;
        if (expected < maximumCapacity && expectedGrowth >= 1f)
        {
            expected = (int)Mathf.FloorToInt(expected * expectedGrowth);
            expectedGrowth -= growthDeclinePer;
        Debug.Log("Next Round! " + "\t" + "Total Amount of Enemies: " + expected + "\t" + " Wave: " + waveIndex);

        }
        ChangeEliasLevel(20);
        waveIndex++;
        bonusHealth += healthPerLevel;
        bonusDmg += damagePerLevel;
        waveTimer = StartCoroutine(WaveTimer());
        player.originalStats.movementSpeed /= playerSpeedScale;
        player.ResetSpeed();
        Prompt.instance.RunMessage("Speed has been reset", MessageType.INFO);
        Debug.Log("Units to Spawn: " + expected + " " + "Next wave growth" + expectedGrowth);

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
                QuestProp.QuestDialogue();
                if (QuestProp is ProtectionQuest)
                {
                    QuestProp.StartQuest();

                }
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

                player.GoldProp += (int)(goldPerWave / Mathf.Log(waveTime));
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


    private void UnitController()
    {
        CheckUnitType();
        absoluteUnit = unitPrefabs[currentType].gameObject;
    }

    /// <summary>
    /// During which waves, and when during those waves should each units spawn.
    /// </summary>
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
            ChangeEliasLevel(4);
            shopKeeper.transform.position = spawns[Random.Range(0, spawns.Length)].transform.position;
            shopKeeper.gameObject.SetActive(true);
            shopOpen = true;
        }
    }
    private void ChangeEliasLevel(int level)
    {
        setLevel.level = level;
        setLevel.themeName = "Objective";
        eliasPlayer.QueueEvent(setLevel.CreateSetLevelEvent(eliasPlayer.Elias));

    }
    /// <summary>
    /// This enumerator represents the recursive gameloop.
    /// </summary>
    /// <returns></returns>
    IEnumerator Spawner()
    {
        while (true)
        {
            float time;

            if (spawned < expected)
            {
                if (objectSpawnerObject == null)
                {
                    objectSpawnerObject = Instantiate(ObjectSpawnerPrefab);
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

#region GameLoopLegacy
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

#endregion