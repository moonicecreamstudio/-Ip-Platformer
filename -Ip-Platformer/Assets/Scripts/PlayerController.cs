using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    FacingDirection lastFacingDirection;

    public enum FacingDirection
    {
        left, right
    }

    // Movement

    public float timeToReachMaxSpeed;
    public float initialTimeToReachMaxSpeed;
    public float maxSpeed;
    public float timeToDecelerate;
    public float acceleration;
    public float deceleration;
    private Rigidbody2D playerRB;
    Vector2 playerInput;

    // Jump Parameters

    public float gravity;
    public float jumpForce;
    public bool isJumping = false;
    public float jumpTimer = 0;
    public float maxJumpTimer;
    public float coyoteTimer = 0.5f;
    public float timerCounter = 0;

    // Running

    public bool isRunning;

    // Double Jump

    public bool doubleJumpUsed;

    // Start is called before the first frame update
    void Start()
    {
        acceleration = maxSpeed / timeToReachMaxSpeed;
        deceleration = maxSpeed / timeToDecelerate;
        playerRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //The input from the player needs to be determined and then passed in the to the MovementUpdate which should
        //manage the actual movement of the character.
        playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        MovementUpdate(playerInput);
        Debug.Log("playerInput = " + playerInput);
    }

    private void MovementUpdate(Vector2 playerInput)
    {
        // Easier to manage compared to addForce

        Vector2 currentVelocity = playerRB.velocity;

        // Move the character to the left
        if (playerInput.x < 0)
        {
            // Do stuff to currentVelocity
            currentVelocity += acceleration * Vector2.left * Time.deltaTime;
        }

        // Move the character to the right
        if (playerInput.x > 0)
        {
            // Do stuff to currentVelocity
            currentVelocity += acceleration * Vector2.right * Time.deltaTime;
        }

        // Move the character up
        if (playerInput.y > 0)
        {
            isJumping = true;
        }

        if (isJumping == true)
        {
            if (jumpTimer <= maxJumpTimer)
            {
                currentVelocity += jumpForce * Vector2.up * Time.deltaTime;
                jumpTimer += Time.deltaTime;
            }
        }

        // Jump Controls

        if (IsGrounded() == false && doubleJumpUsed == false && jumpTimer >= 2)
        {
            if (playerInput.y > 0)
            {
                currentVelocity.y = 0;
                currentVelocity += jumpForce * 2 * Vector2.up * Time.deltaTime;
                doubleJumpUsed = true;
            }
        }

        // When the player is off the ground, gravity is applied to the character until it touches the ground
        if (IsGrounded() == false)
        {
            currentVelocity.y -= gravity * Time.deltaTime;
        }
        if (IsGrounded() == true)
        {
            jumpTimer = 0;
            doubleJumpUsed = false;
        }

        if (playerInput.y == 0)
        {
            isJumping = false;
        }

        // Running controls

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRunning = true;
            Debug.Log("Running.");
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
            Debug.Log("Walking.");
        }

        if (isRunning == true)
        {
            timeToReachMaxSpeed = 0;
        }
        else
        {
            timeToReachMaxSpeed = initialTimeToReachMaxSpeed;
        }

        // Dashing controls

        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentVelocity.x = 0;
            currentVelocity += Vector2.left * 5;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            currentVelocity.x = 0;
            currentVelocity += Vector2.right * 5;
        }

        // If the character is currently accelerating:
        // Exponentially lower the speed
        if (playerInput.x == 0)
        {
            // Deaccelerate
            if (currentVelocity.x > 0)
            {
                currentVelocity.x -= Mathf.Pow(deceleration, 2) * Time.deltaTime;
            }
            if (currentVelocity.x < 0)
            {
                currentVelocity.x += Mathf.Pow(deceleration, 2) * Time.deltaTime;
            }
        }

        //RB handles the delta time stuff
        playerRB.velocity = currentVelocity;
    }

    public bool IsWalking()
    {
        if (playerInput.x != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsGrounded()
    {
        bool playerGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1.0f, LayerMask.GetMask("Ground"));

        if (playerGrounded)
        {
            timerCounter = 0;
            return true;
        }
        else
        {
            if (timerCounter == coyoteTimer)
            {
                timerCounter += Time.deltaTime;
            }
            return false;
        }
    }

    public FacingDirection GetFacingDirection()
    {
        if (playerInput.x > 0)
        {
            lastFacingDirection = FacingDirection.right;
        }

        if (playerInput.x < 0)
        {
            lastFacingDirection = FacingDirection.left;
        }

        return lastFacingDirection;
    }
}
