using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestHandler : MonoBehaviour
{
    [SerializeField] private Quest[] quests;


    public Quest GetRandomQuest()
    {
        Debug.Log("Random quest generated;");
        return quests[Random.Range(0, quests.Length)];
    }

}
