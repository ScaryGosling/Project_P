using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    public static Player instance;

    [Header("Attacks")]
    [SerializeField] private PlayerAttack[] activeAttacks = new PlayerAttack[3];
    private List<PlayerAttack> playerAttacks = new List<PlayerAttack>();
    private PlayerAttack activeAttack;
    private int selectedAttack;

    [Header("UI elements")]
    [SerializeField] private Image[] attackUISpot;

    [Header("Attributes")]
    [SerializeField] private Image resourceImage;
    [SerializeField] private Transform spawnPoint;
    public Resource Resource { get; private set; }
    public PlayerClass playerClass;
    private float tempHP = 100f;
    public float healthProp { get { return tempHP; } set { tempHP = value;  } }
    

    public delegate void Attack();
    public static event Attack AttackEvent;

    public Transform GetSpawnPoint() { return spawnPoint; }

    public void Start()
    {
        CacheComponents();
        EventSystem.Current.RegisterListener<GiveResource>(Refill);
    }

    public void SetResource() {

        switch (playerClass) {

            case PlayerClass.WIZARD:
                Resource = new Mana();
                break;

            case PlayerClass.WARRIOR:
                Resource = new Rage();
                break;

            default:
                throw new System.Exception("Error when setting resource");
                break;

        }
    }

    public void Refill(GiveResource res)
    {
        Resource.IncreaseResource(res.fillAmount);
    }



    public void CacheComponents() {
        
        for (int i = 0; i < attackUISpot.Length; i++)
        {
            attackUISpot[i].sprite = activeAttacks[i].GetImage();
        }
        SelectAttack(0);
        instance = this;
        SetResource();
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
            SelectAttack((selectedAttack + 1) % activeAttacks.Length);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            SelectAttack(selectedAttack - 1 < 0 ? selectedAttack + activeAttacks.Length -1 : selectedAttack-1);
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

        foreach (PlayerAttack attack in activeAttacks) {

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

        if (activeAttacks[selectedAttack]) {

            activeAttack = activeAttacks[selectedAttack];
            attackUISpot[selectedAttack].color = new Color32(255,255,255,255);
            this.selectedAttack = selectedAttack;

        }

        SubscribeToAttackEvent();

    }



}


public enum PlayerClass {

    WIZARD, WARRIOR

}