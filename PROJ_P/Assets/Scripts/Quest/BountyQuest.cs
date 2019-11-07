using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BountyQuest : Quest
{
    [SerializeField] private GameObject boss;
    [SerializeField] private SpawnArea bossSpawner;
    [SerializeField] private float lifetime = 20;
    private Unit bossStats;
    private Timer timer;
    private DialogueEvent dialogueEvent = new DialogueEvent();

    public override void StartQuest()
    {
        bossStats =  Instantiate(boss, bossSpawner.transform.position, Quaternion.identity).GetComponent<Unit>();
        timer = new GameObject("Timer").AddComponent<Timer>();
        timer.RunCountDown(lifetime, EndQuest, Timer.TimerType.DELAY);
        FireArrow(false, bossStats.gameObject);
    }
    public override void QuestDialogue()
    {
        dialogueEvent.buildingIndex = 0;
        EventSystem.Current.FireEvent(dialogueEvent);
    }
    private void Update()
    {
        if (bossStats != null && bossStats.Health <= 0)
        {
            FireArrow(false, bossStats.gameObject);
            QuestSucceeded();
        }
    }

    public override void EndQuest()
    {
        if (bossStats != null)
        {
            FireArrow(false, bossStats.gameObject);
            QuestFailed();
            DestroyBoss();
        }
    }

    private void DestroyBoss()
    {
        Destroy(bossStats.gameObject);
        bossStats = null;
    }
}
