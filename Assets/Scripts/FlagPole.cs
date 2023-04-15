using System.Collections;
using UnityEngine;

public class FlagPole : MonoBehaviour
{
    public Transform flag;
    public Transform poleBottom;
    public Transform castleEntrance;
    public float speed = 6f; // We are going to animate this based on speed, not time. Because Mario's position on the flagpole can vary. 
    public int nextWorld = 1;
    public int nextStage = 1;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            StartCoroutine(MoveTo(flag, poleBottom.position)); // Move the flag to the bottom of the pole
            StartCoroutine(LevelCompleteSequence(other.transform)); // Pass in the transform so we can animate
        }
    }

    private IEnumerator LevelCompleteSequence(Transform player) {
        player.GetComponent<PlayerMovement>().enabled = false; // Disable movement

        yield return MoveTo(player, poleBottom.position); // Mario will move to the bottom of the flagpole
        yield return MoveTo(player, player.position + Vector3.right); // Mario will move one unit to the right
        yield return MoveTo(player, player.position + Vector3.right + Vector3.down); // Mario will move one right and one down [Next to block]
        yield return MoveTo(player, castleEntrance.position); // Mario will move to the castle entrance

        player.gameObject.SetActive(false);

        yield return new WaitForSeconds(2f);

        GameManager.Instance.LoadLevel(nextWorld, nextStage);
    }

    private IEnumerator MoveTo(Transform subject, Vector3 position) {
        while (Vector3.Distance(subject.position, position) > 0.125f) { // While the subject is far from the destination, move toward the destination
            subject.position = Vector3.MoveTowards(subject.position, position, speed * Time.deltaTime);
            yield return null;
        }

        subject.position = position; // Place player directly at the destination
    }

}
