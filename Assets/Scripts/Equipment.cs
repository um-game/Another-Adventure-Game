using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour {

	GameObject equipmentPanel; // Holds slot panel
	GameObject slotPanel; // Holds slots
	public GameObject inventorySlot; // Prefab instance of an inventory slot
	public GameObject inventoryItem; // Prefab instance of an inventory item

	int numSlots;

	public List<AdventureItem> allItems; // Holds all the item instances for the equipment
	public List<int> uids;

	// There will be five slots.
	// slot 0: head gear
	// slot 1: attacking weapon
	// slot 2: chest gear
	// slot 3: shield?
	// slot 4: pants.
	public List<GameObject> allSlots; // Holds all the slot instances for the equipment

	ItemDatabase itemDB;

	public static Equipment myEquip;

	// Use this for initialization
	void Start () {

		if (myEquip == null) {

			allItems = new List<AdventureItem> ();
			allSlots = new List<GameObject> ();
			uids = new List<int> ();
			itemDB = GameObject.Find ("Inventory").GetComponent<ItemDatabase> ();

			equipmentPanel = GameObject.Find ("equipmentPanel");
			slotPanel = equipmentPanel.transform.Find ("slotPanel").gameObject;

			DontDestroyOnLoad(equipmentPanel.transform.root.gameObject);
			DontDestroyOnLoad(slotPanel.transform.root.gameObject);

			numSlots = 5;
			for (int i = 0; i < numSlots; i++) {
				allItems.Add (new AdventureItem ()); // Add empty item
				allSlots.Add (slotPanel.transform.GetChild (i).gameObject);
				allSlots [i].GetComponent<Slot> ().ID = i; // Set ID of slot

				Slot currSlot = allSlots [i].GetComponent<Slot> ();
				currSlot.ID = i; // Set ID of slot
				currSlot.uniqueID = Player.UID;
				uids.Add (Player.UID);
//				currSlot.GetComponent<ItemData> ().slotUID = Player.UID;
				Player.UID += 1;
				currSlot.type = slotType.EQP;
			}
			
			// Load ID's and and items here

			equipmentPanel.SetActive (false);

			myEquip = this;

		} else if(myEquip != null) {
		
			Destroy(gameObject);

		}
	}
		
	public void addItem(int id) {
		AdventureItem itemToAdd = itemDB.getItem (id);
		itemToAdd.equipped = true;

		int slotI = (int)itemToAdd.itemType;

		if (allItems [slotI].ID != -1) { // Check for 'non - empty' slot
			// If item equipped, put back into inventory 

//			return;
			AdventureItem currentItem = allItems [slotI];
			GameObject.Find ("Inventory").GetComponent<Inventory> ().addItem (currentItem.ID); // Put back in inventory
			currentItem.equipped = false;
			removeItem(currentItem);
		}

		// Assign current slot
		allItems [slotI] = itemToAdd; // Assign empty slot to new item

		GameObject itemObject = Instantiate (inventoryItem); // Create instance of item prefab
		itemObject.GetComponent<ItemData> ().init (itemToAdd, slotI, allSlots[slotI].GetComponent<Slot>().uniqueID); // Initialize itemData
		itemObject.transform.SetParent (allSlots [slotI].transform); // Set correct parent
        itemObject.transform.localScale = new Vector3(1,1,1);
        itemObject.transform.localPosition = Vector2.zero; // Center item in slot
		itemObject.GetComponent<Image> ().sprite = itemToAdd.Sprite; // Replace default sprite w/ item sprite
		itemObject.name = itemToAdd.Title; // Set name of prefab to name of item(for convenience)
	}
		
	public void removeItem(AdventureItem itemToRemove) {

		int itemToRmI = (int)itemToRemove.itemType;

		ItemData currData = allSlots [itemToRmI].transform.GetChild (0).GetComponent<ItemData> ();
		currData.removeItem ();

		GameObject item = allSlots [itemToRmI].transform.GetChild (0).transform.gameObject;
		Destroy (item);
//		allSlots [itemToRmI].transform.GetChild (0).transform.gameObject.SetActive (false); // Hmmmm
//		allSlots[itemToRmI].transform.DetachChildren();
		allItems [itemToRmI] = new AdventureItem ();
	}

	public void toggleActive() {
		equipmentPanel.SetActive (!equipmentPanel.activeSelf);
	}
		
	public void printEquipment() {
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
