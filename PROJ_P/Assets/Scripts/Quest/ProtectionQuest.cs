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
    public override void StartQuest()
    {
        buildingToDefend = buildings[Random.Range(0, buildings.Length)];
        buildingHealth = 100;
        healthImage.transform.parent.gameObject.SetActive(true);
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

    protected override void QuestFailed()
    {
        healthImage.transform.parent.gameObject.SetActive(false);
        Debug.Log("Quest Failed");
    }

    public override void EndQuest()
    {
        if (buildingHealth > 0)
        {
            QuestSucceeded();
        }
    }

}
