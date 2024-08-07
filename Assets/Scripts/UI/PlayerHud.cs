using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHud : MonoBehaviour
{
    [SerializeField] private Image experienceBar;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI enemiesKilledText;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI fragmentsText;

    private void Start()
    {
        LevelingSystem.OnLevelUp += OnLevelUp;
        experienceBar.fillAmount = 0;
    }
    private void OnDestroy()
    {
        LevelingSystem.OnLevelUp -= OnLevelUp;
    }

    public void UpdateExperienceBar(float currentExp, float experienceToNextLevel)
    {
        experienceBar.fillAmount = currentExp / experienceToNextLevel;
    }
    public void UpdateLevel(int level)
    {
        levelText.text = "LvL:" + level.ToString();
    }
    public void UpdateEnemiesKilled(int enemiesKilled)
    {
        enemiesKilledText.text = enemiesKilled.ToString();
    }
    public void UpdateMoney(int money)
    {
        moneyText.text = money.ToString();
    }
    public void UpdateFragments(int fragments)
    {
        fragmentsText.text = fragments.ToString();
    }
    private void OnLevelUp()
    {
        LevelingSystem levelingSystem = GameObject.FindObjectOfType<LevelingSystem>();
        UpdateLevel(levelingSystem.currentLevel);
        UpdateExperienceBar(levelingSystem.currentExperience, levelingSystem.experienceToNextLevel);
        //Show Level Up UI.
    }
}
