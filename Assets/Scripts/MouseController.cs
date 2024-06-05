using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    [SerializeField]
    private GameObject gun;  // Reference to the gun GameObject
    [SerializeField]
    private GameObject bulletPrefab;  // Reference to the bullet prefab
    [SerializeField]
    private Transform firePoint;  // Reference to the fire point on the gun
    [SerializeField]
    private float bulletSpeed = 10f;
    [SerializeField]
    private Camera cam;  // Reference to the main camera

    private SpriteRenderer gunSpriteRenderer;
    private Rigidbody2D playerRb;

    Vector2 mousePos;

    void Start()
    {
        gunSpriteRenderer = gun.GetComponent<SpriteRenderer>();
        // Ensure cam is set, otherwise find the main camera
        if (cam == null)
        {
            cam = Camera.main;
        }
    }

    public void SetPlayerRigidbody(Rigidbody2D rb)
    {
        playerRb = rb;
    }

    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetButtonDown("Fire1"))  // Default Fire1 button is left mouse button
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        RotateGunTowardsMouse();
    }

    void RotateGunTowardsMouse()
    {
        if (playerRb == null) return;

        Vector2 lookDir = mousePos - playerRb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        //rb.rotation = angle;
        playerRb.rotation = angle;
    }

    void Shoot()
    {
        // Get the mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Ensure the z position is zero for 2D

        // Calculate the direction from the fire point to the mouse position
        Vector2 direction = (mousePosition - firePoint.position).normalized;

        // Instantiate the bullet at the fire point
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Get the Rigidbody2D component of the bullet
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        // Apply velocity to the bullet towards the mouse position
        rb.velocity = direction * bulletSpeed;

        Debug.DrawLine(firePoint.position, mousePosition, Color.red, 1f); // This will draw a red line for 1 second
    }

    void FlipGunSprite()
    {
        // Get the mouse position in screen space
        Vector3 mouseScreenPosition = Input.mousePosition;

        // Get the screen width
        float screenWidth = Screen.width;

        // Check if the mouse is on the left or right half of the screen
        if (mouseScreenPosition.x < screenWidth / 2)
        {
            // Mouse is on the left half
            gun.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            // Mouse is on the right half
            gun.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
