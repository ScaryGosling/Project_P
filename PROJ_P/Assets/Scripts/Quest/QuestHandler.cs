using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestHandler : MonoBehaviour
{
    [SerializeField] private Quest[] quests;
    [SerializeField] private TMP_Text questReminder;
    public static QuestHandler instance;

    private void Awake()
    {
        instance = this;
    }
    public Quest GetRandomQuest()
    {
        return quests[Random.Range(0, quests.Length)];
    }
    public TMP_Text GetQuestReminder()
    {
        return questReminder;
    }
}
