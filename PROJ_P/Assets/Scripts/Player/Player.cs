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
    [SerializeField] private Texture2D fighterCursor;
    [SerializeField] private Texture2D mysticCursor;
    public Texture2D PlayerCursor { get; private set; }

    [Header("Attacks")]
    public ClassAttackList attackSets = new ClassAttackList();
    [SerializeField] private GameObject[] classModels;
    public AttackSet attackSet { get; private set; }
    private PlayerAttack activeAttack;
    private int selectedAttack;
    public GameObject weapon;
    [SerializeField] private Settings settings;

    [Header("UI elements")]
    [SerializeField] private Image[] attackUISpot;
    [SerializeField] private Image castBar;
    [SerializeField] private Color fullHealth;
    [SerializeField] private Color emptyHealth;
    [SerializeField] [Range(0, 1)] float colorTransitionPoint = 0.5f;
    [SerializeField] private GameObject deathPanel;
    private Coroutine[] cooldowns;
    private Image[] attackUISpotBG = new Image[4];

    [Header("Attributes")]
    [SerializeField] private Image health;
    [SerializeField] private Image resourceImage;
    [SerializeField] private Image durabilityImage;
    [SerializeField] private int gold;
    [SerializeField] private Transform spawnPoint;
    private AttackSet activeAttacks;
    private int highscore = 0;
    public AudioSource Audio { get; private set; }
    [SerializeField] private AudioClip[] hurtClip;
    [SerializeField] private AudioClip lackResourceClip;
    [SerializeField] private AudioClip heartbeatClip;
    [SerializeField] private AudioClip deathClip;
    [SerializeField] private AudioSource heartbeatSource;
    [SerializeField] private AudioSource chargeUpSource;

    [Header("Resources")]
    private int healthPotions;
    private int resourcePotions;
    [SerializeField] private float healthPotionIncrease = 30;
    [SerializeField] private int repairKitIncrease = 20;
    [SerializeField] [Range(0, 3)] private int healthPotionsStart = 3;
    [SerializeField] private Text healthPotionsText;
    [SerializeField] private Text resourcePotionsText;
    [SerializeField] [Range(0, 3)] private int resourcePotionsStart = 3;
    [SerializeField] [Range(0, 100)] private int autoRefillHealthUnder = 30;
    [SerializeField] [Range(0, 100)] private int autoRefillResourceUnder = 30;
    [SerializeField] private int maxHealthPotions = 3;
    [SerializeField] private int maxResourcePotions = 3;

    public EliasPlayer eliasPlayer;
    public EliasSetLevel setLevel;
    public EliasPlayStinger playStinger;

    public int MaxHealthPotionsProp { get; private set; } 
    public int MaxResourcePotionsProp { get; private set; } 

    public Resource Resource { get; private set; }
    public PlayerClass playerClass;
    private float tempHP = 100f;
    [SerializeField] private bool hover = false;


    public PlayerStats originalStats;
    [HideInInspector] public PlayerStats activeStats;
    [SerializeField] private Text durabilityTextObject;

    [SerializeField] private Animator animator;

    public Coroutine RageTap { get; set; }
    public Coroutine attackCast;
    public Coroutine abilityDelay;


    [Header("Flash")]
    public Renderer playerRenderer;
    [SerializeField] private Color flashColor;
    private Color baseColor;
    private Coroutine hitFlash;
    [SerializeField] private BloodVignette bloodVignette;


    public Settings GetSettings() { return settings; }

    public IEnumerator HitFlash()
    {
        playerRenderer.material.SetColor("_BaseColor", flashColor);
        yield return new WaitForSeconds(0.1f);
        playerRenderer.material.SetColor("_BaseColor", baseColor);
        hitFlash = null;
    }

    public void PlayAudio(AudioClip clip)
    {
        if (!settings.UseSFX)
            return;

        float pitch = Random.Range(0.7f, 1.3f);
        if (clip != null && Audio != null)
        {
            Audio.pitch = pitch;
            Audio.clip = clip;
            Audio.Play();
        }
    }

    public void PlayChargeUpAudio(AudioClip clip)
    {
        if (!settings.UseSFX)
            return;

        float pitch = Random.Range(0.7f, 1.3f);
        if (clip != null && Audio != null)
        {
            chargeUpSource.pitch = pitch;
            chargeUpSource.clip = clip;
            chargeUpSource.Play();
        }
    }

    public void ResetStats()
    {
        activeStats.movementSpeed = originalStats.movementSpeed;
        activeStats.resistanceMultiplier = originalStats.resistanceMultiplier;
        activeStats.attackSpeed = originalStats.attackSpeed;
        activeStats.attackDamage = originalStats.attackDamage;
    }

    public void ResetSpeed()
    {
        activeStats.movementSpeed = originalStats.movementSpeed;
    }

    public int HealthPotions
    {
        get { return healthPotions; }
        set
        {
            healthPotions = value;
            healthPotionsText.text = healthPotions + "/" + MaxHealthPotionsProp;
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
                resourcePotionsText.text = resourcePotions +"/" +MaxResourcePotionsProp;
            }
        }
    }
    [SerializeField] private bool invincible;
    public float HealthProp
    {
        get { return tempHP; }
        set
        {
            if (!invincible)
            {
                if (value < 0)
                {
                    tempHP += value * activeStats.resistanceMultiplier;

                    if (hitFlash != null)
                    {
                        StopCoroutine(hitFlash);
                    }
                    playerRenderer.material.SetColor("_BaseColor", baseColor);
                    hitFlash = StartCoroutine(HitFlash());
                }
                else
                {
                    tempHP += value; //When heald resistance should not matter
                }

                if (tempHP > 100)
                {
                    tempHP = 100;
                }
                health.fillAmount = tempHP * 0.01f;

                if (health.fillAmount < colorTransitionPoint)
                    health.color = Color.Lerp(emptyHealth, fullHealth, health.fillAmount / colorTransitionPoint);
                else
                    health.color = fullHealth;

                if (value < 0 && hurtClip != null)
                {
                    PlayAudio(hurtClip[Random.Range(0, hurtClip.Length)]);
                }

                if (tempHP < 25 && !GameLoop.instance.GetShopOpen())
                {
                    if (settings.UseSFX)
                    {
                        heartbeatSource.clip = heartbeatClip;
                        heartbeatSource.Play();
                    }
                    else
                    {
                        heartbeatSource.Stop();
                    }
                    bloodVignette.RunFlash(1 - tempHP/50);
                }

                if(tempHP > 25)
                {
                    bloodVignette.StartCoroutine(bloodVignette.EndFlash());
                }

            }
            else
            {

                Debug.Log("You got damaged");
            }
    
        }
    }

    public void PlayHeartBeat()
    {
        if (settings.UseSFX && HealthProp < 25)
        {
            heartbeatSource.clip = heartbeatClip;
            heartbeatSource.Play();
        }
        else
        {
            heartbeatSource.Stop();
        }

    }

    public int GoldProp
    {
        get { return gold; }
        set
        {
            if (value - gold > 0)
            {
                highscore += (value - gold);
            }
            gold = value;

        }
    }

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
        playerClass = settings.playerClass;
        CacheComponents();
        EventSystem.Current.RegisterListener<GiveResource>(Refill);


    }

    public void SetupClass()
    {

        switch (playerClass)
        {

            case PlayerClass.WARRIOR:
                Resource = ScriptableObject.CreateInstance<Rage>();
                attackSet = attackSets.Get(PlayerClass.WARRIOR);
                classModels[0].SetActive(true);
                classModels[1].SetActive(false);
                animator = classModels[0].GetComponent<Animator>();
                PlayerCursor = fighterCursor;
                break;

            case PlayerClass.WIZARD:
                Resource = ScriptableObject.CreateInstance<Mana>();
                attackSet = attackSets.Get(PlayerClass.WIZARD);
                PlayerCursor = mysticCursor;
                classModels[0].SetActive(false);
                classModels[1].SetActive(true);
                animator = classModels[1].GetComponent<Animator>();
                weapon.SetActive(false);
                break;

            default:
                throw new Exception("Error when setting resource");


        }

        originalStats = attackSet.originalStats;
    }

    public void Refill(GiveResource res)
    {
        Resource.IncreaseResource(res.fillAmount);
    }

    public Animator GetAnimator()
    {
        return animator;
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

    private void Awake()
    {
        instance = this;
    }
    public void CacheComponents()
    {
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

        Audio = GetComponent<AudioSource>();
        MaxHealthPotionsProp = maxHealthPotions;
        MaxResourcePotionsProp = maxResourcePotions;
        HealthPotions = healthPotionsStart;
        ResourcePotionsProp = resourcePotionsStart;
        Cursor.SetCursor(PlayerCursor, Vector2.zero, CursorMode.Auto);
        UpdateIcons();
        for(int i = 0; i < attackUISpot.Length; i++)
        {
            attackUISpotBG[i] = attackUISpot[i].transform.parent.GetComponent<Image>();
        }

        baseColor = playerRenderer.material.GetColor("_BaseColor");
    }

    public void Update()
    {

        if (!hover)
        {

            if (Input.GetKey(settings.GetBind(KeyFeature.BASE_ATTACK)))
            {
                if (activeAttacks.list[0] != null && !casting)
                {
                    attackCast = StartCoroutine(ExecuteAttack(activeAttacks.list[0]));
                }
            }
            else if (Input.GetKeyUp(settings.GetBind(KeyFeature.BASE_ATTACK)))
            {
                holding = false;
            }

            if (Input.GetKeyDown(settings.GetBind(KeyFeature.ABILITY_1)))
            {
                if (activeAttacks.list[1] != null && !casting)
                {
                    attackCast = StartCoroutine(ExecuteAttack(activeAttacks.list[1]));
                }
            }
            else if (Input.GetKeyDown(settings.GetBind(KeyFeature.ABILITY_2)))
            {
                if (activeAttacks.list[2] != null && !casting)
                {
                    attackCast = StartCoroutine(ExecuteAttack(activeAttacks.list[2]));
                }
            }
            else if (Input.GetKeyDown(settings.GetBind(KeyFeature.ABILITY_3)))
            {

                if (activeAttacks.list[3] != null && !casting)
                {
                    attackCast = StartCoroutine(ExecuteAttack(activeAttacks.list[3]));
                }
            }

        }
        if (HealthProp > 0)
        {
            UsePotions();
        }
        else
        {
            PlayerDied();
        }

        UseAbilityCheck();

        UseAutoRefill();

    }

    private void UsePotions()
    {
        if (Input.GetKeyDown(settings.GetBind(KeyFeature.REFILL_HEALTH)))
        {

            if (HealthPotions == 0)
            {
                Prompt.instance.RunMessage("No more Health potions", MessageType.WARNING);
            }
            UseHealthPotion();
        }
        if (Input.GetKeyDown(settings.GetBind(KeyFeature.REFILL_RESOURCE)))
        {
            if (resourcePotions == 0)
            {
                if (playerClass == PlayerClass.WARRIOR)
                {
                    Prompt.instance.RunMessage("No more Rage enhancers", MessageType.WARNING);

                }
                else
                {
                    Prompt.instance.RunMessage("No more Mana potions", MessageType.WARNING);
                }
            }
            UseResourcePotion();
        }
    }

    private void UseAutoRefill()
    {
        if (settings.UseAutoRefill)
        {
            if (HealthProp < autoRefillHealthUnder)
            {
                UseHealthPotion();
            }

            if (playerClass == PlayerClass.WIZARD && Resource.Value * 100 < autoRefillResourceUnder)
            {
                UseResourcePotion();
            }
        }
    }

    public void UseAbilityCheck()
    {
        for (int i = 0; i < activeAttacks.list.Length; i++)
        {

            if (activeAttacks.list[i] != null && activeAttacks.list[i].GetCastCost() / 100 <= Resource.Value)
            {
                attackUISpotBG[i].color = Color.white;
            }
            else
            {
                attackUISpotBG[i].color = new Color32(255, 255, 255, 68);
            }
        }
    }
    private GameObject healthParticles;
    private void UseHealthPotion()
    {
        if (tempHP != 100 && HealthPotions > 0)
        {
            healthParticles = BowoniaPool.instance.GetFromPool(PoolObject.HEALTH_PARTICLES);
            healthParticles.transform.position = transform.position;
            healthParticles.transform.SetParent(transform);

            HealthProp = healthPotionIncrease;
            HealthPotions--;
        }
    }
    private void UseResourcePotion()
    {
        if (ResourcePotionsProp > 0)
        {
            ResourcePotionsProp--;
            Resource.IncreaseResource(healthPotionIncrease / 100);
        }

    }

    private void PlayerDied()
    {
        GetComponent<PlayerMovement>().enabled = false;
        PlayAudio(deathClip);
        Audio = null;
        Animation anim = deathPanel.GetComponent<Animation>();
        SceneHandler handler = SceneHandler.instance;
        handler.StartCoroutine(handler.DelaySceneChange("DeathScene", anim.clip.length));
        anim.Play();
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
            attackUISpot[position].color = Color.white;
            UpdateIcons();
        }
    }

    private bool casting = false;
    private bool holding = false;

    public IEnumerator ExecuteAttack(PlayerAttack attack)
    {

        if (attack.cooldownActive)
        {
            if(!holding && attack.GetCooldown() > 1)
                Prompt.instance.RunMessage(attack.GetAbilityName() + " is on cooldown", MessageType.WARNING);
            yield break;
        }

        if (Resource.Value < attack.GetCastCost() / 100)
        {
            if(!holding)
                Prompt.instance.RunMessage("Not enough " + Resource.name, MessageType.WARNING);
            yield break;
        }

        PlayChargeUpAudio(attack.GetChargeUpSound());
        activeStats.movementSpeed = attack.GetSpeedMultiplier();
        Timer slowMultiplier = BowoniaPool.instance.GetFromPool(PoolObject.TIMER).GetComponent<Timer>();
        slowMultiplier.RunCountDown(attack.GetSlowTime(), attack.ResetSlow, Timer.TimerType.DELAY);

        if (attack.castTime > 0)
        {
            float animationTime = 0;
            float cooldownTime = attack.castTime;

            castBar.transform.parent.gameObject.SetActive(true);
            casting = true;
            while (animationTime < cooldownTime)
            {
                animationTime += Time.deltaTime;
                castBar.fillAmount = animationTime / cooldownTime;
                yield return null;

            }
            casting = false;

            castBar.transform.parent.gameObject.SetActive(false);

        }

        if (attack == activeAttacks.list[0]) { holding = true; }


        
        
        AnimationTrigger("Melee");
        attack.OnEquip();
        attack.Execute();
        attack.cooldownActive = true;
        chargeUpSource.Stop();
        yield return new WaitForSeconds(attack.GetCooldown());
        attack.cooldownActive = false;
    }
    public void AnimationTrigger(string trigger)
    {
        animator.SetTrigger(trigger);
    }

    private void UpdateIcons()
    {

        for (int i = 0; i < attackUISpot.Length; i++)
        {
            if (activeAttacks.list[i] != null)
            {
                attackUISpot[i].color = Color.white;
            }
            else
            {
                attackUISpot[i].color = new Color32(0, 0, 0, 0);
            }
        }
    }

    public bool dealDamageOnCollision { get; set; }
    public float damage { get; set; }
    public float magnitude { get; set; }
    public void OnCollisionEnter(Collision collision)
    {
        if (dealDamageOnCollision && collision.collider.CompareTag("Enemy"))
        {
            collision.collider.GetComponent<Unit>().currentState.TakeDamage(damage, magnitude);
        }
    }



}


public enum PlayerClass
{

    WIZARD, WARRIOR

}


[Serializable]
public struct PlayerStats
{
    public float movementSpeed;
    public float resistanceMultiplier;
    public float attackSpeed;
    public float attackDamage;
}