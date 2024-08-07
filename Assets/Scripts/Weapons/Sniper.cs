using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Sniper : RangedWeapon
{
    [SerializeField] private GameObject sniperBulletPrefab;
    [SerializeField] private Transform sniperFirePoint;
    [SerializeField] private float sniperBulletSpeed = 15f;
    [SerializeField] private float fireRate = 0.5f;
    private float nextFireTime = 0f;
    [SerializeField] private int currentAmmo;
    private bool isReloading = false;
    public float reloadTime = 2f;
    [SerializeField] private int magazineCapacity = 5; // Magazine capacity

    Vector2 mousePos;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip fireSound;
    [SerializeField] private AudioClip reloadSound;
    [SerializeField] private AudioClip dryFireSound;
    [SerializeField] private AudioClip sniperEquipSound;

    private AudioSource audioSource;
    private Coroutine reloadCoroutine;

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
        currentAmmo = magazineCapacity;
        audioSource = GetComponent<AudioSource>();
        Debug.Log("Sniper Start - Current Ammo: " + currentAmmo + " / " + magazineCapacity);
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
    private void OnDisable()
    {
        // Stop reloading if the weapon is disabled
        if (reloadCoroutine != null)
        {
            StopCoroutine(reloadCoroutine);
            reloadCoroutine = null;
        }
        isReloading = false;
    }

    private void Update()
    {
        if (GameManager.instance.IsGamePaused())
            return;

        RotateGunTowardsMouse();

        // Update mouse position
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Automatic reload when current ammo is 0
        if(currentAmmo <= 0 && !isReloading)
        {
            reloadCoroutine = StartCoroutine(Reload());
            return;
        }

        // Check for user input to fire
        if (Input.GetButtonDown("Fire1"))
        {
            Attack(mousePos);
            Debug.Log("Weapon Sniper Script call shoot");
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
        if (Time.time < nextFireTime || currentAmmo <= 0)
            return;


        if(currentAmmo <= 0)
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
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        //bulletScript.Initialize(sniperBulletSpeed, weaponData.damage);
        bulletScript.Initialize(weaponData.bulletSpeed, weaponData.damage);


        // Get the Rigidbody2D component of the bullet
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        // Apply velocity to the bullet towards the target position
        rb.velocity = direction * weaponData.bulletSpeed;

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2f);

        // Decrease ammo count
        currentAmmo--;

        Debug.Log("Sniper Attack - Current Ammo: " + currentAmmo + " / " + magazineCapacity);

        // Trigger ammo change event
        OnAmmoChanged.Invoke();

        Debug.DrawLine(sniperFirePoint.position, targetPosition, Color.red, 1f); // This will draw a red line for 1 second
    }
    private IEnumerator Reload()
    {
        isReloading = true;
        if (audioSource != null && reloadSound != null)
        {
            audioSource.PlayOneShot(reloadSound);
        }

        yield return new WaitForSeconds(reloadTime);
            
        currentAmmo = magazineCapacity;
        isReloading = false;
        reloadCoroutine = null;
        OnAmmoChanged.Invoke();
    }
    public override int GetCurrentAmmo()
    {
        return currentAmmo;
    }
}
