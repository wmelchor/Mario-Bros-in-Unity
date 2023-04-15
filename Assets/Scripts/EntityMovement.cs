using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovement : MonoBehaviour
{
    private new Rigidbody2D rigidbody;

    public float speed = 1f;
    public Vector2 direction = Vector2.left;
    private Vector2 velocity;
    public float gravity = -20f;

    public float maxJumpHeight = 1f; // How high can Entity jump
    public float maxJumpTime = 1f; // How long until it reaches max height
    public float jumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f); // The => is like creating a property with just a getter.
    public bool jumping { get; private set; }
    public bool grounded { get; private set; } // We want to public get this, but only private set

    private void Awake() {
        rigidbody = GetComponent<Rigidbody2D>();
        enabled = false; // Object is sleep by default
    }

    private void OnBecameVisible() { // Built in Unity function that detects when an object becomes visible on screen
        enabled = true;
    }

    private void OnBecameInvisible() {
        enabled = false;
    }

    private void OnEnable() {
        rigidbody.WakeUp(); // Become active when on screen
    }

    private void OnDisable() {
        rigidbody.velocity = Vector2.zero;
        rigidbody.Sleep();
    }

    private void Jump() {
        //velocity.y = Mathf.Max(velocity.y, 0f); // Prevent velocity from building up from staying grounded
        jumping = velocity.y > 0f; // Normally our y velocity is negative until the moment we hit jump, so this helps to make it more accurate.
        if(grounded) {
            velocity.y = jumpForce;
            jumping = true;
        }
    }

    private void FixedUpdate() {
        velocity.x = direction.x * speed;
        velocity.y += gravity * Time.fixedDeltaTime; // Referring to the gravity that Unity defines. Could also use gravity defined in this script

        rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);

        if(rigidbody.Raycast(direction)) { // If we collide with an object in the direction we are going, change direction
            direction = -direction;
        }
        

        if(rigidbody.Raycast(Vector2.down)) {
            velocity.y = Mathf.Max(velocity.y, 0f); // Prevent speed from building up from gravity when grounded
        }

        grounded = rigidbody.Raycast(Vector2.down); // If our raycast detects a collider right below us, then we are grounded

        Jump();

        if (direction.x > 0f) {
            transform.localEulerAngles = new Vector3(0f, 180f, 0f);
        } else if (direction.x < 0f) {
            transform.localEulerAngles = Vector3.zero;
        }
    }
}
