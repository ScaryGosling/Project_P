//Author: Emil Dahl
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [Header("Wave Scaling")]
    [SerializeField] private float healthPerLevel = 5f;
    [SerializeField] private float damagePerLevel = 0f;
    [SerializeField] private int maximumCapacity = 200;
    [SerializeField] private float expectedGrowth = 1.20f;
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
    private bool waveCompleted;
    private GameObject ObjectSpawner;

    private NewWaveEvent newWaveEvent = new NewWaveEvent();
    private UnitsRemaining unitsRemaining = new UnitsRemaining();
    //private GameObject newPotion;
    private GameObject absoluteUnit;
    private QuestHandler questHandler;
    [SerializeField][Range(0,100)] private int questChance = 50;

    private Coroutine waveTimer;
    private float waveTime;

    private void Start()
    {
        player = Player.instance;
        instance = this;
        pauseTime = shopKeeper.GetComponent<Shop>().GetShopTime();
        EventSystem.Current.RegisterListener<UnitDeath>(UnitDeath);
        Mathf.Clamp(expectedGrowth, 1, 3);
        if (!debugMode)
        {
            StartCoroutine(Spawner());
        }
        questHandler = GetComponent<QuestHandler>();
    }

    private void UnitDeath(UnitDeath death)
    {
        unitsKilled += 1;
        player.GoldProp += goldPerKill;
        CheckForDrop(death.enemyObject.transform.position);
    }

    private void CheckForDrop(Vector3 location)
    {

        float temp = Random.Range(0f, 1f);

        if (chanceOfDrop >= temp)
            Instantiate(pickUp[1], new Vector3(location.x, location.y, location.z), Quaternion.identity);
    }

    private void ResetWave()
    {
        waveCompleted = false;
        spawned = 0;
        unitsKilled = 0;
        if (expected < maximumCapacity)
        {
            expected = (int)Mathf.Floor(expected * expectedGrowth);
        }
        waveIndex++;
        bonusHealth += healthPerLevel;
        bonusDmg += damagePerLevel;
        Debug.Log("Next Round! " + "\t" + "Total Amount of Enemies: " + expected + "\t" + " Wave: " + waveIndex);
        waveTimer = StartCoroutine(WaveTimer());
    }

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
    private float CheckWaveCompletion()
    {
        if (unitsKilled >= expected)
        {
            if (!waveCompleted)
            {
                if(waveTimer != null)
                    StopCoroutine(waveTimer);

                player.GoldProp += (int)(goldPerWave / Mathf.Log(waveTime));
                waveCompleted = true;
            }
            SpawnShopKeeper();
            GenerateQuest();

            if (ObjectSpawner != null)
                ObjectSpawner.GetComponent<ObjectSpawner>().TerminateSpawner();
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
    private void UpdateWaveCounter()
    {
        newWaveEvent.waveIndex = waveIndex;
        EventSystem.Current.FireEvent(newWaveEvent);
    }

    private void HandleRemaining()
    {
        if (unitsKilled % showRemaining == 0 && unitsKilled != 0)
        {
            unitsRemaining.remaining = expected - unitsKilled;
            EventSystem.Current.FireEvent(unitsRemaining);
        }
        
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
            HandleRemaining();
            float time;

            if (spawned < expected)
            {
                if (ObjectSpawner == null)
                {
                    ObjectSpawner = Instantiate(ObjectSpawnerPrefab);
                    ObjectSpawner.GetComponent<ObjectSpawner>().Amount = ItemAmount;
                    ObjectSpawner.GetComponent<ObjectSpawner>().item = global::ObjectSpawner.ObjectToSpawn.ManaPotion;
                    ObjectSpawner.GetComponent<ObjectSpawner>().PopulateList();
                }
                time = spawnTime / spawns.Length;
                shopOpen = false;
                if (questGenerated)
                {
                    if (!(QuestProp is ProtectionQuest))
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

