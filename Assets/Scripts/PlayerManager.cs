using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    public PlayerHud ui;
    [SerializeField] private PlayerLevelUpgradeManager playerUpgrade;

    public int level;
    public int exp;

    private float maxHealth = 100f;
    private float currentHealth;
    private float healthRegen = 0f;
    private float pickupRange = 1f;
    private float movementSpeed = 5f;
    private float dashSpeed = 10f;
    private float dashCooldown = 1f;

    public int money;
    public int fragment;
    public int experience;

    private void Start()
    {

    }

    public void IncreaseMaxHealth(float amount)
    {
        maxHealth += amount;
        currentHealth += amount; //Optionally heal the player for the amt increased
    }
    public void IncreaseHealthRegen(float amount)
    {
        healthRegen += amount;
    }
    public void IncreasePickupRange(float amount)
    {
        pickupRange += amount;
    }
    public void IncreaseMovementSpeed(float amount)
    {
        movementSpeed += amount;
    }
    public void IncreaseDashSpeed(float amount)
    {
        dashSpeed += amount;
    }
    public void ReduceDashCooldown(float amount)
    {
        dashCooldown -= amount;
        if (dashCooldown < 0.1f) dashCooldown = 0.1f; //Set a minimum cooldown
    }
    private void Update()
    {
        //Apply health regen
        if(healthRegen > 0)
        {
            currentHealth = Mathf.Min(currentHealth + healthRegen * Time.deltaTime, maxHealth);
        }
    }
}
