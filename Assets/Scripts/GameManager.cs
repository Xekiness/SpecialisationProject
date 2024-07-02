using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private bool isPlayerAlive = true;
    //private bool isGamePaused = false;
    public int totalMoney = 0;
    public int totalExperience = 0;
    public int totalFragments = 0;

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

    public void AddMoney(int amount)
    {
        totalMoney += amount;
        Debug.Log("Money added: " + amount);
    }
    public void AddExperience(int amount)
    {
        totalExperience += amount;
        Debug.Log("Experience added: " + amount);
    }
    public void AddFragments(int amount)
    {
        totalFragments += amount;
        Debug.Log("Fragments added: " + amount);
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
