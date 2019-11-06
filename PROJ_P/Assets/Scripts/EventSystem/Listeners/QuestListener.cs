using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestListener : MonoBehaviour
{
    GameObject building;
    void Start()
    {
        EventSystem.Current.RegisterListener<ProtectionQuestEvent>(AssignBuilding);
    }

    private void AssignBuilding(ProtectionQuestEvent protectionQuest)
    {
        building = protectionQuest.building;
    }
}
