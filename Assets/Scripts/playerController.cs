using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Health;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;

    private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    private Camera cam;

    private bool isDashing = false;
    private bool canDash = true;

    Vector2 movement;
    Vector2 mousePos;

    public Health health;

    [Header("Audio")]
    private AudioSource sfxAudioSrc;
    public AudioClip walkAudioClip;
    public AudioClip dashAudioClip;
    public AudioClip deathSoundClip;
    public AudioClip[] hurtSoundsClip;
    private int currentHurtSoundIndex = 0;
    private bool canPlayHurtSound = true;
    private float hurtSoundCooldown = 1f;

    [SerializeField] private Healthbar _healthbar;
    private float _currentHealth;

    //TODO: Make modular weapon system
    [SerializeField] private WeaponManager weaponManager;

    private PlayerHud playerHud;
    private PlayerData playerData;
    //private LevelingSystem levelingSystem;

    //Temporary, redo the deathmenu later
    //public GameObject deathMenuUI;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        cam = Camera.main;
        sfxAudioSrc = GetComponent<AudioSource>();

        _currentHealth = health.GetCurrentHealth();
        //Debug.Log("_currentHealth = " + _currentHealth);
        _healthbar.UpdateHealthBar(health.maxHealth, _currentHealth);

        //Subscribe to the health changed event
        health.HealthChanged += OnHealthChanged;

        playerHud = GameObject.FindObjectOfType<PlayerHud>();
        playerData = GameManager.instance.playerData;
        playerHud.UpdateLevel(playerData.currentLevel);
        playerHud.UpdateExperienceBar(playerData.currentExperience, playerData.experienceToNextLevel);
        //levelingSystem = GameObject.FindObjectOfType<LevelingSystem>();

        // Ensure weaponManager is assigned
        if (weaponManager == null)
        {
            Debug.LogError("WeaponManager is not assigned to PlayerController.");
            return;
        }

        //TEMP
        //deathMenuUI.SetActive(false);
        
    }
    private void OnDestroy()
    {
        // Unsubscribe from the health changed event to prevent memory leaks
        health.HealthChanged -= OnHealthChanged;
    }
    private void OnHealthChanged(int currentHealth, int maxHealth)
    {
        _currentHealth = currentHealth;
        _healthbar.UpdateHealthBar(maxHealth, currentHealth);

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        animator.SetTrigger("isDead");
        //TODO: Trigger death menu
        sfxAudioSrc.clip = deathSoundClip;
        sfxAudioSrc.Play();

        //Show death menu
        //deathMenuUI.SetActive(true);
        GameManager.instance.PlayerDied();

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.IsGamePaused())
            return;

        //Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if(Mathf.Abs(movement.x) > 0.0f || Mathf.Abs(movement.y) > 0.0f)
        {
            if(!sfxAudioSrc.isPlaying)
            {
                sfxAudioSrc.clip = walkAudioClip;
                sfxAudioSrc.Play(); 
            }
        }

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        //Dash
        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            StartCoroutine(Dash());
        }

        // Delegate weapon handling to WeaponManager
        weaponManager.HandleWeaponSwitching();
        weaponManager.HandleShooting();
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        // Movement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;

        // Play dash sound
        sfxAudioSrc.PlayOneShot(dashAudioClip);

        float startTime = Time.time;

        while (Time.time < startTime + dashDuration)
        {
            rb.velocity = movement.normalized * dashSpeed;
            yield return null;
        }

        // After dash ends, smoothly reset velocity
        rb.velocity = Vector2.zero;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    public void PlayHurtEffects()
    {
        if (canPlayHurtSound)
        {
            PlayHurtSound();
            StartCoroutine(HurtSoundCooldown());
        }
        StartCoroutine(TriggerHurtAnimation());
    }
    private void PlayHurtSound()
    {
        if(hurtSoundsClip.Length == 0)
        {
            return; // No sounds to play
        }
        //Play current hurt sound
        sfxAudioSrc.PlayOneShot(hurtSoundsClip[currentHurtSoundIndex]);
        //Update index to next hurt sound, wrapping around if necessary
        currentHurtSoundIndex = (currentHurtSoundIndex + 1) % hurtSoundsClip.Length;
    }
    private IEnumerator HurtSoundCooldown()
    {
        canPlayHurtSound = false;
        yield return new WaitForSeconds(hurtSoundCooldown);
        canPlayHurtSound = true;
    }
    private IEnumerator TriggerHurtAnimation()
    {
        animator.SetBool("isHurt", true);
        yield return new WaitForSeconds(0.25f);
        animator.SetBool("isHurt", false);
    }
    //If collide with pickup tag, call the collected function
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Pickup"))
        {
            PickUp pickup = collision.gameObject.GetComponent<PickUp>();
            if(pickup != null)
            {
                pickup.Collected();
            }
        }
    }
}
