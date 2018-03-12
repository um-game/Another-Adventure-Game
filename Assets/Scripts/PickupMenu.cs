using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupMenu : MonoBehaviour {

	int itemId; // Id of the item currently being examined
	PlayerController player;
	GameObject pickupMenu;
	GameObject obj; // Game object representing item
	Button[] buttons;

	// Use this for initialization
	void Start () {
		pickupMenu = GameObject.Find ("pickupMenu");
		player = GetComponent<PlayerController> ();
		buttons = pickupMenu.GetComponentsInChildren<Button> ();
		pickupMenu.SetActive (false);
	}

	public void activate(int itemId, GameObject obj) {

		pickupMenu.SetActive (true);


		buttons [0].onClick.AddListener (pickupAction);
		buttons [1].onClick.AddListener (cancelAction);

		this.itemId = itemId;
		this.obj = obj;
	}

	public void deactivate() {
		pickupMenu.SetActive (false);
	}

	void pickupAction(){
		Debug.Log ("clicked pickup button");
		player.addItemToInv (itemId);
		Destroy (obj);
		pickupMenu.SetActive (false);
	}

	void cancelAction(){
		Debug.Log ("clicked cancel button");
		this.itemId = -2;
		pickupMenu.SetActive (false);
		player.closeMenu ();
	}
}
