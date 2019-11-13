using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GatheringQuest : Quest
{
    private int gathered;
    private Text collectText;
    private GameObject spawner;
    private ObjectSpawner objectSpawner;
    [SerializeField] private GameObject collectionTextObject;
    [SerializeField] private GameObject objectSpawnerPrefab;
    [SerializeField] private string baseText = "Amount gathered: ";
    [SerializeField] private int toGather = 3;
    private DialogueEvent dialogueEvent = new DialogueEvent();
    [SerializeField] List<DialogueData> questData;

    private void Start()
    {
        EventSystem.Current.RegisterListener<CollectionEvent>(ManageCollected);
    }
    public override void StartQuest()
    {
        if (collectionTextObject != null)
        {
            collectText = collectionTextObject.GetComponentInChildren<Text>();
            DrawToCanvas();
            collectionTextObject.SetActive(true);
        }

        if (spawner == null)
        {
            spawner = Instantiate(objectSpawnerPrefab);
        }
        objectSpawner = spawner.GetComponent<ObjectSpawner>();
        objectSpawner.PopulateList(toGather, ObjectSpawner.ObjectToSpawn.Reward);
    }
    public override void QuestDialogue()
    {
        dialogueEvent.data = questData[0];
        EventSystem.Current.FireEvent(dialogueEvent);
    }
    public override void EndQuest()
    {
        if (objectSpawner)
        {
            objectSpawner.TerminateSpawner();
            if (gathered >= toGather)
            {
                QuestSucceeded();
            }
            else
            {
                QuestFailed();
            }
            gathered = 0;
            collectionTextObject.SetActive(false);
        }

    }
    void ManageCollected(CollectionEvent collected)
    {
        gathered += collected.pickUpValue;
        DrawToCanvas();

        //Check success and failure
        if (gathered >= toGather)
        {

            EndQuest();
        }
    }
    void DrawToCanvas()
    {
        collectText.text = baseText + gathered;
    }


}
