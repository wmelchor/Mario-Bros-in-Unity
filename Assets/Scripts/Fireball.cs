using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour {

		public Rigidbody2D rb;
		public GameObject explosion;
		public Vector2 velocity;
		public Sprite[] sprites;
		public float framerate = 1f / 6f; // Cycle between sprites x frames per second 1f/6f = 6 frames of sprites
    public float repeatRate = 1f / 6f;

    private SpriteRenderer spriteRenderer;
    private int frame;
    


	// Use this for initialization
	void Start () {
				Destroy (this.gameObject, 10);
				rb = GetComponent<Rigidbody2D> ();
				velocity = rb.velocity;
				spriteRenderer = GetComponent<SpriteRenderer>();
	
	}
	
	// Update is called once per frame
	void Update () {
	

				if (rb.velocity.y <= velocity.y) {
						rb.velocity = velocity;
				}

	}


		void OnCollisionEnter2D(Collision2D col)
		{

				rb.velocity = new Vector2 (velocity.x, -velocity.y);


				if (col.collider.tag == "Enemy") {
						col.gameObject.GetComponent<AnimateSprite>().enabled = false;
        				col.gameObject.GetComponent<DeathAnimation>().enabled = true;
        				Destroy(col.gameObject, 3f);
						Explode ();
				}

				if (col.collider.tag != "Player") { // So that it doesn't immediately explode
					if (Mathf.Abs(col.contacts[0].normal.x) > 0.5f) { // If it makes contact with something that isn't flat ground
							Explode ();
					} else {
						this.velocity.y = -5; // Doesn't do anything. Trying to make it so it bounces normally after landing
					}
				}

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

		void Explode()
		{

				var explodedFireball = Instantiate (explosion, transform.position, Quaternion.identity);
				Destroy(this.gameObject);
				Destroy(explodedFireball, 0.1f);

		}
}
