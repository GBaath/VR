using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject scorePanel;

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
    public void LoadHighscorePanel()
    {
        scorePanel.SetActive(true);
        creditsPanel.SetActive(false);
        menuPanel.SetActive(false);
    }
    public void CloseHighscorePanel()
    {
        scorePanel.SetActive(false);
        creditsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }
    public void CloseCreditsPanel()
    {
        creditsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }
}
