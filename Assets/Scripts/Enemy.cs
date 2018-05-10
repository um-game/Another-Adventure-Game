using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    public int attack;
    public int health;
    public int defense;

	Rigidbody2D RigidBodyEnemy; // Cat Fighter's Rigid Body
	int maxSpeed = 1;           // Speed of Enemy
	int range = 2;              // If player is within range, enemy will attack
	int attRange = 1;
    Tutorial tutorial;
    Transform target;           // Hold transform of player, used to calculate movement direction
	int facing = 0;
	Animator anim;
	Vector3 currentPosition, lastPosition;  // Used for sprite flipping

	public worldItem item; // Type of item enemy will drop
    bool dying = false;
	RandomNumberGenerator rng;

	int difficulty;
    int triggerDelay;

	Dictionary<int, float> dt;

	// Use this for initialization
	void Start () {

		RigidBodyEnemy = GetComponent<Rigidbody2D>();
		target = GameObject.Find("player").transform;
	    tutorial = GameObject.Find("Tutorial").GetComponent<Tutorial>();

        currentPosition = transform.position;
		lastPosition = currentPosition;

		anim = GetComponent<Animator>();

        // TODO: Setup enemy database?
        attack = 11;
        health = 20;

		// TODO: Populate from file?
		dt = new Dictionary<int, float> () { { 0, 0.2f}, { 1, 0.3f}, {2, 0.5f} }; 
		difficulty = 1;

		float[] probs = new float[dt.Count];

		// Create array of probabilities based on drop table
		for (int i =0; i < dt.Count; i++) {
			probs [i] = dt [i];
		}
		rng = new RandomNumberGenerator(probs); 
	}

	// Update is caled once per frame
	void Update () {

		float moveX = Input.GetAxis("Horizontal");
		float moveY = Input.GetAxis("Vertical");

		// FIXME: enemy gets stuck
		Vector3 diff = target.transform.position - this.transform.position; // Get difference in position
		anim.SetInteger("attRange", (int)diff.magnitude);

		if (diff.magnitude < range)
		{
			diff.Normalize();


			// Move towards player
			RigidBodyEnemy.velocity = new Vector2(diff.x * maxSpeed, diff.y * maxSpeed);
		    tutorial.showAttack();

		}
		else
		{
			RigidBodyEnemy.velocity = new Vector2(0, 0); // Otherwse we stop moving
		}

		if (currentPosition.x > lastPosition.x)
		{
			anim.SetInteger("facing", 1);
			GetComponent<SpriteRenderer> ().flipX = false;
		}
		if (currentPosition.x < lastPosition.x)
		{
			anim.SetInteger("facing", 3);
			GetComponent<SpriteRenderer> ().flipX = true;

		}

		var relativePoint = transform.InverseTransformPoint(transform.position);

		//Update the new positions
		lastPosition = currentPosition;
		currentPosition = transform.position;

	}

	void OnTriggerEnter2D(Collider2D collision) {

        if (collision.gameObject.GetComponent<Enemy>() != null)
            return;

        Player myPlayer = collision.gameObject.GetComponent<Player>();
        triggerDelay = 20; // after x frames trigger this again if still colliding

	    if (myPlayer != null)
	    {
            if (myPlayer.isAttacking)
            {
                int damage = defense - myPlayer.attack;
                if (damage > 0)
                    damage = 0;
                health += damage;
                Debug.Log("HIT ENEMY: " + health);
            }
            else
            {
                int damage = myPlayer.defense - attack;
                if (damage > 0)
                    damage = 0;
                myPlayer.health += damage;
                Debug.Log("HIT PLAYER: " + myPlayer.health);
                if (myPlayer.health == 0)
                    Debug.Log("PLAYER DIED!");
            }
        
            if (health <= 0)
            {
                this.gameObject.SetActive(false);
                dying = true;
                Destroy(this.gameObject);
                tutorial.learnAttack();
            }
	    }

	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (triggerDelay <= 0)
            OnTriggerEnter2D(collision);
        else
            triggerDelay -= 1;
    }

    private void OnDestroy()
    {
		if (dying) {
			item = Instantiate (item, this.transform.position, item.transform.rotation);
			item.transform.position = this.transform.position;

			ItemDatabase itemDB = GameObject.Find ("Inventory").GetComponent<ItemDatabase> ();

			// Get all items that correspond to this enemy's difficulty
			List<AdventureItem> possibleDrop = itemDB.getItemByRarity (difficulty);

			int rand = rng.next (); // Generate random item index

			// Get item at generated index
			AdventureItem it = possibleDrop[rand];	
			item.GetComponent<SpriteRenderer> ().sprite = it.Sprite;
			item.id = it.ID; // Make sure this matches the random number too...
		}
    }
}
