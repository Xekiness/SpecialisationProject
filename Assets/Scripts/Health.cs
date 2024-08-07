using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static PlayerUpgradeSO;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    [SerializeField] private int currentHealth;
    [SerializeField] private float healthRegenRate = 0f; // Health points regenerated per second

    // Delegate and event for health change notification
    public delegate void OnHealthChanged(int currentHealth, int maxHealth);
    public event OnHealthChanged HealthChanged;

    // Delegate and event for death notification
    public delegate void OnDeath();
    public event OnDeath OnDeathEvent;

    private Coroutine regenCoroutine;

    void Start()
    {
        currentHealth = maxHealth;
        // Trigger the event at the start to initialize the health bar
        HealthChanged?.Invoke(currentHealth, maxHealth);

        // Start the health regeneration coroutine only if it's the player
        if (GetComponent<PlayerStats>() != null)
        {
            regenCoroutine = StartCoroutine(RegenerateHealth());
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            HealthChanged?.Invoke(currentHealth, maxHealth);
            Die();
        }
        else
        {
            // Notify subscribers about the health change
            HealthChanged?.Invoke(currentHealth, maxHealth);
            if (gameObject.CompareTag("Player"))
                GetComponent<PlayerController>().PlayHurtEffects();
        }
        Debug.Log(gameObject.name + " HP after taking dmg: " + currentHealth);
    }

    void Die()
    {
        // Notify subscribers about the death
        OnDeathEvent?.Invoke();

        // Handle death (destroy the GameObject, play an animation, etc.)
        Debug.Log(gameObject.name + " died.");

        if (gameObject.CompareTag("Player"))
        {
            GameManager.instance.PlayerDied();
        }

        // Deactivate the GameObject after a delay
        StartCoroutine(DeactivateAfterDelay(4f)); 
    }

    IEnumerator DeactivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
    public void SetMaxHealth(int newMaxHealth)
    {
        maxHealth = newMaxHealth;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        HealthChanged?.Invoke(currentHealth, maxHealth);
    }
    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        HealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void ApplyUpgrade(PlayerUpgradeSO upgrade)
    {
        if (GetComponent<PlayerStats>() == null) return; // Skip upgrade if it's not the player

        switch (upgrade.upgradeType)
        {
            case UpgradeType.MaxHealth:
                SetMaxHealth(maxHealth + Mathf.FloorToInt(upgrade.effectStrength));
                Heal(Mathf.FloorToInt(upgrade.effectStrength));
                break;
            case UpgradeType.HealthRegen:
                healthRegenRate += upgrade.effectStrength;
                break;
        }
    }

    private IEnumerator RegenerateHealth()
    {
        while (true)
        {
            if (currentHealth < maxHealth)
            {
                currentHealth += Mathf.FloorToInt(healthRegenRate * Time.deltaTime);
                if (currentHealth > maxHealth)
                {
                    currentHealth = maxHealth;
                }
                HealthChanged?.Invoke(currentHealth, maxHealth);
            }
            yield return null;
        }
    }
}
