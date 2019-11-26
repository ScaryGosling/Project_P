﻿//Main Author: Emil Dahl
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
    public GameObject QuestTargetProp { get; set; }
    public GameObject target { get; set; }
    private GameLoop spawnListener;
    private float baseAttackRange;
    [Header("Sound and animation")]
    [SerializeField] private Animator animator;
    public EnemyWeapon weapon;
    [SerializeField] private AudioSource hurtAudioSource;
    [SerializeField] private AudioSource damageAudioSource;
    [SerializeField] private LayerMask combatMask;
    public LayerMask combatMaskProp { get { return combatMask; } }
    public AudioClip takeDamageClip;
    public AudioClip deathClip;
    public AudioClip attackSound;
    public ProtectionQuest ProtectionQuestProp { get; private set; }
    public bool AliveProp { get; set; }



    #region EnemyStats
    [SerializeField] private float moveSpeed;
    public float SpeedProp { get { return moveSpeed; } set { moveSpeed = value; } }
    [SerializeField] private float baseHeath = 20f;
    public float Health { get { return baseHeath; } set { baseHeath = value; } }

    public float InitialHealth { get; private set; }

    [SerializeField] private float baseAttack = 3f;
    public float Attack { get { return baseAttack; } set { baseAttack = value; } }

    [SerializeField] private float attackSpeed = 0.5f;
    public float AttackSpeed { get { return attackSpeed; } }

    [SerializeField] private int goldPerHit = 1;
    public int GetGold { get { return goldPerHit; } }

    [SerializeField] private float weight = 40;
    public float GetWeight { get { return weight; } }

    [SerializeField] private float attackRange = 2.5f;
    public float GetAttackRange { get { return attackRange; } set { attackRange = value; } }
    [SerializeField] private float attackRangeBuildings = 2.5f;
    public float AttackRangeBuildings { get { return attackRangeBuildings; } set { attackRangeBuildings = value; } }

    public float distanceMultiplier { get; set; } = 2;

    [Header("Unit variation")]
    [SerializeField] private float minSpeed = 0.9f, maxSpeed = 1.1f, minSize = 0.9f, maxSize = 1.1f; 


    #endregion

    public void PlayDamageAudio(AudioClip clip = null)
    {
        float pitch = Random.Range(0.5f, 1.5f);

        if (clip != null && Player.instance.GetSettings().UseSFX)
        {
            damageAudioSource.pitch = pitch;
            damageAudioSource.clip = clip;
            damageAudioSource.Play();
        }
    }

    public void PlayHurtAudio(AudioClip clip = null)
    {
        float pitch = Random.Range(0.5f, 1.5f);
        if (clip != null && Player.instance.GetSettings().UseSFX)
        {
            damageAudioSource.pitch = pitch;
            hurtAudioSource.clip = clip;
            hurtAudioSource.Play();
        }
    }

    #region Timer
    private GenericTimer genericTimer;
    public GenericTimer getGenericTimer { get { return genericTimer; } set { genericTimer = value; } }
    #endregion

    private void Start()
    {
        spawnListener = GameLoop.instance.GetComponent<GameLoop>();
        genericTimer = GetComponent<GenericTimer>();
        rigidbody = gameObject.GetComponent<Rigidbody>();
        capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
        ui = GetComponentInChildren<HostileUI>();
        InitalizeUnit();
        InitialHealth = Health;
        target = Player.instance.gameObject;
        baseAttackRange = GetAttackRange;
        if (spawnListener.QuestProp != null && spawnListener.QuestProp is ProtectionQuest)
        {
            ProtectionQuestProp = ((ProtectionQuest)(spawnListener.QuestProp));
            QuestTargetProp = ProtectionQuestProp.GetBuilding().GetComponent<ProtectionQuestBuildingHittingPoints>().GetRandomPoint();
        }
        else
        {
            QuestTargetProp = null;
        }

        agent.radius = capsuleCollider.radius * distanceMultiplier;
        agent.autoRepath = true;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
        AliveProp = true;
    }
    // Methods
    protected override void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
        agent = GetComponent<NavMeshAgent>();
        base.Awake();
    }

    private void ImprovePower()
    {
        //baseHeath += spawnListener.HealthManagement;
        baseAttack += spawnListener.DamageManagenent;
    }

    //Gotta add code to add and destroy gameObject... alternatively disable it.
    public void CheckTarget()
    {
        //this var is
        //just for development
        if (QuestTargetProp != null && ProtectionQuestProp.GetHealth() > 0 && Vector3.Distance(gameObject.transform.position, Player.instance.transform.position) > Vector3.Distance(gameObject.transform.position, QuestTargetProp.transform.position))
        {
            GetAttackRange = AttackRangeBuildings;
            target = QuestTargetProp.gameObject;
        }
        else
        {
            GetAttackRange = baseAttackRange;
            target = Player.instance.gameObject;
        }

    }
    private void OnEnable()
    {
        if (capsuleCollider )
        {
            AliveProp = true;
            capsuleCollider.enabled = true;
            Health = InitialHealth + spawnListener.HealthManagement; ;
        }

    }

    protected override void Update()
    {
        base.Update();
        if (animator != null)
        {
            animator.SetFloat("speed", Vector3.Dot(transform.forward, agent.velocity.normalized));
            animator.SetFloat("direction", Vector3.Dot(transform.right, agent.velocity.normalized));
        }
        CheckTarget();
    }

    private void InitalizeUnit()
    {
        ImprovePower();
        Randomize();
    }

    public Animator GetAnimator()
    {
        return animator;
    }

    public void Randomize()
    {
        gameObject.transform.localScale *= Random.Range(minSize, maxSize);
        SpeedProp *= Random.Range(minSpeed, maxSpeed);
    }
}
#region UnitLegacy
//public float healthProperty { get { return health; } set { health = value; } }
//Debug.Log("Units health: " + baseHeath);
//Debug.Log("Units attack: " + baseAttack);
//EventSystem.Current.RegisterListener<NewWave>(ImprovePower);

#endregion
