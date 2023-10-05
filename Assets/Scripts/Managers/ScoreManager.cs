using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;


    public TextMeshProUGUI scoreText;

    public int score;

    void Awake()
    {
        instance = this;
    }

    public void UpdateUI()
    {
        scoreText.text = score.ToString();
    }

}
