using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private new Camera camera;
    //public Fireball fireballSprite;
    private new Rigidbody2D rigidbody;
    private new Collider2D collider;
    private Vector2 velocity;
    private float inputAxis;
    public float movementSpeed = 8f;
    public float maxJumpHeight = 5f; // How high can Mario jump
    public float maxJumpTime = 1f; // How long until he reaches max height
    public float jumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f); // The => is like creating a property with just a getter.
    public float gravity => (-2f * maxJumpHeight) / Mathf.Pow((maxJumpTime / 2f), 2);
    public bool grounded { get; private set; } // We want to public get this, but only private set
    public bool jumping { get; private set; }
    public bool directionFacing { get; private set; } // True is right, left is false
    public bool running => Mathf.Abs(velocity.x) > 0.25f || Mathf.Abs(inputAxis) > 0.25f; // If he is moving in either direction(thanks Abs) or player is holding down a direction, set run to true.
    //public bool fastRunning => Mathf.Abs(velocity.x) > movementSpeed/1.25f;
    public bool sliding => (inputAxis > 0f && velocity.x < 0f) || (inputAxis < 0f && velocity.x > 0f); // If we move in one direction but hold down the opposite direction.
    public bool falling => velocity.y < 0f || !Input.GetButton("Jump"); // We know you are falling if your velocity y is less than 0 or if you are not holding jump.
    public bool crouching => Input.GetKey(KeyCode.DownArrow);
    public bool canCrouch => grounded;

    private void Awake() {
        rigidbody = GetComponent<Rigidbody2D>();
        camera = Camera.main; // This will automatically assign camera to the camera that is in our scene
        collider = GetComponent<Collider2D>();
        directionFacing = true;
    }

    private void OnEnable() {
        rigidbody.isKinematic = false;
        collider.enabled = true;
        velocity = Vector2.zero;
        jumping = false;
    }

    private void OnDisable() {
        rigidbody.isKinematic = true;
        collider.enabled = false;
        velocity = Vector2.zero;
        jumping = false;
    }

    /* Runs every frame */
    private void Update() {
        HorizontalMovement();
        grounded = rigidbody.Raycast(Vector2.down); // If our raycast detects a collider right below us, then we are grounded
        if(grounded) {
            GroundedMovement();
        }
        Player player = gameObject.GetComponent<Player>();
        if((crouching && player.big) || player.small) {
           player.capsuleCollider.size = new Vector2(1f, 1f); // Adjust his hitbox to be in line with the crouch
           player.capsuleCollider.offset = new Vector2(0f, 0f); // Change offset as well, or he'll clip through the ground
        } else if (player.big && !crouching) {
            player.capsuleCollider.size = new Vector2(1f, 2f); // Adjust his hitbox to be bigger
            player.capsuleCollider.offset = new Vector2(0f, 0.5f); // Change offset as well, or he'll clip through the ground
        }
        ApplyGravity();
    }

    private void HorizontalMovement() {
        inputAxis = Input.GetAxis("Horizontal");
        //velocity.x = Mathf.MoveTowards(velocity.x, inputAxis * movementSpeed/1.75f, movementSpeed * Time.deltaTime);
        if(Input.GetKey("x")) {
            velocity.x = Mathf.MoveTowards(velocity.x, inputAxis * movementSpeed * 1.40f, movementSpeed * Time.deltaTime);
        } else {
            /* Our x-axis velocity will have our speed go from velocity.x to inputAxis*movementSpeed over the course of movementSpeed*deltaTime(FPS independent)*/
            velocity.x = Mathf.MoveTowards(velocity.x, inputAxis * movementSpeed/1.25f, movementSpeed * Time.deltaTime);
        }

        if((inputAxis > 0f && velocity.x < 0f) || ((inputAxis < 0f && velocity.x > 0f))) {
            velocity.x = Mathf.MoveTowards(velocity.x, 0, movementSpeed * Time.deltaTime);
        }

        if(grounded && crouching) {
            velocity.x = Mathf.MoveTowards(velocity.x, 0, (movementSpeed * 1.5f) * Time.deltaTime);
        }

        /* This if statement is for checking if there is an object immediately horizontal of us. [Vector.right * veloecity.x will acount for both left and right
        as velocity can be negative] This is the similar to the check to see if we are grounded. Logically it is also similar to the check if we bonk Mario's head */
        if(rigidbody.Raycast(Vector2.right * velocity.x)) { 
            velocity.x = 0f;
        }

        /* This will change which direction Mario is facing, when grounded. Mario should not change direction mid-air, as that was how it was in the original. */
        if(grounded) {
            if (velocity.x > 0f) {
                transform.eulerAngles = Vector3.zero; // Mario will face right
                directionFacing = true;
            } else if (velocity.x < 0f) {
                transform.eulerAngles = new Vector3(0f, 180f, 0f); // Rotate the y-axis of Mario by 180 degrees
                directionFacing = false;
            }
        }
    }

    private void GroundedMovement() {
        velocity.y = Mathf.Max(velocity.y, 0f); // Prevent velocity from building up from staying grounded
        jumping = velocity.y > 0f; // Normally our y velocity is negative until the moment we hit jump, so this helps to make it more accurate.
        if(Input.GetButtonDown("Jump")) {
            velocity.y = jumpForce;
            if(!crouching) {
                jumping = true;
            }
        }
    }

    private void ApplyGravity() {
        float multiplier = falling ? 2f : 1f; // If we are falling or if we let go of the jump button we want to make gravity stronger to make Mario fall faster. If not falling, 1f, so we get a higher jump
        velocity.y += gravity * multiplier * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, gravity / 2f); // Not really needed but this will prevent Mario from falling too fast. A terminal velocity.
    }

    /* Apply our velocity to our position, in a frame rate independent update function (Does not run every frame, but is time dependent) */
    private void FixedUpdate() {
        Vector2 position = rigidbody.position;
        position += velocity * Time.fixedDeltaTime;

        Vector2 leftEdge = camera.ScreenToWorldPoint(Vector2.zero); // Left edge is zero
        Vector2 rightEdge = camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)); // Right edge is the boundaries of the screen
        position.x = Mathf.Clamp(position.x, leftEdge.x + 0.5f, rightEdge.x - 0.5f); // This ensures that position.x stays within leftEdge.x and rightEdge.x. The +/- 0.5 is to center Mario

        rigidbody.MovePosition(position);
    }

    /* We will check if Mario's head collides with a non-powerup object 
       We do this by checking if something is above Mario with Dot Products,
       which compares how similar 2 vectors are. If the dot product is 0, then
       we have collided perpendicular with the object, meaning we hit it from the side.
       If the dot product is 1 then the vectors are exactly the same, so we hit it moving
       upwards. If the dot product is -1, then the vectors are opposite, meaning we landed on
       the object. */
    private void OnCollisionEnter2D(Collision2D collision) { // This is a unity built in function name, so it is important to name it exactly like this.
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
            Player player = gameObject.GetComponent<Player>();
            if(transform.DotProductTest(collision.transform, Vector2.down) && !player.GetComponent<Player>().starpower) {
                if(Input.GetButton("Jump")) {
                    velocity.y = jumpForce;
                } else {
                    velocity.y = jumpForce / 2f;
                }
                jumping = true;
            }
        } else if (collision.gameObject.layer != LayerMask.NameToLayer("PowerUp")) {
            if(transform.DotProductTest(collision.transform, Vector2.up)) { // If Mario (transform) collides with an object moving upwards
                velocity.y = 0f;
            }
        }
    }

}
