using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject menuPanel;

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadCredits()
    {
        creditsPanel.SetActive(true);
        menuPanel.SetActive(false);
    }

    public void StartGame(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void CloseCreditsPanel()
    {
        creditsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }
}
