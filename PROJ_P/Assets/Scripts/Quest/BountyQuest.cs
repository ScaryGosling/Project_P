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
    [SerializeField] private int index = 1;
    [SerializeField] List<DialogueData> questData;

    public override void StartQuest()
    {
        base.StartQuest();
        bossStats =  Instantiate(boss, bossSpawner.transform.position, Quaternion.identity).GetComponent<Unit>();
        timer = new GameObject("Timer").AddComponent<Timer>();
        timer.RunCountDown(lifetime, EndQuest, Timer.TimerType.DELAY);
        FireArrow(true, bossStats.gameObject);
    }
    public override void QuestDialogue()
    {
        dialogueEvent.data = questData[Random.Range(0, questData.Count)];
        EventSystem.Current.FireEvent(dialogueEvent);
    }
    private void Update()
    {
        if (bossStats != null && bossStats.Health <= 0)
        {
            FireArrow(false, bossStats.gameObject);
            bossStats = null;
            QuestSucceeded();
        }
    }

    public override void EndQuest()
    {
        base.EndQuest();
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
