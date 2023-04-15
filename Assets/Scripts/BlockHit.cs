using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHit : MonoBehaviour
{
    public GameObject possibleItem1;
    public GameObject possibleItem2;

    public Sprite emptyBlock;
    public int maxHits = -1; // Default value for blocks that can be hot infinitely, can be adjusted in the editor
    private bool animating;


    private void OnCollisionEnter2D(Collision2D collision) {
        if (!animating && maxHits != 0 && collision.gameObject.CompareTag("Player"))
        {
            if (collision.transform.DotProductTest(transform, Vector2.up)) { // If Mario collides with the block while moving up
                Hit();
            }
        }
    }

    private void Hit() {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true; // Reenable the sprite if it is a hidden block

        maxHits--;

        if (maxHits == 0) {
            spriteRenderer.sprite = emptyBlock;
        }
        GameObject player = GameObject.FindWithTag("Player"); // Find thing with tag "Player"
        if (possibleItem1 != null) { // There is an item in the block, so we spawn the item where the block is
            if(player.GetComponent<Player>().small) { // If Mario is small, spawn item in first slot...
                Instantiate(possibleItem1, transform.position, Quaternion.identity);
            } else {
                Instantiate(possibleItem2, transform.position, Quaternion.identity);
            }
        }

        StartCoroutine(Animate());
    }

    private IEnumerator Animate() {
        animating = true; // We sandwhich the animation in between true and false so that we can't hit the block while it is animating

        Vector3 restingPosition = transform.localPosition;
        Vector3 animatedPosition = restingPosition + Vector3.up * 0.5f;

        yield return Move(restingPosition, animatedPosition); // Will move the block up a bit
        yield return Move(animatedPosition, restingPosition); // Will move it back down

        animating = false;
    }

    private IEnumerator Move(Vector3 startPosition, Vector3 endPosition) { // Animate the movement... Maybe later make this done by Tweening
        float elapsed = 0f;
        float duration = 0.125f; // How long the animation is

        while (elapsed < duration)
        {
            float t = elapsed / duration; // The percentage of animation it has gone through

            transform.localPosition = Vector3.Lerp(startPosition, endPosition, t); // Linear interpolation from startPosition to endPosition, exact frame-by-frame position depending on the t value
            elapsed += Time.deltaTime; 

            yield return null;
        }

        transform.localPosition = endPosition; // Make sure our block is in the exact original position
    }
}
