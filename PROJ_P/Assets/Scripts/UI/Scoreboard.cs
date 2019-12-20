using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using TMPro;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
    [SerializeField] private int maxScoreboardEntries = 100;
    [SerializeField] private Transform highscoresHolderTransform = null;
    [SerializeField] private GameObject scoreboardEntryObject = null;
    [SerializeField] private ScoreboardEntryUI firstPlace = null;
    [SerializeField] private TextMeshProUGUI entryName = null;
    [SerializeField]ScoreboardEntryData testEntryData = new ScoreboardEntryData();


    private static string SavePath => $"{Application.persistentDataPath}/highscore.ss";
    [SerializeField] private bool showUI;
    private void Start()
    {
        ScoreboardSaveData savedScores = GetSavedScores();
        if (showUI)
        {

        UpdateUI(savedScores);
        }
        SaveScores(savedScores);
    }
    public bool CanSave()
    {

        if (GetSavedScores().highscores.Count >0)
        {
            if (GetSavedScores().highscores.Count< maxScoreboardEntries)
            {
                return true;
            }
            else
            {
                return PlayerPrefs.GetInt("Score", 0) > GetSavedScores().highscores[GetSavedScores().highscores.Count - 1].entryScore;
            }


        }
        else
        {
            return true;
        }
    }
    public void AddScore()
    {
        if (CanSave() && PlayerPrefs.GetInt("Score", 0) > 0)
        {
            ScoreboardEntryData entryData = new ScoreboardEntryData();
            entryData.entryScore = PlayerPrefs.GetInt("Score", 0);
            entryData.wave = PlayerPrefs.GetInt("Wave", 0);
            entryData.playerClass = PlayerPrefs.GetInt("Class", 0); 

            if (entryName.text.Length <= 1)
            {
                entryData.entryName = "-Unknown-";
            }
            else
            {

            entryData.entryName = entryName.text;
            }

            AddEntry(entryData);
        }

    }
    private void UpdateUI(ScoreboardSaveData savedScores)
    {
        int position = 1;
        firstPlace.gameObject.SetActive(false);
        foreach (Transform child in highscoresHolderTransform)
        {
            Destroy(child.gameObject);
        }
        if (savedScores.highscores.Count > 0)
        {
            firstPlace.gameObject.SetActive(true);
            firstPlace.Initialize(savedScores.highscores[0], position);
        }
        for (int i = 1; i < savedScores.highscores.Count; i++)
        {
            if (savedScores.highscores[i].entryScore != savedScores.highscores[i-1].entryScore)
            {
                position = i +1;
            }

            Instantiate(scoreboardEntryObject, highscoresHolderTransform).GetComponent<ScoreboardEntryUI>().Initialize(savedScores.highscores[i], position);
        }
        Debug.Log(PlayerPrefs.GetInt("PositionAdded", 0)-1);
        StartCoroutine(GoToPosition());
    }



    [SerializeField]ScrollRect scrollRect;

    IEnumerator GoToPosition()
    {
        yield return new WaitForSeconds(0.05f);
        if (PlayerPrefs.GetInt("PositionAdded", 0) > 0)
        {
            UIExtensions.ScrollToCeneter(scrollRect, highscoresHolderTransform.gameObject.GetComponent<RectTransform>().GetChild(PlayerPrefs.GetInt("PositionAdded", 0) - 1).gameObject.GetComponent<RectTransform>());
            PlayerPrefs.SetInt("PositionAdded", 0);
        }
    }

    public void AddEntry(ScoreboardEntryData scoreboardEntryData)
    {
        ScoreboardSaveData savedScores = GetSavedScores();
        bool scoreAdded = false;
        for (int i = 0; i < savedScores.highscores.Count; i++)
        {
            if (scoreboardEntryData.entryScore > savedScores.highscores[i].entryScore)
            {
                savedScores.highscores.Insert(i, scoreboardEntryData);
                PlayerPrefs.SetInt("PositionAdded", i);
                scoreAdded = true;
                break;
            }
        }
        if (!scoreAdded && savedScores.highscores.Count < maxScoreboardEntries)
        {
            savedScores.highscores.Add(scoreboardEntryData);
            PlayerPrefs.SetInt("PositionAdded", savedScores.highscores.Count - 1);
        }
        if (savedScores.highscores.Count > maxScoreboardEntries)
        {
            savedScores.highscores.RemoveRange(maxScoreboardEntries, savedScores.highscores.Count - maxScoreboardEntries);
        }
        if (showUI)
        {

        UpdateUI(savedScores);
        }
        SaveScores(savedScores);
    }
    [ContextMenu("Add test")]
    public void AddTestEntry()
    {
        AddEntry(testEntryData);
    }
    private ScoreboardSaveData GetSavedScores()
    {
        if (!File.Exists(SavePath))
        {
            File.Create(SavePath).Dispose();
            return new ScoreboardSaveData();
        }
        using (StreamReader stream = new StreamReader(SavePath))
        {
            string json = stream.ReadToEnd();
            return JsonUtility.FromJson<ScoreboardSaveData>(json);
        }
    }

    private void SaveScores(ScoreboardSaveData scoreboardSaveData)
    {
        using (StreamWriter stream = new StreamWriter(SavePath))
        {
            string json = JsonUtility.ToJson(scoreboardSaveData, true);
            stream.Write(json);
        }
    }
    [ContextMenu("reset")]
    public void ResetScores()
    {
        ScoreboardSaveData savedScores = GetSavedScores();


        savedScores.highscores.Clear();



        if (showUI)
        {

        UpdateUI(savedScores);
        }
        SaveScores(savedScores);
    }
}
