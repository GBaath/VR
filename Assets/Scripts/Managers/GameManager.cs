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
    public DestroyWeapons dw;

    [Tooltip("DOESN'T WORK AT THE MOMENT! How many enemies may chase the target at once? Set value to (0) if they may chase without waiting altogether.")]
    public int enemiesToChaseAtOnce = 0;

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
