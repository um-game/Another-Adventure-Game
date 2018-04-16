using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO: setup save / load system for items.
// This could be trivially:
// itemID,numItems
public class Synergy : MonoBehaviour {

	GameObject inventoryPanel; // Holds slot panel
	GameObject slotPanel; // Holds slots
	public GameObject inventorySlot; // Prefab instance of an inventory slot
	public GameObject inventoryItem; // Prefab instance of an inventory item

	int numSlots;

	public List<AdventureItem> allItems; // Holds all the item instances for the inventory
	public List<GameObject> allSlots; // Holds all the slot instances for the inventory

	ItemDatabase itemDB;

	public static Synergy mySynergy;

	// Use this for initialization
	void Start () {

		if (mySynergy == null)
		{
			allItems = new List<AdventureItem>();
			allSlots = new List<GameObject>();
			itemDB = GameObject.Find ("Inventory").GetComponent<ItemDatabase> ();

			inventoryPanel = GameObject.Find("synPanel");
			slotPanel = inventoryPanel.transform.Find("slotPanel").gameObject;
			DontDestroyOnLoad(inventoryPanel.transform.root.gameObject);
			DontDestroyOnLoad(slotPanel.transform.root.gameObject);

			numSlots = 4;
			for (int i = 0; i < numSlots; i++)
			{
				allItems.Add(new AdventureItem()); // Add empty item
				allSlots.Add(Instantiate(inventorySlot)); // Create instance of slot prefab
				allSlots[i].transform.SetParent(slotPanel.transform); // Set correct parent
				allSlots[i].transform.localScale = new Vector3(1, 1, 1);
				allSlots[i].GetComponent<Slot>().ID = i; // Set ID of slot
			}

			// Load ID's and add items here


			inventoryPanel.SetActive(false);

			mySynergy = this;
		}
		else if (mySynergy != this)
		{
			Destroy(gameObject);
		}

	}

	public void addItem(int id) {
		AdventureItem itemToAdd = itemDB.getItem (id);
		itemToAdd.equipped = true;

		for (int i = 0; i < allItems.Count; i++) {
			if (allItems [i].ID == -1) { // Check for 'empty slot'
				allItems [i] = itemToAdd; // Assign empty slot to new item
				GameObject itemObject = Instantiate (inventoryItem); // Create instance of item prefab

				itemObject.GetComponent<ItemData>().init(itemToAdd, i); // Initialize itemData
				itemObject.transform.SetParent (allSlots [i].transform); // Set correct parent
				itemObject.transform.localScale = new Vector3(1,1,1);
				itemObject.transform.localPosition = new Vector2(0, 2); // Center item in slot
				itemObject.GetComponent<Image>().sprite = itemToAdd.Sprite; // Replace default sprite w/ item sprite
				itemObject.name = itemToAdd.Title; // Set name of prefab to name of item(for convenience)
				return;
			}
		}
	}

	public void removeItem(AdventureItem itemToRemove) {


		for(int i = 0; i < allItems.Count; i++) {

			if (itemToRemove.ID == allItems[i].ID) {

				ItemData currData = allSlots [i].transform.GetChild (0).GetComponent<ItemData> ();
				currData.decreaseAmt (1);
				if(currData.amt == 0) {
					currData.removeItem ();
					allSlots [i].transform.GetChild (0).transform.gameObject.SetActive (false); // Hmmmm
					allSlots[i].transform.DetachChildren();
					allItems [i] = new AdventureItem ();
					return;
				}
			}
		}
	}

	bool itemAlreadyExists(AdventureItem item) {

		for (int i = 0; i < allItems.Count; i++) {
			if (allItems [i].ID == item.ID) {
				return true;
				// Return index instead, then use that, this circumvents redundantly looping through the items again
			}
		}
		return false;
		// Otherwise return -1(bad item id)
	}

	public void toggleActive() {

		// NULL?
		inventoryPanel.SetActive (!inventoryPanel.activeSelf);

	}

	// Update is called once per frame
	void Update () {}

	public void printInv() {
		foreach (AdventureItem item in allItems) {
			Debug.Log (item.getDataStr());
		}
	}

	private void printSlots() {

		foreach (GameObject obj in allSlots) {
			Debug.Log(obj.transform.childCount);
		}
	}
}
