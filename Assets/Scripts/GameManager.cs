using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private bool isPlayerAlive = true;
    private bool isGamePaused = false;

    public PlayerData playerData; //Ref to scriptable obj

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
        isGamePaused = true;
        // Show death menu
        //Show death menu
        //DeathMenu deathMenu = FindObjectOfType<DeathMenu>();
        //deathMenu.ShowDeathMenu(playerData.enemiesKilled, playerData.totalMoney, playerData.totalFragments);

        DeathMenu deathMenu = FindObjectOfType<DeathMenu>();
        deathMenu.ReturnToMainMenu();

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
        SceneManager.LoadScene("MainMenu"); // Load main menu scene
    }

    public void AddMoney(int amount)
    {
        playerData.AddMoney(amount);
        UpdateHUD();
    }
    public void AddExperience(int amount)
    {
        playerData.AddExperience(amount);
        UpdateHUD();
    }

    public void AddFragments(int amount)
    {
        playerData.AddFragments(amount);
        UpdateHUD();
    }
    public void ResetPlayerStats()
    {
        playerData.currentExperience = 0;
        playerData.totalMoney = 0;
        playerData.enemiesKilled = 0;
    }
    public void EnemyKilled()
    {
        playerData.AddEnemyKilled();
        UpdateHUD();
    }

    private void UpdateHUD()
    {
        PlayerHud playerHud = FindObjectOfType<PlayerHud>();
        if (playerHud != null)
        {
            playerHud.UpdateExperienceBar(playerData.currentExperience, playerData.experienceToNextLevel);
            playerHud.UpdateLevel(playerData.currentLevel);
            playerHud.UpdateEnemiesKilled(playerData.enemiesKilled);
            playerHud.UpdateMoney(playerData.totalMoney);
            playerHud.UpdateFragments(playerData.totalFragments);
        }
    }
    public void PauseGame()
    {
        if (isGamePaused) return;

        Time.timeScale = 0f;
        isGamePaused = true;
        
    }
    public void ResumeGame()
    {
        if (!isGamePaused) return;

        Time.timeScale = 1f;
        isGamePaused = false;

    }
    public bool IsGamePaused()
    {
        return isGamePaused;
    }
}
