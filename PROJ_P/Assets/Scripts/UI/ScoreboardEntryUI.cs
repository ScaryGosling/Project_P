using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreboardEntryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI entryPositionText = null;
    [SerializeField] private TextMeshProUGUI entryNameText = null;
    [SerializeField] private TextMeshProUGUI entryScoreText = null;

    public void Initialize(ScoreboardEntryData scoreboardEntry, int position)
    {
        entryNameText.text = scoreboardEntry.entryName;
        entryScoreText.text = scoreboardEntry.entryScore.ToString();
        entryPositionText.text = position.ToString();
    }
}
