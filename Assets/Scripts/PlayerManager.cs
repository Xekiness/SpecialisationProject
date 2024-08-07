using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    public PlayerHud ui;
    [SerializeField] private PlayerLevelUpgradeManager playerUpgrade;
    [SerializeField] private Health health; // Assuming Health script handles health management

    public int level;
    public int exp;
    public int money;
    public int fragment;
    public int experience;

    private void Start()
    {
        // Initialize UI
        UpdateHud();
    }
    private void UpdateHud()
    {
        if (ui != null)
        {
            ui.UpdateLevel(level);
            ui.UpdateExperienceBar(experience, playerData.experienceToNextLevel);
            ui.UpdateEnemiesKilled(playerData.enemiesKilled);
            ui.UpdateMoney(money);
            ui.UpdateFragments(fragment);
        }
    }

    public void IncreaseMaxHealth(int amount)
    {
        health.SetMaxHealth(health.maxHealth + amount); 
        health.Heal(amount);
    }
    public void IncreaseMovementSpeed(float amount)
    {
        playerData.movementSpeed += amount;
    }
    public void IncreaseDashSpeed(float amount)
    {
        playerData.dashRange += amount;
    }
    public void ReduceDashCooldown(float amount)
    {
        playerData.dashCooldown -= amount;
        if (playerData.dashCooldown < 0.1f) playerData.dashCooldown = 0.1f; //Set a minimum cooldown
    }
    public void LevelUp()
    {
        level++;
        playerUpgrade.ShowLevelUpOptions();
    }
}
