using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO: setup save / load system for items.
// This could be trivially:
// itemID,numItems
public class Inventory : MonoBehaviour {

	GameObject inventoryPanel; // Holds slot panel
	GameObject slotPanel; // Holds slots
	public GameObject inventorySlot; // Prefab instance of an inventory slot
	public GameObject inventoryItem; // Prefab instance of an inventory item

	int numSlots;

	public List<AdventureItem> allItems; // Holds all the item instances for the inventory
	public List<GameObject> allSlots; // Holds all the slot instances for the inventory

	ItemDatabase itemDB;

	public List<int> uids;

    public static Inventory myInventory;

	// Use this for initialization
	void Start () {
	
        if (myInventory == null)
        {
            allItems = new List<AdventureItem>();
            allSlots = new List<GameObject>();
			uids = new List<int> ();
            itemDB = GetComponent<ItemDatabase>();

            inventoryPanel = GameObject.Find("inventoryPanel");
            slotPanel = inventoryPanel.transform.Find("slotPanel").gameObject;
            DontDestroyOnLoad(inventoryPanel.transform.root.gameObject);
            DontDestroyOnLoad(slotPanel.transform.root.gameObject);

            numSlots = 20;
            for (int i = 0; i < numSlots; i++)
            {
                allItems.Add(new AdventureItem()); // Add empty item
                allSlots.Add(Instantiate(inventorySlot)); // Create instance of slot prefab
                allSlots[i].transform.SetParent(slotPanel.transform); // Set correct parent
                allSlots[i].transform.localScale = new Vector3(1,1,1);
                
				Slot currSlot = allSlots [i].GetComponent<Slot> ();
				currSlot.ID = i; // Set ID of slot
				currSlot.uniqueID = Player.UID;
//				currSlot.GetComponent<ItemData> ().slotUID = Player.UID;
				uids.Add (Player.UID);
				Player.UID += 1;
				currSlot.type = slotType.INV;

            }

            // Load ID's and add items here


            // This is just here to show off the functionality of the inventory...
			addItem(4);
			addItem (5);
			addItem(7);
			addItem(9);
			addItem(14);
			addItem (6);
			addItem (16);
			addItem (17);
			addItem (18);
			addItem (19);
			addItem (20);
			addItem (21);
			addItem (22);

            inventoryPanel.SetActive(false);

            myInventory = this;
        }
		else if (myInventory != this)
        {
            Destroy(gameObject);
        }

    }

	public void addItem(int id) {
		AdventureItem itemToAdd = itemDB.getItem (id);

		if (itemToAdd.IsStackable && itemAlreadyExists(itemToAdd)) {
			for (int i = 0; i < allItems.Count; i++) {
				if (allItems [i].ID == id) {
					allSlots [i].transform.GetChild (0).GetComponent<ItemData> ().increaseAmt(1); // Update associated item data
					return;
				}
			}
		}

		for (int i = 0; i < allItems.Count; i++) {
			if (allItems [i].ID == -1) { // Check for 'empty slot'
				allItems [i] = itemToAdd; // Assign empty slot to new item
				GameObject itemObject = Instantiate (inventoryItem); // Create instance of item prefab

				itemObject.GetComponent<ItemData>().init(itemToAdd, i, allSlots[i].GetComponent<Slot>().uniqueID); // Initialize itemData
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

					GameObject item = allSlots [i].transform.GetChild (0).transform.gameObject;
					Destroy (item);
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

		// Shut down the tooltip and itemMenu too, if it is active
		GetComponent<ToolTip>().deactivate();
		GetComponent<ItemMenu> ().deactivate ();
        
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
