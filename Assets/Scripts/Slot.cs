using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum slotType { INV, SYN, EQP, NA }


public class Slot : MonoBehaviour, IDropHandler {

	private Inventory inv;
	private Synergy syn;
	private Equipment equip;

	public slotType type;

	public int ID { get; set; }

	public int uniqueID { get; set; }

	// Use this for initialization
	void Start () {
		// Grab the inventory
		inv = GameObject.Find ("Inventory").GetComponent<Inventory> ();
		syn = GameObject.Find ("Synergy").GetComponent<Synergy> ();
		equip = GameObject.Find ("Equipment").GetComponent<Equipment>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void OnDrop(PointerEventData eventData) {

		ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData> (); // Get item from event data


		List<List<int>> allUID = new List<List<int>> ();

		allUID.Add (inv.uids);
		allUID.Add (syn.uids);
		allUID.Add (equip.uids);

		slotType prevType = slotType.NA;

		for (int i = 0; i < allUID.Count; i++) {

			for (int j = 0; j < allUID [i].Count; j++) {

				if (droppedItem.slotUID == allUID [i] [j]) {
					prevType = (slotType)i;
					break;
				}
			}
		}

		int priorLocalID;

		switch (prevType) {

		case slotType.INV:
			priorLocalID = inv.uidToLocal (droppedItem.slotUID);
			break;

		case slotType.SYN:
			priorLocalID = syn.uidToLocal (droppedItem.slotUID);
			break;

		case slotType.EQP:
			priorLocalID = equip.uidToLocal (droppedItem.slotUID);
			break;

		default:
			priorLocalID = -1;
			Debug.Log ("NA ENCOUNTERED ERROR");
			break;

		}


		int localID = uidToLocal();
		Debug.Log (localID);

		switch(type) {

		case slotType.INV:
			{
				// If there is no item in the slot, put it in there 
				if (inv.allItems [this.ID].ID == -1) {
					removePrior (prevType, priorLocalID);
					inv.allItems [localID] = droppedItem.item; // Update item in slot
					droppedItem.slotId = localID; // Update which slot we are in
					droppedItem.slotUID = this.uniqueID;


//					inv.allItems [priorLocalID] = new AdventureItem (); // Null out the previous slot
//					inv.allItems [this.ID] = droppedItem.item; // Update item in slot
//					droppedItem.slotId = ID; // Update which slot we are in

				} else if (droppedItem.slotId != this.ID) { // Otherwise, swap the item locations


					Transform item = this.transform.GetChild (0); // Get item in current slot
					item.GetComponent<ItemData> ().slotId = droppedItem.slotId; // Assign new slot id
					item.transform.SetParent (inv.allSlots [droppedItem.slotId].transform); // Set parent to new slot
					item.transform.position = inv.allSlots [droppedItem.slotId].transform.position;

					// Swap the items in the list(before re-assigning droppedItem's slot ID
					inv.allItems [droppedItem.slotId] = item.GetComponent<ItemData> ().item;
					inv.allItems [this.ID] = droppedItem.item;

					// Move item currrently in slot to 'old' slot
					droppedItem.slotId = this.ID;
					droppedItem.transform.SetParent (this.transform);
					droppedItem.transform.localPosition = new Vector3 (0, 0, 0);
				}
				break;
			}

		case slotType.EQP:
			{
				System.Type currentItemType = droppedItem.item.GetType ();
				AdventureItem currItem = droppedItem.item;

				if(currentItemType == typeof(ItemArmor) || currentItemType == typeof(ItemWeapon)) {
					Debug.Log("EQP");

//					int localID = 0;

//					for(int i = 0; i < equip.uids.Count; i++) {
//						if (equip.uids[i] == this.uniqueID) {
//							localID = i;
//							break;
//						}
//					}

					if(equip.allItems[this.ID].ID == -1) {

						if (currItem.itemType == ItemType.weapon && this.ID == (int)ItemType.weapon) {
							removePrior (prevType, priorLocalID);
							equip.allItems [localID] = droppedItem.item; // Update item in slot
							droppedItem.slotId = localID; // Update which slot we are in
							droppedItem.slotUID = this.uniqueID;
						} else if(currItem.itemType == ItemType.shield && this.ID == (int)ItemType.shield) {
							removePrior (prevType, priorLocalID);
							equip.allItems [localID] = droppedItem.item; // Update item in slot
							droppedItem.slotId = localID; // Update which slot we are in
							droppedItem.slotUID = this.uniqueID;
						}  else if(currItem.itemType == ItemType.head && this.ID == (int)ItemType.head) {
							removePrior (prevType, priorLocalID);
							equip.allItems [localID] = droppedItem.item; // Update item in slot
							droppedItem.slotId = localID; // Update which slot we are in
							droppedItem.slotUID = this.uniqueID;
						}  else if(currItem.itemType == ItemType.chest && this.ID == (int)ItemType.chest) {
							removePrior (prevType, priorLocalID);
							equip.allItems [localID] = droppedItem.item; // Update item in slot
							droppedItem.slotId = localID; // Update which slot we are in
							droppedItem.slotUID = this.uniqueID;
						}
					}

				}
				break;
			}
		case slotType.SYN:
			{

				if (droppedItem.item.GetType() == typeof(ItemSynergy)) {


//					int localID = 0;

//					List<int> synUID = syn.uids;
//					for(int i = 0; i < synUID.Count; i++) {
//						if (synUID [i] == this.uniqueID) {
//							localID = i;
//							break;
//						}
//					}

					if (syn.allItems [localID].ID == -1) {
						removePrior (prevType, priorLocalID);
//						syn.allItems [droppedItem.slotId] = new AdventureItem (); // Null out the previous slot
						syn.allItems [localID] = droppedItem.item; // Update item in slot
						droppedItem.slotId = localID; // Update which slot we are in
						droppedItem.slotUID = this.uniqueID;
					}
					syn.printInv ();
					
				}

				break;
			}
		}
	}

	void removePrior(slotType prevType, int prevID) {

		switch (prevType) {

		case slotType.INV:
			inv.allItems [prevID] = new AdventureItem ();
			break;

		case slotType.EQP:
			equip.allItems [prevID] = new AdventureItem ();
			break;

		case slotType.SYN:
			syn.allItems [prevID] = new AdventureItem ();
			break;
		}

	}

	int uidToLocal() {

		int newID;

		switch (this.type) {

		case slotType.INV:
			Debug.Log ("inv");
			newID = inv.uidToLocal (this.uniqueID);
			break;

		case slotType.EQP:
			Debug.Log ("eqp");
			newID = equip.uidToLocal (this.uniqueID);
			break;

		case slotType.SYN:
			Debug.Log ("syn");
			newID = syn.uidToLocal (this.uniqueID);
			break;

		default:
			Debug.Log ("default");
			newID = -1;
			break;
		}

		return newID;

	}
}
