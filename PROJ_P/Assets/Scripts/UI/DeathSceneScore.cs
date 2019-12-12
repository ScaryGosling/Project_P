using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathSceneScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI returnToMain = null;
    [SerializeField] private string scoreAndReturn = "Save and return to Main Menu";
    [SerializeField] private string noScoreAndReturn = "Return to Main Menu";
    [SerializeField] private TextMeshProUGUI seeHighscores = null;
    [SerializeField] private string scoreAndSee = "Save and see Highscores";
    [SerializeField] private string noScoreAndSee = "See Highscores";
    [SerializeField] private TextMeshProUGUI tryAgain = null;
    [SerializeField] private string scoreAndTry = "Save and try again";
    [SerializeField] private string noScoreAndTry = "Try again";
    [SerializeField] private RectTransform icon = null;
    [SerializeField] private TextMeshProUGUI playScore = null;
    [SerializeField] private Scoreboard scoreboard;

    void Start()
    {
        playScore.text = PlayerPrefs.GetInt("Score", 0).ToString();
        if (scoreboard.CanSave() && PlayerPrefs.GetInt("Score", 0)> 0)
        {
            returnToMain.text = scoreAndReturn;
            seeHighscores.text = scoreAndSee;
            tryAgain.text = scoreAndTry;
        }
        else
        {
            returnToMain.text = noScoreAndReturn;
            seeHighscores.text = noScoreAndSee;
            tryAgain.text = noScoreAndTry;
            icon.anchorMin = new Vector2(icon.anchorMin.x, 0.3f);
            gameObject.SetActive(false);
        }
    }

}
