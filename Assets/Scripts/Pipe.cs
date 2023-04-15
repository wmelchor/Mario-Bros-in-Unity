using System.Collections;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public Transform connection;
    public KeyCode enterKeyCode = KeyCode.DownArrow;
    public Vector3 enterDirection = Vector3.down;
    public Vector3 exitDirection = Vector3.zero;

    private void OnTriggerStay2D(Collider2D other) {
        if (connection != null && other.CompareTag("Player")) { // Check if this is Mario and if the pipe connects to somewhere
            if (Input.GetKey(enterKeyCode)) { // If we press down
                StartCoroutine(Enter(other.transform)); // Start the enter animation
            }
        }
    }

    private IEnumerator Enter(Transform player)
    {
        player.GetComponent<PlayerMovement>().enabled = false; // Disable Mario's movement

        Vector3 enteredPosition = transform.position + enterDirection; // The pipe's position + the direction we are entering into
        Vector3 enteredScale = Vector3.one * 0.5f; // We're going to scale down Mario to look better

        yield return Move(player, enteredPosition, enteredScale);
        yield return new WaitForSeconds(1f);

        var sideSrolling = Camera.main.GetComponent<SideScrolling>();
        sideSrolling.SetUnderground(connection.position.y < sideSrolling.undergroundThreshold);

        if (exitDirection != Vector3.zero) { // We are exiting another pipe instead of appearing at a fixed point
            player.position = connection.position - exitDirection;
            yield return Move(player, connection.position + exitDirection, Vector3.one); 
        } else {
            player.position = connection.position;
            player.localScale = Vector3.one;
        }

        player.GetComponent<PlayerMovement>().enabled = true;
    }

    private IEnumerator Move(Transform player, Vector3 endPosition, Vector3 endScale) { // Similar to block coin code...
        float elapsed = 0f;
        float duration = 1f;

        Vector3 startPosition = player.position;
        Vector3 startScale = player.localScale; // Mario's shortened scale

        while (elapsed < duration) {
            float t = elapsed / duration;

            player.position = Vector3.Lerp(startPosition, endPosition, t);
            player.localScale = Vector3.Lerp(startScale, endScale, t);
            elapsed += Time.deltaTime;

            yield return null;
        }

        player.position = endPosition;
        player.localScale = endScale;
    }

}