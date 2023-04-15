using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : MonoBehaviour
{
    public Sprite flatSprite;

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Player")) { // If a Goomba collides with a player [Mario]
            Player player = collision.gameObject.GetComponent<Player>();
            if(player.starpower) {
                Hit();
            } else if(collision.transform.DotProductTest(transform, Vector2.down)) { // If this is true, then we know the player is falling down and landed on the goomba's head
                Flatten();
            } else {
                player.Hit();
                StartCoroutine("Reset"); // Invincibility frames (kinda)
            }
        }

        if(collision.gameObject.CompareTag("Enemy")) { // If the Goomba collides with another enemy, change direction
            EntityMovement movement = collision.gameObject.GetComponent<EntityMovement>();
            movement.direction = -movement.direction;
        }
    }

    IEnumerator Reset() // Basically give invincibility frames
    {
        Physics2D.IgnoreLayerCollision(3, 7, true);
        yield return new WaitForSecondsRealtime(1);
        Physics2D.IgnoreLayerCollision(3, 7, false);
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Shell")) {
            Hit();
        }
    }

    private void Flatten() {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<EntityMovement>().enabled = false;
        GetComponent<AnimateSprite>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = flatSprite;
        Destroy(gameObject, 0.5f);
    }

    private void Hit() {
        GetComponent<AnimateSprite>().enabled = false;
        GetComponent<DeathAnimation>().enabled = true;
        Destroy(gameObject, 3f);
    }
}
