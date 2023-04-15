using UnityEngine;

public class PlayerSpriteRenderer : MonoBehaviour
{
    public SpriteRenderer spriteRenderer { get; private set; }
    private PlayerMovement movement;

    public Sprite idle;
    public Sprite jump;
    public Sprite slide;
    public SpriteRenderer crouch;
    public AnimateSprite run;

    private void Awake() {
        movement = GetComponentInParent<PlayerMovement>(); // We say inParent because that script is in the parent object, not the individual ones.
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable() {
        spriteRenderer.enabled = true;
    }

    private void OnDisable() {
        spriteRenderer.enabled = false;
        run.enabled = false;
    }

    private void LateUpdate() {

        /* ORDER MATTERS 
           For example we can run and jump, but we want to show jumping sprite */
        crouch.enabled = false;
        spriteRenderer.enabled = true;
        run.enabled = movement.running; // If we are running then run is enabled
        if(movement.jumping) {
            crouch.enabled = false;
            spriteRenderer.enabled = true;
            spriteRenderer.sprite = jump;
        } else if (movement.crouching) { // Sprite is too high, figure out how to offset it in some way...
            spriteRenderer.enabled = false;
            crouch.enabled = true;
        } else if (movement.sliding) {
            crouch.enabled = false;
            spriteRenderer.enabled = true;
            spriteRenderer.sprite = slide;
        } else if (!movement.running) {
            crouch.enabled = false;
            spriteRenderer.enabled = true;
            spriteRenderer.sprite = idle;
        }
    }
}
