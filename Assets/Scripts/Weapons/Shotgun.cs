using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Shotgun : RangedWeapon
{
    [SerializeField] private GameObject shotgunBulletPrefab;
    [SerializeField] private Transform shotgunFirePoint;
    
    [SerializeField] private float shotgunBulletSpeed = 10f;
    [SerializeField] private int pelletsPerShot = 5;
    [SerializeField] private float bulletSpreadAngle = 30f;
    //[SerializeField] private int maxAmmo = 25;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private int magazineCapacity = 5;

    private float nextFireTime = 0f;
    private int currentAmmo;
    private bool isReloading = false;
    public float reloadTime = 2f;
    

    [Header("Sound Effects")]
    [SerializeField] private AudioClip fireSound;
    [SerializeField] private AudioClip reloadSound;
    [SerializeField] private AudioClip dryFireSound;
    [SerializeField] private AudioClip shotgunPumpSound;

    private AudioSource audioSource;
    private Coroutine reloadCoroutine;

    private void Awake()
    {
        // Initialize audio source in Awake
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("AudioSource component missing on Shotgun object.");
        }
    }
    private void Start()
    {
        currentAmmo = magazineCapacity;

        audioSource = GetComponent<AudioSource>();

        Debug.Log("Shotgun Start - Current Ammo: " + currentAmmo + " / " + magazineCapacity);

        OnAmmoChanged.Invoke(); 
    }
    private void OnEnable()
    {
        // Play shotgun pump sound when the weapon is enabled 
        if (audioSource != null && shotgunPumpSound != null)
        {
            audioSource.PlayOneShot(shotgunPumpSound);
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

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RotateTowardsMouse(mousePosition);

        if (Input.GetButtonDown("Fire1"))
        {
            Attack(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        //Automatic reload when current ammo is 0
        if(currentAmmo <= 0 && !isReloading)
        {
            //StartCoroutine(Reload());
            Reload();
            return;
        }

    }

    private void RotateTowardsMouse(Vector3 mousePosition)
    {
        Vector2 direction = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Flip the gun if the player is facing left
        if (direction.x < 0)
        {
            // Set the local scale to flip the gun 
            transform.localScale = new Vector3(0.4f, -0.4f, 0.4f);
        }
        else
        {
            // Reset the local scale to its original state
            transform.localScale = new Vector3(0.4f,0.4f,0.4f);
        }
    }
    public override void Attack(Vector2 targetPosition)
    {
        if (Time.time < nextFireTime || currentAmmo <= 0)
            return;

        if (currentAmmo <= 0)
        {
            //Play dry fire sound
            if (audioSource != null && dryFireSound != null)
            {
                audioSource.PlayOneShot(dryFireSound);
            }
            return;
        }

        nextFireTime = Time.time + 1f / fireRate;

        //Play fire sound
        if (audioSource != null && fireSound != null)
        {
            audioSource.PlayOneShot(fireSound);
        }

        for (int i = 0; i < pelletsPerShot; i++)
        {
            Vector2 direction = GetRandomDirection();
            GameObject bullet = Instantiate(shotgunBulletPrefab, shotgunFirePoint.position, shotgunFirePoint.rotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            //bulletScript.Initialize(shotgunBulletSpeed, weaponData.damage);
            bulletScript.Initialize(weaponData.bulletSpeed, weaponData.damage);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = direction * weaponData.bulletSpeed;
            Destroy(bullet, 2f);
        }

        currentAmmo--;
        OnAmmoChanged.Invoke();
    }

    private Vector2 GetRandomDirection()
    {
        float angle = Random.Range(-bulletSpreadAngle / 2, bulletSpreadAngle / 2);
        return Quaternion.Euler(0, 0, angle) * shotgunFirePoint.right;
    }

    public override void Reload()
    {
        if (currentAmmo < magazineCapacity && !isReloading)
        {
            if (audioSource != null && dryFireSound != null)
            {
                audioSource.PlayOneShot(dryFireSound);
            }
            StartCoroutine(ReloadSequence());
        }
    }
    private IEnumerator ReloadSequence()
    {
        isReloading = true;

        // Play reload sound
        if (audioSource != null && reloadSound != null)
        {
            audioSource.PlayOneShot(reloadSound);
        }

        // Wait for the reload sound to finish before playing the pump sound
        //yield return new WaitForSeconds(reloadSound.length);
        yield return new WaitForSeconds(reloadTime);
        //yield return new WaitForSeconds(WeaponData.)

        currentAmmo = magazineCapacity;

        // Play shotgun pump sound
        if (audioSource != null && shotgunPumpSound != null)
        {
            audioSource.PlayOneShot(shotgunPumpSound);
        }

        // Update ammo counts
        isReloading = false;
        OnAmmoChanged.Invoke(); 
    }

    public override int GetCurrentAmmo()
    {
        return currentAmmo;
    }
}
