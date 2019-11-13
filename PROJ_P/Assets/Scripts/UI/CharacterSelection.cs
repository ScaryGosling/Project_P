﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterSelection : MonoBehaviour
{
    public static CharacterSelection instance;

    [Header("Scene References")]
    [SerializeField] private GameObject abilityPanel;
    [SerializeField] private GameObject classPanel;
    [SerializeField] private TextMeshProUGUI header; 
    [SerializeField] private TextMeshProUGUI summary;
    [SerializeField] private TextMeshProUGUI abilityDescription;
    [SerializeField] private SceneHandler sceneHandler;

    [Header("Prefabs")]
    [SerializeField] private AttackSet[] attackSets;
    [SerializeField] private GameObject classPrefab;
    [SerializeField] private GameObject abilityPrefab;

    private AttackSet selectedClass;


    public void Start()
    {
        instance = this;

        foreach(AttackSet attackSet in attackSets)
        {
            Class classInstance = Instantiate(classPrefab, classPanel.transform).GetComponent<Class>();
            classInstance.attackSet = attackSet;
            classInstance.icon.sprite = attackSet.classIcon;
        }

        SetClass(attackSets[0]);
        
    }

    public void LoadAttacks()
    {
        for(int i = 0; i < abilityPanel.transform.childCount; i++)
        {
            Destroy(abilityPanel.transform.GetChild(i).gameObject);
        }

        for(int i = 0; i < selectedClass.list.Length; i++)
        {
            AbilityUI ability = Instantiate(abilityPrefab, abilityPanel.transform).GetComponent<AbilityUI>();
            ability.icon.sprite = selectedClass.list[i].GetImage();
            ability.attack = selectedClass.list[i];
        }
    }


    public void SetAbilityDescription(string description)
    {
        abilityDescription.text = description;
    }

    public void SetClass(AttackSet attackSet) {

        selectedClass = attackSet;
        header.text = attackSet.name;
        SetAbilityDescription("");
        LoadAttacks();
        summary.text = selectedClass.description;
        sceneHandler.keybindSet.playerClass = attackSet.playerClass;

    }

}
