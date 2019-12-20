using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ScoreboardEntryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI entryPositionText = null;
    [SerializeField] private TextMeshProUGUI entryNameText = null;
    [SerializeField] private TextMeshProUGUI entryScoreText = null;
    [SerializeField] private Image playerClass = null;
    [SerializeField] private TextMeshProUGUI waveText = null;
    [SerializeField] private Sprite[] classIcons ;

    public void Initialize(ScoreboardEntryData scoreboardEntry, int position)
    {
        entryNameText.text = scoreboardEntry.entryName;
        entryScoreText.text = scoreboardEntry.entryScore.ToString();
        entryPositionText.text = position.ToString();
        waveText.text = scoreboardEntry.wave.ToString();
        if (scoreboardEntry.playerClass > 0 )
        {
            playerClass.sprite = classIcons[scoreboardEntry.playerClass-1];
        }

    }
}
