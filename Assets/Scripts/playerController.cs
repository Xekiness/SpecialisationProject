using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class playerController : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 1f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sprite;

    private Vector2 lookDir;
    private Vector2 moveDir;
    private int lastDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }
    private void FixedUpdate()
    {
        /*Moving*/
        moveDir.x = Input.GetAxis("Horizontal");
        moveDir.y = Input.GetAxis("Vertical");

        // Update Animator parameter
        UpdateAnimatorParameters(moveDir.x, moveDir.y);

        // Calculate movement direction
        Vector2 moveDirection = new Vector2(moveDir.x, moveDir.y).normalized;

        // Apply movement
        rb.velocity = movementSpeed * Time.deltaTime * moveDirection;

    }
        
    void UpdateAnimatorParameters(float horizontalInput, float verticalInput)
    {
        // Set Animator parameters
        animator.SetFloat("Horizontal", horizontalInput);
        animator.SetFloat("Vertical", verticalInput);
        float magnitude = Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput);
        
        if(magnitude != 0)
        {
            if(verticalInput >= 0.1f)
            {
                lastDirection = 1;
                animator.Play("Girl_Walk_N");
            }
            else if(verticalInput <= 0.1f)
            {
                lastDirection = 0;
                animator.Play("Girl_Walk_S");
            }
        }
        animator.SetFloat("Magnitude", Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        Debug.Log("Horizontal: " + horizontalInput + "Vertical: " + verticalInput + "Magnitude: " + magnitude);
    }

    private void UpdateSpriteDirection()
    {
        if (moveDir.x > 0.0f)
        {
            sprite.flipX = false;
        }
        else if(moveDir.x < 0.0f)
        {
            sprite.flipX= true;
        }
    }
}



