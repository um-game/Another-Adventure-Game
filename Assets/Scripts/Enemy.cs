using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

	public worldItem item; // Type of item enemy will drop
    bool dying = false;
	RandomNumberGenerator rng;

	int difficulty;

	Dictionary<int, float> dt;

	// Use this for initialization
	void Start () {

		// TODO: Setup enemy database?

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
		this.gameObject.SetActive (false);
        dying = true;
		Destroy (this.gameObject);
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
