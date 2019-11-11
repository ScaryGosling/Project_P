//Author: Emil Dahl
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectableListener : Quest
{
    private int gathered;
    private Text collectText;
    private GameObject objectSpawner;
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject objectSpawnerPrefab;
    [SerializeField] private string baseText = "Amount gathered: ";
    [SerializeField] private int toGather = 3;
    private DialogueEvent dialogueEvent = new DialogueEvent();
    [SerializeField] List<DialogueData> questData;

    public override void StartQuest()
    {
        EventSystem.Current.RegisterListener<CollectionEvent>(ManageCollected);

        Debug.Log("startedQuest");

        if (panel != null)
        {
            collectText = panel.GetComponentInChildren<Text>();
            DrawToCanvas();
            panel.SetActive(true);
        }

        if(objectSpawner == null)
        objectSpawner = Instantiate(objectSpawnerPrefab);
        objectSpawner.GetComponent<ObjectSpawner>().PopulateList(toGather, global::ObjectSpawner.ObjectToSpawn.Reward);

    }
    public override void QuestDialogue()
    {
        Debug.Log("coll");
        dialogueEvent.data = questData[0];
        EventSystem.Current.FireEvent(dialogueEvent);
    }

    void ManageCollected(CollectionEvent collected)
    {
        Debug.Log("manageCollected");
        gathered += collected.pickUpValue;
        DrawToCanvas();
        //Check success and failure
        if (gathered >= toGather)
            EndQuest();
    }

    void DrawToCanvas()
    {
        collectText.text = baseText + gathered;
    }


    public override void EndQuest()
    {
        if (gathered >= toGather)
            QuestSucceeded();
        else
        {
            QuestFailed();
        }
        gathered = 0;
        panel.SetActive(false);
        objectSpawner.GetComponent<ObjectSpawner>().TerminateSpawner();
    }


}
