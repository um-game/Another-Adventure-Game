using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemMenu : MonoBehaviour {

	GameObject itemMenu;
	Button[] buttons;
	AdventureItem item;

	// Use this for initialization
	void Start () {
		itemMenu = GameObject.Find ("itemMenu");
		buttons = itemMenu.GetComponentsInChildren<Button> ();
		buttons [0].onClick.AddListener (useAction);
		buttons [1].onClick.AddListener (removeAction);
		itemMenu.SetActive(false);
	}

	public void activate(AdventureItem item) {

		itemMenu.SetActive (true);

		this.item = item;
	}

	public void deactivate() {
		itemMenu.SetActive (false);
	}

	// Update is called once per frame
	void Update () {


	}
		

	void useAction(){

		Debug.Log ("clicked use button");

		itemMenu.SetActive (false);
	}

	void removeAction(){
		Debug.Log ("clicked remove button");
		GetComponent<Inventory> ().removeItem (item);
		itemMenu.SetActive (false);
	}
}
