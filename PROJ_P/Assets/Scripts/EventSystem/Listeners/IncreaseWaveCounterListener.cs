using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IncreaseWaveCounterListener : MonoBehaviour
{
    [SerializeField]private WaveCounter waveCounter;

    void Start()
    {
        EventSystem.Current.RegisterListener<NewWaveEvent>(IncreaseCounter);
    }

    void IncreaseCounter(NewWaveEvent newWave)
    {
        waveCounter.StartNewWave();
        waveCounter.UpdateCounter(newWave.waveIndex);
        
    }

}
