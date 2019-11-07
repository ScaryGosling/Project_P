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
    public override void StartQuest()
    {
        buildingToDefend = buildings[Random.Range(0, buildings.Length)];
        buildingHealth = 100;
        healthImage.transform.parent.gameObject.SetActive(true);
        toggleArrow.goal = buildingToDefend;
        toggleArrow.toggle = true;
        EventSystem.Current.FireEvent(toggleArrow);
    }

    public GameObject GetBuilding()
    {
        return buildingToDefend;
    }

    public void TakeDamage(float damage)
    {
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
        toggleArrow.toggle = false;
        EventSystem.Current.FireEvent(toggleArrow);
    }

    public override void EndQuest()
    {
        toggleArrow.toggle = false;
        EventSystem.Current.FireEvent(toggleArrow);
        if (buildingHealth > 0)
        {
            QuestSucceeded();
        }
    }

}
