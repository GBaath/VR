using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject playerPhysicsBase;
    public AssetLoader assetLoader;
    public AudioManager audioManager;
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
