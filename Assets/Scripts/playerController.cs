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
    public int damageTakenOnCollision = 10;

    private AudioSource sfxAudioSrc;
    public AudioClip walkAudioClip;

    [SerializeField] private Healthbar _healthbar;
    private float _currentHealth;

    private Sniper currentWeapon; // Assuming Sniper is the weapon script

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        cam = Camera.main;
        sfxAudioSrc = GetComponent<AudioSource>();

        _currentHealth = health.GetCurrentHealth();
        Debug.Log("_currentHealth = " + _currentHealth);
        _healthbar.UpdateHealthBar(health.maxHealth, _currentHealth);

        //Subscribe to the health changed event
        health.HealthChanged += OnHealthChanged;

        currentWeapon = GetComponent<Sniper>(); // Assuming the Sniper script is on the same GameObject
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
    }

    // Update is called once per frame
    void Update()
    {
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

        //Shoot
        //if (Input.GetButtonDown("Fire1") && currentWeapon != null)
        //{
        //}
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        // Movement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        // Rotate the gun towards the mouse
        //RotateGunTowardsMouse();
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;

        float startTime = Time.time;

        while (Time.time < startTime + dashDuration)
        {
            rb.velocity = movement.normalized * dashSpeed;
            yield return null;
        }

        // After dash ends, smoothly reset velocity
        rb.velocity = Vector2.zero;

        Debug.Log("I'm dashing");

        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            health.TakeDamage(damageTakenOnCollision);
            //_currentHealth = health.GetCurrentHealth(); // Update _currentHealth after taking damage
            //_healthbar.UpdateHealthBar(health.maxHealth, _currentHealth);
            Debug.Log("Player collided with an enemy!");
        }
    }
}
