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
        if (instance && instance != this)
        {
            Destroy(this);
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
    }

    public void UpdateUI()
    {
        scoreText.text = score.ToString();
    }

}
