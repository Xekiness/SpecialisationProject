using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempShooter : MonoBehaviour
{
    public GameObject projectilePrefab; // Reference to the projectile prefab
    public Transform firePoint; // Point from where the projectile will be fired
    public float projectileSpeed = 10f; // Speed of the projectile

    // Update is called once per frame
    void Update()
    {
        // Check for input to shoot (e.g., pressing the spacebar)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Instantiate a new projectile at the firePoint position and rotation
        GameObject newProjectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // Get the Rigidbody2D component of the projectile
        Rigidbody2D rb = newProjectile.GetComponent<Rigidbody2D>();

        // Check if the Rigidbody2D component exists
        if (rb != null)
        {
            // Set the velocity of the projectile in the direction of the firePoint's forward vector
            rb.velocity = firePoint.up * projectileSpeed;
        }
        else
        {
            Debug.LogWarning("Projectile prefab does not have a Rigidbody2D component.");
        }
    }
}
