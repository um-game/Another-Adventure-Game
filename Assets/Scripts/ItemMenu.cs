using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemMenu : MonoBehaviour {

	GameObject itemMenu;
	Button[] buttons;
	AdventureItem item;

	Player player;

	// Use this for initialization
	void Start () {
		itemMenu = GameObject.Find ("itemMenu");
		buttons = itemMenu.GetComponentsInChildren<Button> ();
		buttons [0].onClick.AddListener (useAction);
		buttons [1].onClick.AddListener (dropAction);
		buttons [2].onClick.AddListener (cancelAction);
		itemMenu.SetActive(false);
		player = GameObject.Find ("player").GetComponent<Player> ();
	}

	public void activate(AdventureItem item) {

		itemMenu.SetActive (true);

		this.item = item;
	}

	public void deactivate() {
		itemMenu.SetActive (false);
	}

	void useAction(){

		Debug.Log ("clicked use button");
		player.useItem (item);
		player.printStats ();
		itemMenu.SetActive (false);
	}

	void dropAction(){
		Debug.Log ("clicked remove button");
		GetComponent<Inventory> ().removeItem (item);
		itemMenu.SetActive (false);
	}

	void cancelAction(){
		deactivate ();
	}
}
