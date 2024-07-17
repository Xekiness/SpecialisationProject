using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Sniper : RangedWeapon
{
    [SerializeField] private GameObject sniperBulletPrefab;
    [SerializeField] private Transform sniperFirePoint;
    [SerializeField] private float sniperBulletSpeed = 15f;
    [SerializeField] private int maxAmmo = 20;
    [SerializeField] private float fireRate = 0.5f;

    Vector2 mousePos;

    private float nextFireTime = 0f;
    [SerializeField] private int currentAmmo;
    [SerializeField] private int currentMagazineCount; // Current bullets in the magazine
    [SerializeField] private int magazineCapacity = 5; // Magazine capacity

    [Header("Sound Effects")]
    [SerializeField] private AudioClip fireSound;
    [SerializeField] private AudioClip reloadSound;
    [SerializeField] private AudioClip dryFireSound;
    [SerializeField] private AudioClip sniperEquipSound;

    private AudioSource audioSource;

    private void Awake()
    {
        // Initialize audio source in Awake
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("AudioSource component missing on Sniper object.");
        }
    }
    private void Start()
    {
        currentMagazineCount = magazineCapacity; // Initialize magazine count
        currentAmmo = maxAmmo; // Initialize total ammo

        audioSource = GetComponent<AudioSource>();

        Debug.Log("Sniper Start - Current Magazine: " + currentMagazineCount + " / " + magazineCapacity);
        Debug.Log("Sniper Start - Current Ammo: " + currentAmmo + " / " + maxAmmo);

        // Trigger initial ammo update
        OnAmmoChanged.Invoke();
    }
    private void OnEnable()
    {
        // Play shotgun pump sound when the weapon is enabled 
        if (audioSource != null && sniperEquipSound != null)
        {
            audioSource.PlayOneShot(sniperEquipSound);
        }
    }

    private void Update()
    {
        if (GameManager.instance.IsGamePaused())
            return;

        RotateGunTowardsMouse();

        // Update mouse position
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Check for user input to fire
        if (Input.GetButtonDown("Fire1"))
        {
            Attack(mousePos);
            Debug.Log("Weapon Sniper Script call shoot");
        }

        // Check for user input to reload
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
            Debug.Log("Weapon Sniper Script call reload");
        }
    }

    private void RotateGunTowardsMouse()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = mousePos - (Vector2)sniperFirePoint.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        //Rotate the gun
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Flip the gun if the player is facing left
        if (lookDir.x < 0)
        {
            // Set the local scale to flip the gun 
            transform.localScale = new Vector3(1, -1, 1);
        }
        else
        {
            // Reset the local scale to its original state
            transform.localScale = Vector3.one;
        }
    }

    public override void Attack(Vector2 targetPosition)
    {
        if (Time.time < nextFireTime || currentMagazineCount <= 0)
            return;


        if(currentMagazineCount <= 0)
        {
            //Play dry fire sound
            if(audioSource != null && dryFireSound != null)
            {
                audioSource.PlayOneShot(dryFireSound);
            }
            return;
        }

        // Update next fire time
        nextFireTime = Time.time + 1f / fireRate;

        //Play fire sound
        if (audioSource != null && fireSound != null)
        {
            audioSource.PlayOneShot(fireSound);
        }

        // Calculate the direction from the fire point to the target position
        Vector2 direction = (targetPosition - (Vector2)sniperFirePoint.position).normalized;

        // Instantiate the bullet at the fire point
        GameObject bullet = Instantiate(sniperBulletPrefab, sniperFirePoint.position, sniperFirePoint.rotation);

        // Get the Rigidbody2D component of the bullet
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        // Apply velocity to the bullet towards the target position
        rb.velocity = direction * sniperBulletSpeed;

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2f);

        // Decrease ammo count
        currentMagazineCount--;

        Debug.Log("Sniper Attack - Current Ammo: " + currentMagazineCount + " / " + magazineCapacity);

        // Trigger ammo change event
        OnAmmoChanged.Invoke();

        Debug.DrawLine(sniperFirePoint.position, targetPosition, Color.red, 1f); // This will draw a red line for 1 second
    }

    public override void Reload()
    {
        if (currentMagazineCount < magazineCapacity) // Check if the magazine is not full
        {
            // Play reload sound
            if (audioSource != null && reloadSound != null)
            {
                audioSource.PlayOneShot(reloadSound);
            }

            // Calculate the available ammo that can be refilled into the magazine
            int availableAmmo = Mathf.Min(magazineCapacity - currentMagazineCount, currentAmmo);

            // Refill the magazine to its capacity or available ammo count
            currentMagazineCount += availableAmmo;

            // Reduce the total ammo count by the amount refilled into the magazine
            currentAmmo -= availableAmmo;

            Debug.Log("Reloading Sniper Rifle: " + currentMagazineCount + " / " + magazineCapacity);
            Debug.Log("Current Ammo: " + currentAmmo + " / " + maxAmmo);

            // Trigger ammo change event
            OnAmmoChanged.Invoke();
        }
    }

    public int GetCurrentAmmo()
    {
        return currentMagazineCount;
    }

    public int GetMaxAmmo()
    {
        return maxAmmo;
    }
    public int GetReserveAmmo()
    {
        return currentAmmo;
    }
}
