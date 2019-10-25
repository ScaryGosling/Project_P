//Main Author: Emil Dahl
//Secondary Author: Paschalis Tolios
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(GenericTimer))]
public class Unit : StateMachine
{
    // Attributes
    [HideInInspector] public MeshRenderer renderer;
    [HideInInspector] public NavMeshAgent agent;
    public HostileUI ui { get; set; }
    public CapsuleCollider capsuleCollider { get; set; }
    public Rigidbody rigidbody { get; set; }
    public LayerMask visionMask;
    public GameObject player;
    private SpawnListener spawnListener;

    #region EnemyStats
    [SerializeField] private float baseHeath = 20f;
    public float Health { get { return baseHeath; } set { baseHeath = value; } }

    public float InitialHealth { get; private set; }

    [SerializeField] private float baseAttack = 3f;
    public float Attack { get { return baseAttack; } set { baseAttack = value; } }

    [SerializeField] private float attackSpeed = 0.5f;
    public float AttackSpeed { get { return attackSpeed; } }
    #endregion



    #region Timer
    private GenericTimer genericTimer;
    public GenericTimer getGenericTimer { get { return genericTimer; } set { genericTimer = value; } }
    #endregion

    private void Start()
    {
        spawnListener = GameObject.FindGameObjectWithTag("Listeners").GetComponent<SpawnListener>();
        genericTimer = GetComponent<GenericTimer>();
        rigidbody = gameObject.GetComponent<Rigidbody>();
        capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
        ui = GetComponentInChildren<HostileUI>();
        ImprovePower();
        InitialHealth = Health;
    }
    // Methods
    protected override void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        base.Awake();
    }

    private void ImprovePower()
    {
        baseHeath += spawnListener.HealthManagement;
        baseAttack += spawnListener.DamageManagenent;
    }
}
#region UnitLegacy
//public float healthProperty { get { return health; } set { health = value; } }
//Debug.Log("Units health: " + baseHeath);
//Debug.Log("Units attack: " + baseAttack);
//EventSystem.Current.RegisterListener<NewWave>(ImprovePower);

#endregion
