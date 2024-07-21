using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public GameObject deathMenuUI;
    public TextMeshProUGUI enemyKillText;
    public TextMeshProUGUI moneyCollectedText;
    public TextMeshProUGUI fragmentsCollectedText;
    public Button mainMenuButton;

    private void Start()
    {
        deathMenuUI.SetActive(false);
    }

    public void ShowDeathMenu(int enemyKills, int moneyCollected, int fragmentsCollected)
    {
        deathMenuUI.SetActive(true);
        Cursor.visible = true;
        enemyKillText.text = "Kills: " + enemyKills;
        moneyCollectedText.text = "Money: " + moneyCollected;
        fragmentsCollectedText.text = "Fragments: " + fragmentsCollected;
        Time.timeScale = 0f;
    }
    public void ReturnToMainMenu()
    {
        GameManager.instance.ResumeGame();
        SceneManager.LoadScene("MainMenu");
        ResetPlayerStats();
        Cursor.visible = true;
    }
    public void ResetPlayerStats()
    {
        //Reset player stats
        GameManager.instance.ResetPlayerStats();
    }

}
