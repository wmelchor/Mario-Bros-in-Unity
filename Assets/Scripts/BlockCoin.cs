using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCoin : MonoBehaviour
{
    private void Start() {
        GameManager.Instance.AddCoin();

        StartCoroutine(Animate()); // Animate coin in a similar manner to the blocks
    }

    private IEnumerator Animate() {
        Vector3 restingPosition = transform.localPosition;
        Vector3 animatedPosition = restingPosition + Vector3.up * 2f; // Make the coin go higher than the block does

        yield return Move(restingPosition, animatedPosition);
        yield return Move(animatedPosition, restingPosition);

        Destroy(gameObject);
    }

    private IEnumerator Move(Vector3 startPosition, Vector3 endPosition) {
        float elapsed = 0f;
        float duration = 0.25f; // Longer animation duration than the block

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            transform.localPosition = Vector3.Lerp(startPosition, endPosition, t);
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = endPosition;
    }
}
