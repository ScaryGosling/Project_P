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
    public LayerMask visionMask;
    public GameObject player;

    [SerializeField] private float baseHeath = 20;
    public float health { get { return baseHeath; } set { baseHeath = value; } }
    private SpawnListener spawnListener;
    //public float healthProperty { get { return health; } set { health = value; } }



    private GenericTimer genericTimer;
    public GenericTimer getGenericTimer { get { return genericTimer; } set { genericTimer = value; } }
    

    private void Start()
    {
        spawnListener = GameObject.FindGameObjectWithTag("Listeners").GetComponent<SpawnListener>();
        genericTimer = GetComponent<GenericTimer>();
        ImprovePower();
        //Debug.Log("Units health: " + baseHeath);
        //EventSystem.Current.RegisterListener<NewWave>(ImprovePower);
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
        baseHeath += spawnListener.healthManagement;
    }


}
