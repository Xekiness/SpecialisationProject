using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private bool isPlayerAlive = true;
    //private bool isGamePaused = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public bool IsPlayerAlive()
    {
        return isPlayerAlive;
    }

    public void PlayerDied()
    {
        isPlayerAlive = false;
        // Show death menu
        //FindObjectOfType<DeathMenu>().ShowDeathMenu();
    }
    public void Retry()
    {
        isPlayerAlive = true;
        //isGamePaused = false;
        Time.timeScale = 1f; // Resume the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload current scene
    }
    public void ReturnToMainMenu()
    {
        isPlayerAlive = true;
        //isGamePaused = false;
        Time.timeScale = 1f; // Resume the game
        SceneManager.LoadScene("MainMenu"); // Load main menu scene, ensure you have a scene named "MainMenu"
    }

}
