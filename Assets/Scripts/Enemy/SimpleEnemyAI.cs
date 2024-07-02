using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Health;

public class SimpleEnemyAI : MonoBehaviour
{
    public float speed = 2f;
    public float meleeRange = 1.5f;
    public float attackCooldown = 1f;

    private Transform player;
    private Animator animator;
    private bool isAttacking = false;
    private float nextAttackTime = 0f;

    public int damageDealtOnCollision = 10;
    public Health health;
    [SerializeField] private Healthbar healthbar;

    private SimpleEnemySpawner spawner; // Reference to the spawner
    public GameObject moneyPrefab;
    public GameObject experiencePrefab;
    public GameObject fragmentPrefab;
    public int moneyDrop = 10;
    public int experienceDrop = 5;
    public int fragmentDrop = 1;

    private bool isDying = false;
    private Collider2D enemyCollider;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponent<Animator>();
        enemyCollider = GetComponent<Collider2D>(); 

        if (health != null)
        {
            // Subscribe to the health changed event
            health.HealthChanged += OnHealthChanged;
            health.OnDeathEvent += OnDeath;
            // Initialize the health bar
            OnHealthChanged(health.GetCurrentHealth(), health.maxHealth);
        }
        else
        {
            Debug.LogError("Health component is not assigned to the enemy.");
        }

        if (animator == null)
        {
            Debug.LogError("Animator component is not assigned or found on the enemy.");
        }
    }
    private void OnDestroy()
    {
        if (health != null)
        {
            // Unsubscribe from the health changed event to prevent memory leaks
            health.HealthChanged -= OnHealthChanged;
            health.OnDeathEvent -= OnDeath;
        }
    }

    private void OnHealthChanged(int currentHealth, int maxHealth)
    {
        healthbar.UpdateHealthBar(maxHealth, currentHealth);

        if (currentHealth < maxHealth && currentHealth > 0)
        {
            //animator.SetTrigger("isHurt");
            StartCoroutine(TriggerHurtAnimation());
        }
    }
    void Update()
    {
        if (!GameManager.instance.IsPlayerAlive() || player == null)
        {
            animator.SetFloat("Speed", 0);
            return; // Do nothing if the player is dead or not found
        }

        if (isAttacking)
        {
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= meleeRange)
        {
            if (Time.time >= nextAttackTime)
            {
                StartCoroutine(Attack());
                nextAttackTime = Time.time + attackCooldown;
            }
        }
        else
        {
            MoveTowardsPlayer();
        }

        animator.SetFloat("Speed", speed);
    }

    void MoveTowardsPlayer()
    {
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        // Flip the enemy based on the direction
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(-3, 3, 3); // Facing right
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(3, 3, 3); // Facing left
        }
    }
    private IEnumerator TriggerHurtAnimation()
    {
        animator.SetTrigger("isHurt");
        yield return new WaitForSeconds(0.25f);
        animator.ResetTrigger("isHurt");
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetBool("isAttacking", true);
        Debug.Log("Enemy starts attacking");

        // Wait for the attack animation to finish
        yield return new WaitForSeconds(0.5f); // Adjust this based on your attack animation length

        // Perform the actual attack (e.g., deal damage to player if in range)
        if (Vector2.Distance(transform.position, player.position) <= meleeRange)
        {
            // Add your damage dealing code here
            player.GetComponent<Health>().TakeDamage(damageDealtOnCollision);
            Debug.Log("Enemy attacks player");
        }

        animator.SetBool("isAttacking", false);
        isAttacking = false;
        Debug.Log("Enemy finishes attacking");
    }

    private void OnDeath()
    {
        if (isDying) return;

        isDying = true; // Set the flag to true

        //Disable enemy AI and collider on death
        enabled = false;
        isAttacking = false;
        animator.SetTrigger("isDead");

        //Disable the collider
        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }
 
        // Decrease the enemy count in the spawner
        if (spawner != null)
        {
            spawner.DecreaseEnemyCount();
        }

        // Always drop experience
        InstantiatePickup(experiencePrefab, experienceDrop);

        // 50% Chance to drop money
        if (Random.value <= 0.5f)
        {
            InstantiatePickup(moneyPrefab, moneyDrop);
        }

        // 10% Chance to drop fragment
        if (Random.value <= 0.1f)
        {
            InstantiatePickup(fragmentPrefab, fragmentDrop);
        }

        Destroy(gameObject, 2f); // Adjust the delay based on your death animation length
    }
    private void InstantiatePickup(GameObject prefab, int value)
    {
        Vector3 offset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
        GameObject pickup = Instantiate(prefab, transform.position + offset, Quaternion.identity);
        pickup.GetComponent<PickUp>().value = value;
    }

    // Method to handle player death
    public void OnPlayerDeath()
    {
        // Stop enemy actions
        player = null;
        animator.SetBool("isAttacking", false);
        isAttacking = false;

        // Show death menu (implement your death menu logic here)
        Debug.Log("Player is dead. Show death menu.");
    }
}
