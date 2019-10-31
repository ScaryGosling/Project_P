using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IncreaseWaveCounterListener : MonoBehaviour
{
    [SerializeField]private TMP_Text text;

    void Start()
    {
        EventSystem.Current.RegisterListener<NewWaveEvent>(IncreaseCounter);

    }

    void IncreaseCounter(NewWaveEvent newWave)
    {
        text.text = newWave.waveIndex.ToString();

    }

}
