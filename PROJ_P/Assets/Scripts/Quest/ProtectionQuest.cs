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

    public override void StartQuest()
    {
        buildingIndex = Random.Range(0, buildings.Length);
        buildingToDefend = buildings[buildingIndex];
        buildingHealth = 100;
        healthImage.transform.parent.gameObject.SetActive(true);
        FireArrow(true);
        dialogueEvent.buildingIndex = buildingIndex;
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
            QuestFailed();
        }
    }

    public float GetHealth()
    {
        return buildingHealth;
    }

    protected override void QuestFailed()
    {
        healthImage.transform.parent.gameObject.SetActive(false);
        Debug.Log("Quest Failed");
        FireArrow(false);
    }

    public override void EndQuest()
    {
        FireArrow(false);
        if (buildingHealth > 0)
        {
            QuestSucceeded();
        }
    }

    private void FireArrow(bool toggle)
    {
        toggleArrow.goal = buildingToDefend;
        toggleArrow.toggle = toggle;
        EventSystem.Current.FireEvent(toggleArrow);
    }

}
