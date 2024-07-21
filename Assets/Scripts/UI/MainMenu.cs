using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsPanelUI;
    public GameObject howToPlayPanelUI;

    private void Start()
    {
        settingsPanelUI.SetActive(false);
        howToPlayPanelUI.SetActive(false);  
    }
    public void ShowSettingsPanel()
    {
        if (settingsPanelUI != null)
        {
            settingsPanelUI.SetActive(true);
        }
    }
    public void HideSettingsPanel()
    {
        if (settingsPanelUI != null)
        {
            settingsPanelUI.SetActive(false);
        }
    }
    public void ShowHowToPlayPanel()
    {
        if (howToPlayPanelUI != null)
        {
            howToPlayPanelUI.SetActive(true);
        }
    }

    public void HideHowToPlayPanel()
    {
        if (howToPlayPanelUI != null)
        {
            howToPlayPanelUI.SetActive(false);
        }
    }
    public void GoToGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
