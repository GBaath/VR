using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


[System.Serializable]
public class HighScoresData
{
    public List<int> highScores = new List<int>();
}
public class ScoreSaver : MonoBehaviour
{
    //TODO: Make score variables read only, so other scripts can read but not change the value.
    //public int score;
    public int bestScore;
    //public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bestScoreText;
    //For saving score
    public HighScoresData highScoresData = new HighScoresData(); // Use the HighScoresData object directly
    private const string highScoreKeyString = "HighScores"; //Key for getting HighScores
    private const int MaxNumberOfHighScores = 5; //Number of highscores i can save


    private static ScoreSaver _instance;
    public static ScoreSaver Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        //UpdateScore();
        LoadHighScores();
    }

    //public void UpdateScore()
    //{
    //    //Done: format score to display it like this as 000100
    //    scoreText.text = string.Format("SCORE: {0:000000}", score);
    //}

    ////Done: add functionallity for gaining score.
    //public void AddScore(int addedScore)
    //{
    //    score += addedScore;
    //    UpdateScore();
    //}



    //TODO: add highscore functionallity that saves the best score to player prefs.
    public void SaveHighScore(int score)
    {
        highScoresData.highScores.Add(score);
        highScoresData.highScores.Sort((a, b) => b.CompareTo(a)); // Sort in descending order, need to use this more often now that i know what it is
        if (highScoresData.highScores.Count > MaxNumberOfHighScores)
        {
            highScoresData.highScores.RemoveAt(MaxNumberOfHighScores); // Remove the lowest score
        }
        SaveHighScores();
    }

    private void SaveHighScores()
    {
        // Convert the HighScoresData object into a JSON string
        string json = JsonUtility.ToJson(highScoresData);
        PlayerPrefs.SetString(highScoreKeyString, json);
        PlayerPrefs.Save();
    }

    public void LoadHighScores()
    {
        string json = PlayerPrefs.GetString(highScoreKeyString, ""); // Get JSON string, "" default value if there is no highScores saved
        if (!string.IsNullOrEmpty(json))
        {
            highScoresData = JsonUtility.FromJson<HighScoresData>(json); // Turn the string back into C# object
        }
    }
    public void DisplayHighScores()
    {
        LoadHighScores(); // Ensure the latest scores are loaded from PlayerPrefs

        string scoresString = " "; /*= "High Scores:\n";*/

        for (int i = 0; i < highScoresData.highScores.Count; i++)
        {
            scoresString += (i + 1).ToString() + ".     " + highScoresData.highScores[i].ToString() + "\n";
        }

        bestScoreText.text = scoresString;
    }
    [ContextMenu("OnDeath")]
    public void OnDeath()
    {
        SaveHighScore(ScoreManager.instance.score);
        //DisplayHighScores();

    }

}


