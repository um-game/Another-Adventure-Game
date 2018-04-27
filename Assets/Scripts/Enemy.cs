using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    public int attack;
    public int health;
    public int defense;

	public worldItem item; // Type of item enemy will drop
    bool dying = false;
	RandomNumberGenerator rng;

	int difficulty;
    int triggerDelay;

	Dictionary<int, float> dt;

	// Use this for initialization
	void Start () {

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
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D collision) {
        Player myPlayer = collision.gameObject.GetComponent<Player>();
        triggerDelay = 20; // after x frames trigger this again if still colliding

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
