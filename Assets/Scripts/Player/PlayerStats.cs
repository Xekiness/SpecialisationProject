using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerUpgradeSO;

public class PlayerStats : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dashSpeed = 10f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 2f;
    public int maxHealth = 100;

    public int currentHealth;

    public PlayerStats()
    {
        currentHealth = maxHealth;
    }

    public void ApplyUpgrade(PlayerUpgradeSO upgrade)
    {
        switch (upgrade.upgradeType)
        {
            case UpgradeType.MaxHealth:
                maxHealth += Mathf.RoundToInt(upgrade.effectStrength);
                break;
            case UpgradeType.HealthRegen:
                // Implement health regen logic

                break;
            case UpgradeType.MovementSpeed:
                moveSpeed += upgrade.effectStrength;
                break;
            case UpgradeType.DashRange:
                dashSpeed += upgrade.effectStrength;
                break;
            case UpgradeType.DashCooldown:
                dashCooldown -= upgrade.effectStrength;
                break;
        }
    }
}
