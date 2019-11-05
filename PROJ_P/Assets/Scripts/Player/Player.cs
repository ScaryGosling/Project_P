using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{

    public static Player instance;
    [SerializeField] private bool debugMode;

    [Header("Attacks")]
    public ClassAttackList attackSets = new ClassAttackList();
    public AttackSet attackSet { get; private set; }
    private PlayerAttack activeAttack;
    private int selectedAttack;
    public GameObject weapon;
    [SerializeField] private KeybindSet keybindSet;

    [Header("UI elements")]
    [SerializeField] private Image[] attackUISpot;
    [SerializeField] private Color fullHealth;
    [SerializeField] private Color emptyHealth;
    [SerializeField] [Range(0, 1)] float colorTransitionPoint = 0.5f;
    private Coroutine[] cooldowns;

    [Header("Attributes")]
    [SerializeField] private Image health;
    [SerializeField] private Image resourceImage;
    [SerializeField] private int gold;
    [SerializeField] private Transform spawnPoint;
    private AttackSet activeAttacks;
    public AudioSource Audio { get; private set; }
    [SerializeField] private AudioClip[] hurtClip;
    [SerializeField] private AudioClip lackResourceClip;

    [Header("Resources")]
    private int healthPotions;
    private int resourcePotions;
    [SerializeField] private float healthPotionIncrease = 30;
    [SerializeField] [Range(0, 3)] private int healthPotionsStart = 3;
    [SerializeField] private Text healthPotionsText;
    [SerializeField] private Text resourcePotionsText;
    [SerializeField] [Range(0, 3)] private int resourcePotionsStart = 3;
    
    public Resource Resource { get; private set; }
    public PlayerClass playerClass;
    private float tempHP = 100f;
    [SerializeField] private bool hover = false;


    [SerializeField] private PlayerStats originalStats;
    [HideInInspector] public PlayerStats activeStats;
    [SerializeField] private Text durabilityTextObject;

    public Coroutine RageTap { get; set; }



    [Serializable]
    public struct PlayerStats
    {
        public float movementSpeed;
        public float resistanceMultiplier;
        public float attackSpeed;
        public float attackDamage;
    }

    public KeybindSet GetKeybindSet() { return keybindSet; }

    public void PlayAudio(AudioClip clip)
    {
        Audio.clip = clip;
        Audio.Play();
    }

    public void ResetStats()
    {
        activeStats.movementSpeed = originalStats.movementSpeed;
        activeStats.resistanceMultiplier = originalStats.resistanceMultiplier;
        activeStats.attackSpeed = originalStats.attackSpeed;
        activeStats.attackDamage = originalStats.attackDamage;
    }

    public int HealthPotions { 
        get { return healthPotions; }
        set
        {
            healthPotions = value;
            healthPotionsText.text = healthPotions.ToString();
        }
    }

    public int ResourcePotionsProp
    {
        get
        {
            return resourcePotions;
        }
        set
        {
            resourcePotions = value;
            if (resourcePotionsText != null)
            {
                resourcePotionsText.text = resourcePotions.ToString();
            }
        }
    }

    public float HealthProp {
        get { return tempHP; }
        set {
            tempHP += value * activeStats.resistanceMultiplier;
            health.fillAmount = tempHP * 0.01f;

            if (health.fillAmount < colorTransitionPoint)
                health.color = Color.Lerp(emptyHealth, fullHealth, health.fillAmount /colorTransitionPoint);

            if (value < 0 && hurtClip != null)
            {
                Audio.clip = hurtClip[Random.Range(0, hurtClip.Length)];
                Audio.Play();
            }
        }
    }
    public int GoldProp
    {
        get { return gold; }
        set
        {
            gold = value;
        }
    }


    
    public delegate void Attack();
    public  event Attack AttackEvent;

    public Transform GetSpawnPoint() { return spawnPoint; }


    public Text GetDurabilityText()
    {
        return durabilityTextObject;
    }
   public void RunAttackCooldown(PlayerAttack executedAttack)
    {
        for (int i = 0; i < activeAttacks.list.Length; i++)
        {
            if (activeAttacks.list[i] == executedAttack)
            {
                cooldowns[i] = StartCoroutine(ShowCooldown(i));
            }
        }
    }

    public void Start()
    {
        playerClass = keybindSet.playerClass;
        CacheComponents();
        EventSystem.Current.RegisterListener<GiveResource>(Refill);
    }

    public void SetupClass() {

        switch (playerClass) {

            case PlayerClass.WIZARD:
                Resource = ScriptableObject.CreateInstance<Mana>();
                attackSet = attackSets.Get(PlayerClass.WIZARD);
                break;

            case PlayerClass.WARRIOR:
                Resource = ScriptableObject.CreateInstance<Rage>();
                attackSet = attackSets.Get(PlayerClass.WARRIOR);
                break;

            default:
                throw new Exception("Error when setting resource");
                

        }
    }
  
    public void Refill(GiveResource res)
    {
        Resource.IncreaseResource(res.fillAmount);
    }


    public AttackSet CloneAttackSet()
    {
        AttackSet clone = Instantiate(attackSet);
        for (int i = 0; i < clone.list.Length; i++)
        {
            clone.list[i] = Instantiate(attackSet.list[i]);

        }

        return clone;
    }


    public void CacheComponents() {

        instance = this;
        SetupClass();
        attackSet = CloneAttackSet();
        activeAttacks = ScriptableObject.CreateInstance<AttackSet>();
        activeAttacks.list[0] = attackSet.list[0];
        if (debugMode)
        {
            for (int i = 0; i < 4; i++)
            {
                activeAttacks.list[i] = attackSet.list[i];
            }

        }
        for (int i = 0; i < attackUISpot.Length; i++)
        {
            if (activeAttacks.list[i] != null)
            {

                attackUISpot[i].sprite = activeAttacks.list[i].GetImage();
            }
        }
        Resource.CacheComponents(resourceImage);

        for (int i = 0; i < activeAttacks.list.Length; i++)
        {
            if (activeAttacks.list[i] != null)
            {
                activeAttacks.list[i].ResetCooldown();
            }
        }

        cooldowns = new Coroutine[attackUISpot.Length];
        ResetStats();
        health.color = fullHealth;

        if (playerClass == PlayerClass.WARRIOR)
        {
            ((MeleeHack)activeAttacks.list[0]).SetDurabilityTextObject(durabilityTextObject);
        }
        else
        {
            durabilityTextObject.gameObject.SetActive(false);
        }
        Audio = GetComponent<AudioSource>();
        HealthPotions = healthPotionsStart;
        ResourcePotionsProp = resourcePotionsStart;
        UpdateIcons();
    }

    public void Update() {

        if (!hover)
        {

            if (Input.GetKey(keybindSet.GetBind(KeyFeature.BASE_ATTACK)))
            {
                if (activeAttacks.list[0] != null)
                {
                    ExecuteAttack(activeAttacks.list[0]);
                }
            }

            if (Input.GetKey(keybindSet.GetBind(KeyFeature.ABILITY_1)))
            {
                if (activeAttacks.list[1] != null)
                {
                    ExecuteAttack(activeAttacks.list[1]);
                }
            }
            else if (Input.GetKey(keybindSet.GetBind(KeyFeature.ABILITY_2)))
            {
                if (activeAttacks.list[2] != null)
                {
                    ExecuteAttack(activeAttacks.list[2]);
                }
            }
            else if (Input.GetKey(keybindSet.GetBind(KeyFeature.ABILITY_3)))
            {

                if (activeAttacks.list[3] != null)
                {
                    ExecuteAttack(activeAttacks.list[3]);
                }
            }

        }
       

        if (Input.GetKeyDown(keybindSet.GetBind(KeyFeature.REFILL_HEALTH)))
        {
            UseHealthPotion();
        }
        if (Input.GetKeyDown(keybindSet.GetBind(KeyFeature.REFILL_RESOURCE)))
        {
            UseResourcePotion();
        }

        if(tempHP <= 0)
        {
            PlayerDied();   
        }

    }

    private void UseHealthPotion()
    {
        if (HealthPotions>0)
        {
            HealthProp = healthPotionIncrease;
            HealthPotions--;
        }
    }
    private void UseResourcePotion()
    {
        if (ResourcePotionsProp > 0)
        {
            ResourcePotionsProp--;
            if (playerClass == PlayerClass.WIZARD)
            {
                Resource.IncreaseResource(healthPotionIncrease / 100);
            }
            else
            {
                ((MeleeHack)activeAttacks.list[0]).IncreaseDurability(5 /100.0f);
            }
        }

    }

    private void PlayerDied()
    {
        SceneHandler.instance.GoToScene(SceneManager.GetActiveScene().name);
    }

    Image[] attack = new Image[4];
    private IEnumerator ShowCooldown(int position)
    {
        float animationTime;
        float cooldownTime;
        attack[position] = attackUISpot[position];
        attack[position].fillAmount = 0;
        animationTime = 0;
        cooldownTime = activeAttacks.list[position].GetCooldown() / activeStats.attackSpeed;
        while (animationTime < cooldownTime)
        {
            animationTime += Time.deltaTime;
            attack[position].fillAmount = animationTime / cooldownTime;
            yield return null;

        }
    }

    RaycastHit hit;
    Ray ray;
    private bool ClickOnFriendly()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.tag == "Friendly")
            {
                return true;
            }
        }
        return false;
    }

    public void SetHover(bool hover)
    {
        this.hover = hover;
    }


    public void SetAbility(int position, PlayerAttack ability)
    {
        activeAttacks.list[position] = ability;

        if (ability == null)
        {
            attackUISpot[position].color = new Color32(0, 0, 0, 0);
        }
        else
        {
            attackUISpot[position].color = new Color32(0, 0, 0, 100);
            UpdateIcons();
        }
    }

    public void ExecuteAttack(PlayerAttack attack) {

        if (Resource.Value >= attack.GetCastCost() / 100) 
        {
            attack.OnEquip();
            attack.Execute();

        }
        else
        {
            Audio.clip = lackResourceClip;
            Audio.Play();
        }
    }


    private void UpdateIcons()
    {

        for (int i = 0; i < attackUISpot.Length; i++)
        {
            if (activeAttacks.list[i] != null)
            {
                attackUISpot[i].color = new Color32(0, 0, 0, 255);
            }
            else
            {
                attackUISpot[i].color = new Color32(0, 0, 0, 0);
            }
        }
    }




}


public enum PlayerClass {

    WIZARD, WARRIOR

}