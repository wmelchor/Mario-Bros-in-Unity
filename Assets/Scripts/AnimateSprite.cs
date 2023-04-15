using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateSprite : MonoBehaviour
{
    public Sprite[] sprites; // For Mario, we use Unity to input his run sprites in this order run sprite 1, 2, 3, 2. This makes it loop as we want.
    public float framerate = 1f / 6f; // Cycle between sprites x frames per second 1f/6f = 6 frames of sprites
    public float repeatRate = 1f / 6f;

    private SpriteRenderer spriteRenderer;
    private int frame;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable() {
        InvokeRepeating(nameof(Animate), framerate, framerate);
    }

    private void OnDisable() {
        CancelInvoke();
    }

    private void Animate() {
        frame++;
        if(frame >= sprites.Length) {
            frame = 0;
        }
        //frame = (frame + 1) % sprites.Length; // See if this can replace the code above...

        if(frame >= 0 && frame < sprites.Length) { // Extra safety precaution...
            spriteRenderer.sprite = sprites[frame]; // Cycle through each sprite
        }
    }
}
