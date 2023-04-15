using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerSpriteRenderer smallRenderer;
    public PlayerSpriteRenderer bigRenderer;
    public PlayerSpriteRenderer bigFireRenderer;
    public PlayerSpriteRenderer smallFireRenderer;
    public PlayerSpriteRenderer activeRenderer { get; private set; } // The current state the player is in
    public ShootPower canShootFireball;
    public CapsuleCollider2D capsuleCollider { get; private set; }
    public MarioDeathAnimation deathAnimation { get; private set; }

    public bool big => bigRenderer.enabled || bigFireRenderer.enabled; // Mario is big if big renderer is enabled
    public bool small => smallRenderer.enabled;
    public bool hasFirePower => bigFireRenderer.enabled || smallFireRenderer.enabled;
    public bool dead => deathAnimation.enabled;
    public bool starpower { get; private set; }

    private void Awake() {
        deathAnimation = GetComponent<MarioDeathAnimation>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        activeRenderer = smallRenderer;
    }

    public void Hit() {
        if (!dead && !starpower) {
            if(hasFirePower) {
                LosePower();
                canShootFireball.enabled = false;
            } else if(big) {
                Shrink();
            } else {
                Death();
            }
        }
    }

    public void Grow() {
        smallRenderer.crouch.enabled = false;
        activeRenderer.enabled = true;
        if(activeRenderer == smallRenderer) {
            smallRenderer.enabled = false;
            bigRenderer.enabled = true;

            activeRenderer = bigRenderer;

            capsuleCollider.size = new Vector2(1f, 2f); // Adjust his hitbox to be bigger
            capsuleCollider.offset = new Vector2(0f, 0.5f); // Change offset as well, or he'll clip through the ground

            StartCoroutine(ScaleAnimation());
        } else {
            // Give points or something...

        }
    }

    public void GainFirePower() {
        activeRenderer.crouch.enabled = false;
        activeRenderer.enabled = true;
        canShootFireball.enabled = true;
        if(smallRenderer.enabled) {
            smallRenderer.enabled = false;
            smallFireRenderer.enabled = true;

            activeRenderer = smallFireRenderer;

            capsuleCollider.size = new Vector2(1f, 2f); // Adjust his hitbox to be bigger
            capsuleCollider.offset = new Vector2(0f, 0.5f); // Change offset as well, or he'll clip through the ground

            StartCoroutine(SmallFireTransformationAnimation());
        } else if(activeRenderer != bigFireRenderer){
            bigRenderer.enabled = false;
            smallFireRenderer.enabled = false;
            bigFireRenderer.enabled = true;

            // Do not adjust capsule collider...

            activeRenderer = bigFireRenderer;

            StartCoroutine(BigFireTransformationAnimation());
        } else {
            // Give points or something...
        }
    }

    private void Shrink() {
        smallRenderer.enabled = true;
        bigRenderer.enabled = false;

        activeRenderer = smallRenderer;

        capsuleCollider.size = new Vector2(1f, 1f); // Adjust his hitbox to be smaller
        capsuleCollider.offset = new Vector2(0f, 0f); // Change offset as well

        StartCoroutine(ScaleAnimation());
    }

    private void LosePower() { // If you lose a power up stronger than the mushroom
        activeRenderer.crouch.enabled = false;
        activeRenderer.enabled = true;
        if(bigFireRenderer.enabled) {
            bigRenderer.enabled = true;
            bigFireRenderer.enabled = false;

            activeRenderer = bigRenderer;

            StartCoroutine(BigFireTransformationAnimation());
        } else if(smallFireRenderer.enabled) {
            smallRenderer.enabled = true;
            smallFireRenderer.enabled = false;

            activeRenderer = smallRenderer;

            StartCoroutine(SmallFireTransformationAnimation());
        }
    }

    private IEnumerator ScaleAnimation() {
        float elapsed = 0f;
        float duration = 0.5f; // Half second animation

        while (elapsed < duration) {
            elapsed += Time.deltaTime;

            if (Time.frameCount % 4 == 0) { // We will alternate between the sprites for the animation
                smallRenderer.enabled = !smallRenderer.enabled; // We do it this way so it works for both getting big or small.
                bigRenderer.enabled = !smallRenderer.enabled;
            }

            yield return null;
        }

        smallRenderer.crouch.enabled = false;
        bigRenderer.crouch.enabled = false;
        smallFireRenderer.crouch.enabled = false;
        bigFireRenderer.crouch.enabled = false;
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        activeRenderer.enabled = true;
    }

    private IEnumerator BigFireTransformationAnimation() {
        float elapsed = 0f;
        float duration = 0.5f; // Half second animation

        while (elapsed < duration) {
            elapsed += Time.deltaTime;

            if (Time.frameCount % 4 == 0) { // We will alternate between the sprites for the animation
                bigRenderer.enabled = !bigRenderer.enabled;
                bigFireRenderer.enabled = !bigRenderer.enabled;
            }
            yield return null;
        }

        smallRenderer.crouch.enabled = false;
        bigRenderer.crouch.enabled = false;
        smallFireRenderer.crouch.enabled = false;
        bigFireRenderer.crouch.enabled = false;
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        smallFireRenderer.enabled = false;
        bigFireRenderer.enabled = false;
        activeRenderer.enabled = true;

    }

    private IEnumerator SmallFireTransformationAnimation() {
        float elapsed = 0f;
        float duration = 0.5f; // Half second animation

        while (elapsed < duration) {
            elapsed += Time.deltaTime;

            if (Time.frameCount % 4 == 0) { // We will alternate between the sprites for the animation
                smallRenderer.enabled = !smallRenderer.enabled;
                smallFireRenderer.enabled = !smallRenderer.enabled;
            }
            yield return null;
        }

        smallRenderer.crouch.enabled = false;
        bigRenderer.crouch.enabled = false;
        smallFireRenderer.crouch.enabled = false;
        bigFireRenderer.crouch.enabled = false;
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        bigFireRenderer.enabled = false;
        smallFireRenderer.enabled = false;
        activeRenderer.enabled = true;

    }

    public void Starpower() {
        StartCoroutine(StarpowerAnimation());
    }

    private IEnumerator StarpowerAnimation() {
        starpower = true;

        float elapsed = 0f;
        float duration = 10f;

        while (elapsed < duration) {
            elapsed += Time.deltaTime;

            if (Time.frameCount % 4 == 0) {
                activeRenderer.spriteRenderer.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
            }

            yield return null;
        }

        activeRenderer.spriteRenderer.color = Color.white;
        starpower = false;
    }

    private void Death() {
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        deathAnimation.enabled = true;

        GameManager.Instance.ResetLevel(3f);
    }
}
