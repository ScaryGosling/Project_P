using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProtectionQuest : Quest
{
    [SerializeField] private GameObject[] buildings;
    private GameObject buildingToDefend;
    private float buildingHealth = 100;
    [SerializeField] private Image healthImage;
    private ProtectionQuestEvent protectionQuestEvent;
    private ToggleArrowEvent toggleArrow = new ToggleArrowEvent();
    [SerializeField] [Range(0.01f, 1.2f)]private float enemyDamagePercentage = 0.1f;
    private int buildingIndex;
    private DialogueEvent dialogueEvent = new DialogueEvent();
    [SerializeField] List<DialogueData> questData;

    public override void StartQuest()
    {
        base.StartQuest();
        buildingToDefend = buildings[buildingIndex];
        buildingHealth = 100;
        healthImage.transform.parent.gameObject.SetActive(true);
        healthImage.fillAmount = 1;
        FireArrow(true, buildingToDefend);

    }
    public override void QuestDialogue()
    {
        buildingIndex = Random.Range(0, buildings.Length);
        dialogueEvent.data = questData[buildingIndex];
        EventSystem.Current.FireEvent(dialogueEvent);
    }

    public GameObject GetBuilding()
    {
        return buildingToDefend;
    }

    public void TakeDamage(float damage)
    {
        damage *= 0.1f;
        buildingHealth -= damage;
        healthImage.fillAmount = buildingHealth / 100;
        if (buildingHealth <= 0)
        {
            EndQuest();
        }
    }

    public float GetHealth()
    {
        return buildingHealth;
    }


    public override void EndQuest()
    {
        base.EndQuest();
        healthImage.transform.parent.gameObject.SetActive(false);
        FireArrow(false, buildingToDefend);
        if (buildingHealth > 0)
        {
            QuestSucceeded();
        }
        else
        {
            QuestFailed();
        }
    }

}
