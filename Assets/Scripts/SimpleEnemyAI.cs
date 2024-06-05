using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemyAI : MonoBehaviour
{
    public float speed = 2f;
    public float meleeRange = 3.0f;
    public float attackCooldown = 1f;

    private Transform player;
    private Animator animator;
    private bool isAttacking = false;
    private float nextAttackTime = 0f;

    public Health health;
    public int damageDealtOnCollision = 10;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
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
    IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetBool("isAttacking", true);

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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Enemy collided with the player!");
            // Handle enemy interaction with the player here
        }
    }

}
