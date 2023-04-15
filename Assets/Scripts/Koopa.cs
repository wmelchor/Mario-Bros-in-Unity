using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koopa : MonoBehaviour
{
    public Sprite shellSprite;
    public float shellSpeed = 12f;
    private bool inShell;
    private bool shellMoving;

    private void OnCollisionEnter2D(Collision2D collision) {
        if(!inShell && collision.gameObject.CompareTag("Player")) { // If a Koopa collides with a player [Mario]
            Player player = collision.gameObject.GetComponent<Player>();
            if(player.starpower) {
                Hit();
            } else if(collision.transform.DotProductTest(transform, Vector2.down)) { // If this is true, then we know the player is falling down and landed on the koopa's head
                EnterShell();
            } else {
                player.Hit();
                StartCoroutine("Reset"); // Invincibility frames (kinda)
            }
        }

        if(!inShell && collision.gameObject.CompareTag("Enemy")) { // If the Koopa collides with another enemy, change direction
            EntityMovement movement = collision.gameObject.GetComponent<EntityMovement>();
            movement.direction = -movement.direction;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(inShell && other.CompareTag("Player")) {
            if(!shellMoving) {
                Vector2 direction = new Vector2(transform.position.x - other.transform.position.x, 0f);
                PushShell(direction);
            } else {
                Player player = other.GetComponent<Player>();
                if(player.starpower) {
                    Hit();
                } else {
                    player.Hit();
                    StartCoroutine("Reset"); // Invincibility frames (kinda)
                }
            }
        }
        else if (!inShell && other.gameObject.layer == LayerMask.NameToLayer("Shell")) {
            Hit();
        }
    }

    IEnumerator Reset() // Basically give invincibility frames
    {
        Physics2D.IgnoreLayerCollision(3, 7, true);
        yield return new WaitForSecondsRealtime(1);
        Physics2D.IgnoreLayerCollision(3, 7, false);
        
    }

    private void EnterShell() {
        inShell = true;
        GetComponent<SpriteRenderer>().sprite = shellSprite;
        GetComponent<AnimateSprite>().enabled = false;
        GetComponent<EntityMovement>().enabled = false;
    }

    private void PushShell(Vector2 direction) {
        shellMoving = true;
        GetComponent<Rigidbody2D>().isKinematic = false;

        EntityMovement movement = GetComponent<EntityMovement>();
        movement.direction = direction.normalized;
        movement.speed = shellSpeed;
        movement.enabled = true;

        gameObject.layer = LayerMask.NameToLayer("Shell");
    }

    private void Hit() {
        GetComponent<AnimateSprite>().enabled = false;
        GetComponent<DeathAnimation>().enabled = true;
        Destroy(gameObject, 3f);
    }

    /* private void OnBecameInvisible() { // If the shell is pushed and it leaves the screen then it gets destroyed
        if (shellMoving) {
            Destroy(gameObject);
        }
    } */
}
