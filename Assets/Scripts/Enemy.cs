using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public worldItem item; // Type of item enemy will drop
    bool dying = false;
	Rigidbody2D rb2d;
	int maxSpeed = 1;
	int range; // If player is within range, enemy will attack
	Transform p; // Hold transform of player, used to calculate movement direction

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
		p = GameObject.Find ("player").transform;

		range = 2;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		Vector3 diff = p.transform.position - this.transform.position; // Get difference in position

		if (diff.magnitude < range) {

			diff.Normalize ();

			// Move towards player
			rb2d.velocity = new Vector2 (diff.x * maxSpeed, diff.y * maxSpeed);

		} else {
			rb2d.velocity = new Vector2 (0, 0); // Otherwse we stop moving
		}
	}

	void OnCollisionEnter2D(Collision2D collision) {

		Player p = collision.gameObject.GetComponent<Player> ();

		if (p != null) {
			this.gameObject.SetActive (false);
			dying = true;
			Destroy (this.gameObject);
		}
	}

    private void OnDestroy()
    {
        if (dying)
            Instantiate(item, transform.position, item.transform.rotation);
        //item.transform.position = new Vector3 (this.gameObject.transform.position.x, this.gameObject.transform.position.y + 1, 0);
    }
}
