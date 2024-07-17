using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
        mainMenuButton.onClick.AddListener(ReturnToMainMenu);
    }

    public void ShowDeathMenu(int enemyKills, int moneyCollected, int fragmentsCollected)
    {
        deathMenuUI.SetActive(true);

        //Update text
        enemyKillText.text = "Enemies Killed: " + enemyKills;
        moneyCollectedText.text = "Money Collected: " + moneyCollected;
        fragmentsCollectedText.text = "Fragments: " + fragmentsCollected;

        GameManager.instance.PlayerDied();
    }
    public void ReturnToMainMenu()
    {
        GameManager.instance.ResumeGame();
        SceneController.instance.LoadScene("MainMenu");
    }
    public void ResetPlayerStats()
    {
        //Reset player stats
        GameManager.instance.ResetPlayerStats();
    }

}
