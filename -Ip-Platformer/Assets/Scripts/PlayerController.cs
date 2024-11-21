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

    public float timeToReachMaxSpeed;
    public float maxSpeed;
    public float timeToDecelerate;

    public float acceleration;
    public float deceleration;
    private Rigidbody2D playerRB;
    Vector2 playerInput;

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
        playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
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

        // Move the character to the jumps
        // Do stuff to currentVelocity

        // If the character is currently accelerating:
        if (playerInput.x == 0)
        {
            // Deaccelerate
            currentVelocity = Vector2.zero;
        }

        //RB handles the delta time stuff
        playerRB.velocity = currentVelocity;
    }

    public bool IsWalking()
    {
        return false;
    }
    public bool IsGrounded()
    {
        return true;
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
