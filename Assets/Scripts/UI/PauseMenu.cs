using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject settingsPanelUI;

    //Array of AudioSources to pause/unpause individually
    public AudioSource[] audioSources;
    
    private void Start()
    {
        //Disable PauseMenu on StartUp
        pauseMenuUI.SetActive(false);
        settingsPanelUI.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        settingsPanelUI.SetActive(false);
        GameManager.instance.ResumeGame();
        PauseAudioSources();
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Confined;
    }
    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        settingsPanelUI.SetActive(false);
        GameManager.instance.PauseGame();
        PauseAudioSources();
        GameIsPaused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    private void PauseAudioSources()
    {
        if(GameManager.instance.IsGamePaused())
        {
            foreach(var audioSource in audioSources)
            {
                if (audioSource.isPlaying)  // Check if audio source is playing
                {
                    audioSource.Pause();
                }
            }
        }
        else
        {
            foreach(var audioSource in audioSources)
            {
                audioSource.UnPause();
            }
        }
    }
    public void ShowSettingsPanel()
    {
        if (settingsPanelUI != null)
        {
            settingsPanelUI.SetActive(true);
            pauseMenuUI.SetActive(false);
        }
    }
    public void HideSettingsPanel()
    {
        if (settingsPanelUI != null)
        {
            settingsPanelUI.SetActive(false);
            pauseMenuUI.SetActive(true);
        }
    }
    public void LoadMenu()
    {
        GameManager.instance.ResumeGame(); // Ensure time scale is reset
        if (SceneController.instance != null) 
        {
            SceneController.instance.LoadScene("MainMenu");
        }
        else
        {
            Debug.LogError("SceneController instance is not found.");
        }
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void RestartGame()
    {
        GameManager.instance.ResumeGame(); // Ensure time scale is reset
        if (SceneController.instance != null)
        {
            SceneController.instance.RestartGame();
        }
        else
        {
            Debug.LogError("SceneController instance is not found.");
        }
    }
    public void QuitGame()
    {
        if (SceneController.instance != null)
        {
            SceneController.instance.QuitGame();
        }
        else
        {
            Debug.LogError("SceneController instance is not found.");
        }
    }
}
