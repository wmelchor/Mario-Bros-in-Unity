using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBarrier : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")) { // If the player falls into the death barrier...
            other.gameObject.SetActive(false); // Completely disable the player
            GameManager.Instance.ResetLevel(3f);
        } else {
            Destroy(other.gameObject); // If anything else touches it, destroy it
        }
    }
}
