using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

	public worldItem item; // Type of item enemy will drop
    bool dying = false;

	// Use this for initialization
	void Start () {
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

			int rand = Random.Range (0, 7); // Generate random item

			// Replace 'one' with whatever comes out of drop table
			AdventureItem it = GameObject.Find ("Inventory").GetComponent<ItemDatabase> ().getItem (rand);	
			item.GetComponent<SpriteRenderer> ().sprite = it.Sprite;
			item.id = rand; // Make sure this matches the random number too...
		}
    }
}
