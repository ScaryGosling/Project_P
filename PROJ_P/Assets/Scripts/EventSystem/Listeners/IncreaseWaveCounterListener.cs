using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IncreaseWaveCounterListener : MonoBehaviour
{
    [SerializeField]private WaveCounter waveCounter;
    [SerializeField] private GameObject unitUpdater;
    [SerializeField] private Dialogue dialogue;

    void Start()
    {
        EventSystem.Current.RegisterListener<NewWaveEvent>(IncreaseWave);
        EventSystem.Current.RegisterListener<UnitsRemaining>(UpdateRemaining);
        EventSystem.Current.RegisterListener<DialogueEvent>(StartDialogue);
    }

    void IncreaseWave(NewWaveEvent newWave)
    {
        waveCounter.StartNewWave();
        waveCounter.UpdateCounter(newWave.waveIndex);
        
    }

    void UpdateRemaining(UnitsRemaining unitsRemaining)
    {
        unitUpdater.SetActive(true);
        unitUpdater.GetComponent<UnitUpdater>().ActivateUI(unitsRemaining.remaining);
    }

    void StartDialogue(DialogueEvent dialogueEvent)
    {
        dialogue.gameObject.SetActive(true);
        dialogue.InitText();
    }


}
