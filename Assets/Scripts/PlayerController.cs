using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float maxSpeed;

    Animator anim;
    Rigidbody2D rb2d;
	Inventory inv;
	bool isInvOpen; // Used to block input to player while inventory is open
	bool menuOpen;
	PickupMenu pickupMenu;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
		pickupMenu = GetComponent<PickupMenu> ();
        anim.SetInteger("direction", 3);
        anim.SetBool("moving", false);
		inv = GameObject.Find ("Inventory").GetComponent<Inventory> ();
		isInvOpen = false; // Assume the inventory is closed upon loading
		menuOpen = false;
	}

	void Update() {
		// Toggle the inventory(if aother menu isnt already open
		if (!menuOpen && Input.GetKeyDown (KeyCode.I) ) {
			toggleInventory ();
		}
	}
	
	// Fixed Update is called once per frame
	void FixedUpdate () {

		// Block input while inentory is open
		if (menuOpen || isInvOpen) {
			return;
		}

        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        
        if (Input.GetKey(KeyCode.W)) {
            anim.SetInteger("direction", 1);
            anim.SetBool("moving", true);

           rb2d.velocity = new Vector2(0, moveY * maxSpeed);
        }
        else if (Input.GetKey(KeyCode.S)) {
            anim.SetInteger("direction", 3);
            anim.SetBool("moving", true);

           rb2d.velocity = new Vector2(0, moveY * maxSpeed);
        }
        else if (Input.GetKey(KeyCode.D)) {
            anim.SetInteger("direction", 2);
            anim.SetBool("moving", true);

           rb2d.velocity = new Vector2(moveX * maxSpeed, 0);
        }
        else if (Input.GetKey(KeyCode.A)) {
            anim.SetInteger("direction", 4);
            anim.SetBool("moving", true);

            rb2d.velocity = new Vector2(moveX * maxSpeed, 0);
        }
        
        else {
            anim.SetBool("moving", false);
            rb2d.velocity = new Vector2(0, 0);
        }
        
        
    }

	// This method is fired whenever the Player's collider passes through an 'isTrigger' collider
	void OnTriggerEnter2D(Collider2D other){

		// Check if trigger is world item
		worldItem itemDat = other.gameObject.GetComponent<worldItem> ();
		if (itemDat != null) {
			// Stop player
			anim.SetBool("moving", false);
			rb2d.velocity = new Vector2(0, 0);

			pickupMenu.activate (itemDat.id, other.gameObject);
			menuOpen = true;

		}
	}

	public void addItemToInv(int id) {
		inv.addItem (id);
		menuOpen = false;
	}

	public void closeMenu() {
		menuOpen = false;
	}

	private void toggleInventory () {
		inv.toggleActive ();
		isInvOpen = !isInvOpen;

		// Stop player
		anim.SetBool("moving", false);
		rb2d.velocity = new Vector2(0, 0);
	}
}

 
