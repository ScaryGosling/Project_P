//Author: Emil Dahl
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class takes care of the collection quest logic. 
/// </summary>
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
    private bool questEnded = false;

    protected override void Start()
    {
        base.Start();
        EventSystem.Current.RegisterListener<CollectionEvent>(ManageCollected);
    }
    /// <summary>
    /// Populates the map with rewards, this with the help of the object spawner. 
    /// </summary>
    public override void StartQuest()
    {
        base.StartQuest();
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
        questEnded = false;
    }

    public override void QuestDialogue()
    {
        dialogueEvent.data = questData[0];
        EventSystem.Current.FireEvent(dialogueEvent);
    }

    /// <summary>
    /// Checks if quest was completed and terminates quest items.
    /// </summary>
    public override void EndQuest()
    {
        base.EndQuest();
        if (objectSpawner && !questEnded)
        {
            base.EndQuest();

            if (gathered >= toGather)
            {
                QuestSucceeded();
            }
            else
            {
                objectSpawner.TerminateSpawner();
                QuestFailed();
            }
            gathered = 0;
            questEnded = true;
            collectionTextObject.SetActive(false);
        }
    }

    /// <summary>
    /// Updates quest progress.
    /// </summary>
    /// <param name="collected"></param>
    void ManageCollected(CollectionEvent collected)
    {
        gathered += collected.pickUpValue;
        DrawToCanvas();

        if (gathered >= toGather)
        {
            EndQuest();
        }
    }
    /// <summary>
    /// Informs player about the current quest progress.
    /// </summary>
    void DrawToCanvas()
    {
        collectText.text = baseText + gathered;
    }


}
