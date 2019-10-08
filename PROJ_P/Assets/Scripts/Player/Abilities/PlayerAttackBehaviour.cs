﻿using System.Collections;
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
    private int selectedAttack;
    [SerializeField] private Image[] attackUISpot;

    public void Start()
    {
        CacheComponents();
    }

    public void CacheComponents() {
        activeAttack = activeAttacks[0];
        for (int i = 0; i < attackUISpot.Length; i++)
        {
            attackUISpot[i].sprite = activeAttacks[i].GetImage();
        }
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
    }

    public void ExecuteAttack() {

        if (mana.fillAmount >= activeAttack.GetCastCost() / 100) {

            activeAttack.Execute(spawnPoint);
            mana.fillAmount -= activeAttack.GetCastCost()/100;

        }
    }

    public void IncreaseMana(float mana)
    {

        this.mana.fillAmount += mana;
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

    }



}
