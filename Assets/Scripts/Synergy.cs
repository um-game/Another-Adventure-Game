using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO: setup save / load system for items.
// This could be trivially:
// itemID,numItems
public class Synergy : MonoBehaviour {

	GameObject inventoryPanel; // Holds slot panel
    GameObject inventoryPanelClone;
	GameObject slotPanel; // Holds slots
    GameObject slotPanelClone;
	public GameObject inventorySlot; // Prefab instance of an inventory slot
	public GameObject inventoryItem; // Prefab instance of an inventory item

	int numSlots;

	public List<AdventureItem> allItems; // Holds all the item instances for the inventory
    public List<AdventureItem> allItemsClone;
	public List<GameObject> allSlots; // Holds all the slot instances for the inventory
    public List<GameObject> allSlotsClone;
	public List<int> uids;

	ItemDatabase itemDB;

	public static Synergy mySynergy;

	// Use this for initialization
	void Start () {

		if (mySynergy == null)
		{
			allItems = new List<AdventureItem>();
            allItemsClone = new List<AdventureItem>();
			allSlots = new List<GameObject>();
            allSlotsClone = new List<GameObject>();
			uids = new List<int> ();
			itemDB = GameObject.Find ("Inventory").GetComponent<ItemDatabase> ();

			inventoryPanel = GameObject.Find("synPanel");
            inventoryPanelClone = GameObject.Find("synPanel2");
			slotPanel = inventoryPanel.transform.Find("slotPanel").gameObject;
            slotPanelClone = inventoryPanelClone.transform.Find("slotPanel").gameObject;
            DontDestroyOnLoad(inventoryPanel.transform.root.gameObject);
			DontDestroyOnLoad(slotPanel.transform.root.gameObject);
            DontDestroyOnLoad(inventoryPanelClone.transform.root.gameObject);
            DontDestroyOnLoad(slotPanelClone.transform.root.gameObject);

            numSlots = 4;
			for (int i = 0; i < numSlots; i++)
			{
				allItems.Add(new AdventureItem()); // Add empty item
                allItemsClone.Add(new AdventureItem());
				allSlots.Add(Instantiate(inventorySlot)); // Create instance of slot prefab
                allSlotsClone.Add(Instantiate(inventorySlot));
				allSlots[i].transform.SetParent(slotPanel.transform); // Set correct parent
                allSlotsClone[i].transform.SetParent(slotPanelClone.transform);
				allSlots[i].transform.localScale = new Vector3(1, 1, 1);
                allSlotsClone[i].transform.localScale = new Vector3(1, 1, 1);

				Slot currSlot = allSlots [i].GetComponent<Slot> ();
				currSlot.uniqueID = Player.UID; // Set unique ID of slot
				uids.Add (Player.UID); // Add to list
				Player.UID += 1; // Increment
				currSlot.type = slotType.SYN; // Set proper slot type
            }

			// Load ID's and add items here

			inventoryPanel.SetActive(false);
            inventoryPanelClone.SetActive(true);

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
                allItemsClone[i] = itemToAdd;
				GameObject itemObject = Instantiate (inventoryItem); // Create instance of item prefab
                GameObject itemObjectClone = Instantiate(inventoryItem);

				itemObject.GetComponent<ItemData>().init(itemToAdd, i, allSlots[i].GetComponent<Slot>().uniqueID); // Initialize itemData
				itemObjectClone.GetComponent<ItemData>().init(itemToAdd, i, allSlots[i].GetComponent<Slot>().uniqueID);
                itemObject.transform.SetParent (allSlots [i].transform); // Set correct parent
                itemObjectClone.transform.SetParent(allSlotsClone[i].transform);
                itemObject.transform.localScale = new Vector3(1,1,1);
                itemObjectClone.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                itemObject.transform.localPosition = new Vector2(0, 2); // Center item in slot
                itemObjectClone.transform.localPosition = new Vector2(0, 2);
                itemObject.GetComponent<Image>().sprite = itemToAdd.Sprite; // Replace default sprite w/ item sprite
                itemObjectClone.GetComponent<Image>().sprite = itemToAdd.Sprite;
                itemObject.name = itemToAdd.Title; // Set name of prefab to name of item(for convenience)
                itemObjectClone.name = itemToAdd.Title;
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


					Destroy (allSlotsClone [i].transform.GetChild (0).transform.gameObject);
					allItemsClone [i] = new AdventureItem ();

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
		inventoryPanel.SetActive (!inventoryPanel.activeSelf);
	}

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

	public void printUID() {
		foreach (GameObject obj in allSlots) {
			Debug.Log (obj.GetComponent<Slot> ().uniqueID);
		}
			
	}

	public int uidToLocal(int UID) {
		for (int i = 0; i < uids.Count; i++) {
			if (uids [i] == UID) {
				return i;
			}
		}
		return -1;
	}
}
