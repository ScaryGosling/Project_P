using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    public static Player instance;

    [Header("Attacks")]
    private List<PlayerAttack> playerAttacks = new List<PlayerAttack>();

    public ClassAttackList attackSets = new ClassAttackList();
    public AttackSet attackSet { get; private set; }
    private PlayerAttack activeAttack;
    private int selectedAttack;
    public GameObject weapon;

    [Header("UI elements")]
    [SerializeField] private Image[] attackUISpot;
    private Coroutine[] cooldowns;

    [Header("Attributes")]
    [SerializeField] private Image health;
    [SerializeField] private Image resourceImage;
    [SerializeField] private int gold;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private bool hover = false;

    //public bool Attackable { get; set; } = false;

    public Resource Resource { get; private set; }
    public PlayerClass playerClass;
    private float tempHP = 100f;

    [SerializeField] private PlayerStats originalStats;
    [HideInInspector] public PlayerStats activeStats;

    [Serializable]
    public struct PlayerStats
    {
        public float movementSpeed;
        public float resistanceMultiplier;
        public float attackSpeed;
        public float attackDamage;

    }

    public void ResetStats()
    {
        activeStats.movementSpeed = originalStats.movementSpeed;
        activeStats.resistanceMultiplier = originalStats.resistanceMultiplier;
        activeStats.attackSpeed = originalStats.attackSpeed;
        activeStats.attackDamage = originalStats.attackDamage;
    }

    public float HealthProp {
        get { return tempHP; }
        set {
            tempHP += value * activeStats.resistanceMultiplier;
            health.fillAmount = tempHP * 0.01f;
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
    public static event Attack AttackEvent;

    public Transform GetSpawnPoint() { return spawnPoint; }

   public void RunAttackCooldown()
    {
        cooldowns[selectedAttack] = StartCoroutine(ShowCooldown());
    }

    public void Start()
    {
        CacheComponents();
        EventSystem.Current.RegisterListener<GiveResource>(Refill);
    }

    public void SetupClass() {

        switch (playerClass) {

            case PlayerClass.WIZARD:
                Resource = new Mana();
                attackSet = attackSets.Get(PlayerClass.WIZARD);
                break;

            case PlayerClass.WARRIOR:
                Resource = new Rage();
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
            Debug.Log(clone.list[i].name);

        }

        return clone;
    }


    public void CacheComponents() {
        
        instance = this;
        SetupClass();
        attackSet = CloneAttackSet();
        for (int i = 0; i < attackUISpot.Length; i++)
        {
            if (attackSet.list[i] != null)
            {

                attackUISpot[i].sprite = attackSet.list[i].GetImage();
            }
        }
        SelectAttack(0);
        Resource.CacheComponents(resourceImage);

        for (int i = 0; i < attackSet.list.Length; i++)
        {
            if (attackSet.list[i] != null)
            {
                attackSet.list[i].ResetCooldown();
            }
        }

        cooldowns = new Coroutine[attackUISpot.Length];
        ResetStats();

    }

    public void Update() {

        if (Input.GetMouseButton(0)) {
            if (!ClickOnFriendly() && !hover)
            {
                ExecuteAttack();
            }

        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (attackSet.list[0] != null)
            {
                SelectAttack(0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (attackSet.list[1] != null)
            {
                SelectAttack(1);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (attackSet.list[2] != null)
            {
                SelectAttack(2);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) {

            if (attackSet.list[3] != null)
            {
                SelectAttack(3);
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (attackSet.list[(selectedAttack + 1) % attackSet.list.Length] != null)
            {
                SelectAttack((selectedAttack + 1) % attackSet.list.Length);
            }
            else
            {
                int temp = selectedAttack +1;
                while (attackSet.list[(temp) % attackSet.list.Length] == null)
                {
                    temp++;
                }
                SelectAttack(temp % attackSet.list.Length);

            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {


            int temp = (selectedAttack - 1 < 0 ? selectedAttack + attackSet.list.Length - 1 : selectedAttack - 1);
                while (attackSet.list[temp] == null)
                {
                    temp--;
                }
                SelectAttack(temp);
        }
        

        if(tempHP <= 0)
        {
            SceneManager.LoadScene("SampleScene");
        }
    }


    public IEnumerator ShowCooldown()
    {
        Image attack = attackUISpot[selectedAttack];
        attack.fillAmount = 0;

        float animationTime = 0;
        float cooldownTime = activeAttack.GetCooldown();

        while (animationTime < cooldownTime)
        {
            animationTime += Time.deltaTime;
            attack.fillAmount = animationTime / cooldownTime;
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

    public void ExecuteAttack() {

        if (Resource.Value >= activeAttack.GetCastCost() / 100) {

            AttackEvent();
            

        }
    }


    public void SubscribeToAttackEvent() {

        for (int i = 0; i < attackSet.list.Length; i++)
        {
            if (attackSet.list[i] != null)
            {
                AttackEvent -= attackSet.list[i].Execute;
            }
        }

        AttackEvent += activeAttack.Execute;

    }
    

    public void SelectAttack(int selectedAttack)
    {

        for (int i = 0; i < attackUISpot.Length; i++)
        {
            if (attackSet.list[i] != null)
            {
                attackUISpot[i].color = new Color32(0, 0, 0, 100);
            }
            else
            {
                attackUISpot[i].color = new Color32(0, 0, 0, 0);
            }
        }
       

        if (attackSet.list[selectedAttack]) {

            activeAttack = attackSet.list[selectedAttack];
            attackUISpot[selectedAttack].color = new Color32(255,255,255,255);
            this.selectedAttack = selectedAttack;
            activeAttack.OnEquip();
        }

        SubscribeToAttackEvent();

    }



}


public enum PlayerClass {

    WIZARD, WARRIOR

}