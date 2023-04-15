using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockItem : MonoBehaviour
{
   private void Start() {
        StartCoroutine(Animate()); // Need to animate it coming out
    }

    private IEnumerator Animate() {

        /* We need to disable all colliders while it's animating, so that it doesn't look weird while it's coming out */
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        CircleCollider2D physicsCollider = GetComponent<CircleCollider2D>();
        BoxCollider2D triggerCollider = GetComponent<BoxCollider2D>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        rigidbody.isKinematic = true; // Remove physics
        physicsCollider.enabled = false;
        triggerCollider.enabled = false;
        spriteRenderer.enabled = false; // Remove block item sprite so that the player can't see it while the block it animating

        yield return new WaitForSeconds(0.25f); // Let block animation play out

        spriteRenderer.enabled = true; // Reenable sprite and begin animation process

        float elapsed = 0f;
        float duration = 0.5f;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position + new Vector3(0f, 0.9f, 0f); // A little less than Vector3.up

        while (elapsed < duration) {
            float t = elapsed / duration;

            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            elapsed += Time.deltaTime;

            yield return null;
        }

        rigidbody.isKinematic = false; // Physics can affect it now
        physicsCollider.enabled = true; 
        triggerCollider.enabled = true;
    }
}
