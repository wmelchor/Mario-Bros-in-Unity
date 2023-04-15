using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPower : MonoBehaviour /* THIS AND FIREBALL.CS ARE JUST SKELETONS, APPLY THIS YOURSELF, ME!!! THIS DOES NOT WORK AT ALL, THIS HAS ONLY HELP TO SPAWN THE FIREBALL*/
{
    public GameObject projectile;
	public Vector2 velocity;
	public PlayerMovement currentMovement;
	bool canShoot= true;
	public Vector2 offset = new Vector2(0.4f,0.1f);
	public float cooldown = 1f;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
				if(!currentMovement.grounded) {
					this.velocity.y = -8;
				} else {
					this.velocity.y = -5;
				}
				if (Input.GetKeyDown (KeyCode.T) && canShoot) {
						if(!currentMovement.directionFacing) { // If facing left
							GameObject go = Instantiate (projectile, ((Vector2)transform.position + offset * -(transform.localScale.x * 1.25f)), Quaternion.identity);
							go.GetComponent<Rigidbody2D>().velocity = new Vector2 (-velocity.x * transform.localScale.x, velocity.y);
						} else { // If facing right
							GameObject go = Instantiate (projectile, ((Vector2)transform.position + offset * (transform.localScale.x * 1.25f)), Quaternion.identity);
							go.GetComponent<Rigidbody2D>().velocity = new Vector2 (velocity.x * transform.localScale.x, velocity.y);
						}


						StartCoroutine (CanShoot());

						//GetComponent<Animator> ().SetTrigger ("shoot");
				}


	}

	
		IEnumerator CanShoot()
		{
				canShoot = false;
				yield return new WaitForSeconds (cooldown);
				canShoot = true;


		}
}
