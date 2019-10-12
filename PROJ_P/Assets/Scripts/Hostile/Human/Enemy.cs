//Main Author: Emil Dahl
//Secondary Author: Paschalis Tolios
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : StateMachine
{
    // Attributes
    [HideInInspector] public MeshRenderer Renderer;
    [HideInInspector] public NavMeshAgent agent;
    public LayerMask visionMask;
    //public LayerMask ignoreLayerMask;
    public GameObject player;
    //public CapsuleCollider capsuleCollider;
    [SerializeField] private float health = 20;
    public float Health { get; private set; }

    private void Start()
    {
        Health = health;
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
        Health -= damage;    
    }
}
