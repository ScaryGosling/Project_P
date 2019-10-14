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
    private AttackSet attackSet;
    private PlayerAttack activeAttack;
    private int selectedAttack;
    public GameObject weapon;
    public GameObject attackBox;

    [Header("UI elements")]
    [SerializeField] private Image[] attackUISpot;

    [Header("Attributes")]
    [SerializeField] private Image health;
    [SerializeField] private Image resourceImage;
    [SerializeField] private Transform spawnPoint;
    public Resource Resource { get; private set; }
    public PlayerClass playerClass;
    private float tempHP = 100f;
    public float healthProp {
        get { return tempHP; }
        set { tempHP = value;
            health.fillAmount = value / 100;
        }
    }
    

    public delegate void Attack();
    public static event Attack AttackEvent;

    public Transform GetSpawnPoint() { return spawnPoint; }

   

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
                throw new System.Exception("Error when setting resource");
                

        }
    }

    public void Refill(GiveResource res)
    {
        Resource.IncreaseResource(res.fillAmount);
    }



    public void CacheComponents() {
        
        instance = this;
        SetupClass();
        for (int i = 0; i < attackUISpot.Length; i++)
        {
            attackUISpot[i].sprite = attackSet.list[i].GetImage();
        }
        SelectAttack(0);
        Resource.CacheComponents(resourceImage);
    }

    public void Update() {

        if (Input.GetMouseButtonDown(0)) {

            ExecuteAttack();

        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectAttack(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectAttack(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectAttack(2);
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            SelectAttack((selectedAttack + 1) % attackSet.list.Length);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            SelectAttack(selectedAttack - 1 < 0 ? selectedAttack + attackSet.list.Length -1 : selectedAttack-1);
        }

        if(tempHP <= 0)
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    public void ExecuteAttack() {

        if (Resource.Value >= activeAttack.GetCastCost() / 100) {

            Debug.Log("Mana high enough");
            AttackEvent();

        }
    }


    public void SubscribeToAttackEvent() {

        foreach (PlayerAttack attack in attackSet.list) {

            AttackEvent -= attack.Execute;
        }

        AttackEvent += activeAttack.Execute;

    }
    

    public void SelectAttack(int selectedAttack)
    {

        foreach (var item in attackUISpot)
        {
            item.color = new Color32(0, 0, 0, 100);
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