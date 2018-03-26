using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler {

	private Inventory inv;

	public int ID { get; set; }

	// Use this for initialization
	void Start () {
		// Grab the inventory
		inv = GameObject.Find ("Inventory").GetComponent<Inventory> ();
	}
	
	public void OnDrop(PointerEventData eventData) {

		ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData> (); // Get item from event data

		// If there is no item in the slot, put it in there 
		if (inv.allItems [this.ID].ID == -1) {
			inv.allItems [droppedItem.slotId] = new AdventureItem (); // Null out the previous slot
			inv.allItems [this.ID] = droppedItem.item; // Update item in slot
			droppedItem.slotId = ID; // Update which slot we are in

		} else if (droppedItem.slotId != this.ID) { // Otherwise, swap the item locations
			Transform item = this.transform.GetChild(0); // Get item in current slot
			item.GetComponent<ItemData> ().slotId = droppedItem.slotId; // Assign new slot id
			item.transform.SetParent (inv.allSlots [droppedItem.slotId].transform); // Set parent to new slot
			item.transform.position = inv.allSlots [droppedItem.slotId].transform.position;

			// Swap the items in the list(before re-assigning droppedItem's slot ID)
			inv.allItems [droppedItem.slotId] = item.GetComponent<ItemData> ().item;
			inv.allItems [this.ID] = droppedItem.item;

			// Move item currently in slot to 'old' slot
			droppedItem.slotId = this.ID;
			droppedItem.transform.SetParent (this.transform);
			droppedItem.transform.localPosition = new Vector3 (0, 0, 0);
		}
	}
}
