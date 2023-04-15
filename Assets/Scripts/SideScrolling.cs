using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideScrolling : MonoBehaviour
{
    private Transform player;

    public float cameraHeight = 6.5f; // Default for 1-1
    public float undergroundHeight = -9.5f; // Default for 1-1
    public float undergroundThreshold = 0f;

    private void Awake() {
        player = GameObject.FindWithTag("Player").transform;
    }

    /* LateUpdate ensures that this happens after Mario's position is updated */
    private void LateUpdate() {
        Vector3 cameraPosition = transform.position; // Get current camera position (transform refers to that)
        //cameraPosition.x = player.position.x; // Set camera position to player position
        cameraPosition.x = Mathf.Max(cameraPosition.x, player.position.x); // This ensures that the camera will only be able to move right (Like the original)
        transform.position = cameraPosition; // Set transform to that position
    }

    public void SetUnderground(bool underground) {
        Vector3 cameraPosition = transform.position;
        cameraPosition.y = underground ? undergroundHeight : cameraHeight;
        transform.position = cameraPosition;
    }
}
