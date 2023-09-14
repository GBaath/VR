using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public AssetLoader assetLoader;
    public ScoreManager scoreManager;
    public RoomManager roomManager;

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


        SetAppSettings();
    }
    void SetAppSettings()
    {
        Application.targetFrameRate = 90;
    }
}
