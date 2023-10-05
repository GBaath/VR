using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class ScoreData
{
    public int score;
    public int levelReached;
}
[System.Serializable]
public class HighScoresData
{
    public List<ScoreData> highScores = new List<ScoreData>();
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
    private const int MaxNumberOfHighScores = 10; //Number of highscores i can save


    private static ScoreSaver _instance;
    public static ScoreSaver Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }
    void Start()
    {
        DisplayHighScores();
        //UpdateScore();
        //LoadHighScores();
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
    public void SaveHighScore(int score, int levelReached)
    {
        highScoresData.highScores.Add(new ScoreData { score = score, levelReached = levelReached });
        highScoresData.highScores.Sort((a, b) => b.score.CompareTo(a.score));

        if (highScoresData.highScores.Count > MaxNumberOfHighScores)
        {
            highScoresData.highScores.RemoveAt(MaxNumberOfHighScores);
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
        LoadHighScores();

        string scoresString = " ";

        for (int i = 0; i < highScoresData.highScores.Count; i++)
        {
            scoresString += (i + 1).ToString() + ".Score:" + highScoresData.highScores[i].score.ToString() + " - Rooms: " + highScoresData.highScores[i].levelReached.ToString() + "\n";
        }

        bestScoreText.text = scoresString;
    }

    [ContextMenu("OnDeath")]
    public void OnDeath()
    {
        SaveHighScore(ScoreManager.instance.score, GameManager.instance.roomManager.roomsPassed);
        DisplayHighScores();
    }

}


