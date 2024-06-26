using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lifeTime = 2f;
    private float lifeTimer;

    public int damage = 10;


    void Start()
    {
        //Destroy(gameObject, lifeTime); // Destroy bullet after a certain time to avoid clutter

        // Reset the timer whenever the bullet is activated
        lifeTimer = lifeTime;

        // Start the coroutine to deactivate the bullet after lifeTime seconds
        StartCoroutine(DeactivateAfterTime(lifeTime));
    }

    void Update()
    {
        //transform.Translate(Vector2.up * speed * Time.deltaTime); // Move bullet upwards

        // Count down the timer and deactivate the bullet if it exceeds lifeTime
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Bullet hit enemy");
            Health enemyHealth = collision.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }

            // Destroy the bullet after hitting the enemy
            Destroy(gameObject);
        }
        //gameObject.SetActive(false); // Deactivate the bullet on collision
    }
    private IEnumerator DeactivateAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
