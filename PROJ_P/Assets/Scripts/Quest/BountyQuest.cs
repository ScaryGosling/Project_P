using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private GameObject bossTimerObject;
    private Text bossTimerText;
    private string baseText = "Time left: ";


    protected override void Start()
    {
        base.Start();
        bossTimerText = bossTimerObject.GetComponent<Text>();
    }
    public override void StartQuest()
    {
        base.StartQuest();
        bossStats =  Instantiate(boss, bossSpawner.transform.position, Quaternion.identity).GetComponent<Unit>();
        timer = BowoniaPool.instance.GetFromPool(PoolObject.TIMER).GetComponent<Timer>();
        timer.RunCountDown(lifetime, EndQuest, Timer.TimerType.DELAY);
        FireArrow(true, bossStats.gameObject);
        if (timer != null)
        {
            bossTimerObject.SetActive(true);
        }
    }
    public override void QuestDialogue()
    {
        dialogueEvent.data = questData[Random.Range(0, questData.Count)];
        EventSystem.Current.FireEvent(dialogueEvent);
    }
    private void Update()
    {
        if (timer != null)
        {
            DrawToCanvas();
        }
        if (bossStats != null)
            if (bossStats != null && bossStats.Health <= 0)
            {
                EndQuest();
            }
    }

    private void DrawToCanvas()
    {
        bossTimerText.text = baseText + ((int)timer.Countdown).ToString("00") /*+ ":" + ((timer.Countdown % 1) * 100).ToString("00")*/;
    }

    public override void EndQuest()
    {
        base.EndQuest();
        bossTimerObject.SetActive(false);
        if (bossStats != null)
        {
            FireArrow(false, bossStats.gameObject);
            if (bossStats.Health <= 0)
            {
                QuestSucceeded();
            }
            else
            {
                QuestFailed();
            }
            DestroyBoss();
        }
    }

    private void DestroyBoss()
    {
        Destroy(bossStats.gameObject);
        timer.CancelMethod();
        timer = null;
        bossStats = null;
    }
}
