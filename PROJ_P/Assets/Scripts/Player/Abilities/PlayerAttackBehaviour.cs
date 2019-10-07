using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttackBehaviour : MonoBehaviour
{

    private List<PlayerAttack> playerAttacks = new List<PlayerAttack>();
    [SerializeField] private PlayerAttack[] activeAttacks = new PlayerAttack[3];
    private PlayerAttack activeAttack;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject[] UIAttacks;
    [SerializeField] private Image mana;

    public void Start()
    {
        CacheComponents();
    }

    public void CacheComponents() {
        activeAttack = activeAttacks[0];
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


    }

    public void ExecuteAttack() {

        if (mana.fillAmount >= activeAttack.GetCastCost() / 100) {

            activeAttack.Execute(spawnPoint);
            mana.fillAmount -= activeAttack.GetCastCost()/100;

        }
    }

    public void IncreaseMana(float mana) {

        this.mana.fillAmount += mana;
    }

    public void SelectAttack(int selectedAttack) {

        UIAttacks[0].SetActive(false);
        UIAttacks[1].SetActive(false);
        UIAttacks[2].SetActive(false);

        if (activeAttacks[selectedAttack]) {

            activeAttack = activeAttacks[selectedAttack];
            UIAttacks[selectedAttack].SetActive(true);

        }

    }



}
