using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    [SerializeField] private int currentHealth;

    // Delegate and event for health change notification
    public delegate void OnHealthChanged(int currentHealth, int maxHealth);
    public event OnHealthChanged HealthChanged;

    // Delegate and event for death notification
    public delegate void OnDeath();
    public event OnDeath OnDeathEvent;

    void Start()
    {
        currentHealth = maxHealth;
        // Trigger the event at the start to initialize the health bar
        HealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        else
        {
            // Notify subscribers about the health change
            HealthChanged?.Invoke(currentHealth, maxHealth);
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

        //gameObject.SetActive(false);

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

}
