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
    [HideInInspector] public MeshRenderer Renderer;
    [HideInInspector] public NavMeshAgent agent;
    public LayerMask visionMask;
    public GameObject player;
    [SerializeField] private float health = 20;
    public float Health { get { return health; } set { health = value; } }
    private GenericTimer genericTimer;
    public GenericTimer getGenericTimer { get { return genericTimer; } set { genericTimer = value; } }
    public bool takingDamage { get; set; }

    private void Start()
    {
        genericTimer = GetComponent<GenericTimer>();
    }
    // Methods
    protected override void Awake()
    {
        Renderer = GetComponent<MeshRenderer>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        base.Awake();
    }

    public void TakeDamage(float damage)
    {
        takingDamage = true;
        Health -= damage;

        //metod if true
        //taking damage false
    }
}
