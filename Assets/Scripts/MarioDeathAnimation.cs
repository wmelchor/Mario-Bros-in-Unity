using UnityEngine;
using System.Collections;

public class MarioDeathAnimation : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite deathSprite;

    private void Reset() { // Kind of like Awake, give it a default value
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable() {
        UpdateSprite();
        DisablePhysics();
        StartCoroutine(Animate());
    }

    private void UpdateSprite() {
        spriteRenderer.enabled = true;
        spriteRenderer.sortingOrder = 10;

        if(deathSprite != null) {
            spriteRenderer.sprite = deathSprite;
        }
    }

    private void DisablePhysics() {
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach(Collider2D collider in colliders) {
            collider.enabled = false;
        }
        GetComponent<Rigidbody2D>().isKinematic = true;

        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        EntityMovement entityMovement =  GetComponent<EntityMovement>();

        if(playerMovement != null) {
            playerMovement.enabled = false;
        }

        if(entityMovement != null) {
            entityMovement.enabled = false;
        }

    }

    private IEnumerator Animate() {
        float elapsed = 0f;
        float duration = 3f;

        float jumpVelocity = 10f;
        float gravity = -36f;

        Vector3 velocity = Vector3.up * jumpVelocity;

        while(elapsed < duration) {
            transform.position += velocity * Time.deltaTime;
            velocity.y += gravity * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}