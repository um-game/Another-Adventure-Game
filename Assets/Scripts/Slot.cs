using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum slotType { INV, SYN, EQP, NA }

public class Slot : MonoBehaviour, IDropHandler {

	private Inventory inv;
	private Synergy syn;
	private Equipment equip;
	private Player player;

	public slotType type; // enum corresponding to type of slot

	public int uniqueID { get; set; }

	// Use this for initialization
	void Start () {
		// Grab the inventory
		inv = GameObject.Find ("Inventory").GetComponent<Inventory> ();
		syn = GameObject.Find ("Synergy").GetComponent<Synergy> ();
		equip = GameObject.Find ("Equipment").GetComponent<Equipment>();
		player = GameObject.Find ("player").GetComponent<Player> ();
	}
	
	public void OnDrop(PointerEventData eventData) {

		ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData> (); // Get item from event data

		List<List<int>> allUID = new List<List<int>> ();
		allUID.Add (inv.uids);
		allUID.Add (syn.uids);
		allUID.Add (equip.uids);

		slotType prevType = Slot.uidToType (allUID, droppedItem.slotUID);

		// Figure out the local ID
		int priorLocalID = uidToLocal(prevType, droppedItem.slotUID);
		int localID = uidToLocal(this.type, this.uniqueID);

		// TODO: implement swapping

//		Debug.Log (localID);

		// Check what kind of slot we are
		switch(type) {

		case slotType.INV:
			{
				// If there is no item in the slot, put it in there 
				if (inv.allItems [localID].ID == -1) {
					removePrior (prevType, priorLocalID);
					inv.allItems [localID] = droppedItem.item; // Update item in slot
					droppedItem.slotUID = this.uniqueID;  // Update ID

				} else if (droppedItem.slotUID != this.uniqueID) { // Otherwise, swap the item locations

					Transform item = this.transform.GetChild (0); // Get item in current slot
					item.GetComponent<ItemData> ().slotUID = droppedItem.slotUID; // Assign new slot id
					item.transform.SetParent (inv.allSlots [priorLocalID].transform); // Set parent to new slot
					item.transform.position = inv.allSlots [priorLocalID].transform.position;

					// Swap the items in the list(before re-assigning droppedItem's slot ID
					inv.allItems [priorLocalID] = item.GetComponent<ItemData> ().item;
					inv.allItems [localID] = droppedItem.item;

					// Reassign ID's
					droppedItem.slotUID = this.uniqueID;
					droppedItem.transform.SetParent (this.transform);
					droppedItem.transform.localPosition = new Vector3 (0, 0, 0);
				}
				break;
			}

		case slotType.EQP:
			{
				System.Type currentItemType = droppedItem.item.GetType ();
				AdventureItem currItem = droppedItem.item;

				// Make sure it is the right type of item
				if(currentItemType == typeof(ItemArmor) || currentItemType == typeof(ItemWeapon)) {
//					Debug.Log("EQP");

					if(equip.allItems[localID].ID == -1) {

						// This little bit ensures the proper piece of equipment ends up in the proper slot
						bool isHead = currItem.itemType == ItemType.head && localID == (int)ItemType.head;
						bool isweapon = currItem.itemType == ItemType.weapon && localID == (int)ItemType.weapon;
						bool isChest = currItem.itemType == ItemType.chest && localID == (int)ItemType.chest;
						bool isShield = currItem.itemType == ItemType.shield && localID == (int)ItemType.shield;

						// If one of them is true, proceed accordingly
						if (isHead || isweapon || isChest || isShield) {
							droppedItem.item.equipped = true; // 0. NOTE: We set equipped to true to ensure it does not get equipped twice
							droppedItem.slotUID = this.uniqueID; // 1. update dropped item's ID
							removePrior (prevType, priorLocalID); // 2. null out the previous slot that the item was just in
							equip.allItems [localID] = droppedItem.item; // 3. update the item in this slot
							player.useItem (droppedItem.item); // 4. update player stats accordingly
						}
					}
				}
				break;
			}
		case slotType.SYN:
			{
				// Make sure it is the right type of item
				if (droppedItem.item.GetType() == typeof(ItemSynergy)) {

					if (syn.allItems [localID].ID == -1) {
						removePrior (prevType, priorLocalID); // Null out the previous slot
						syn.allItems [localID] = droppedItem.item; // Update item in slot
						syn.allItemsClone[localID] = droppedItem.item;

						// Update clone panel
						Transform slotCloneT = syn.allSlotsClone[localID].transform;
						GameObject itemObj = Instantiate (inv.inventoryItem);
						itemObj.transform.SetParent (slotCloneT);
						itemObj.GetComponent<Image> ().sprite = droppedItem.item.Sprite;
						itemObj.transform.localPosition = new Vector2 (0, 2);
						itemObj.GetComponent<ItemData>().init(droppedItem.item, localID, syn.allSlots[localID].GetComponent<Slot>().uniqueID);
						slotCloneT.localScale = new Vector3(1, 1, 1);

						droppedItem.slotUID = this.uniqueID;
					} 
//					else if (droppedItem.slotUID != this.uniqueID) { // Otherwise, swap the item locations
//
//						Transform item = this.transform.GetChild (0); // Get item in current slot
//						item.GetComponent<ItemData> ().slotUID = droppedItem.slotUID; // Assign new slot id
//						item.transform.SetParent (inv.allSlots [priorLocalID].transform); // Set parent to new slot
//						item.transform.position = inv.allSlots [priorLocalID].transform.position;
//
//						// Swap the items in the list(before re-assigning droppedItem's slot ID
//						syn.allItems [priorLocalID] = item.GetComponent<ItemData> ().item;
//						syn.allItems [localID] = droppedItem.item;
//
//						// Move item currrently in slot to 'old' slot
//						droppedItem.slotUID = this.uniqueID;
//						droppedItem.transform.SetParent (this.transform);
//						droppedItem.transform.localPosition = new Vector3 (0, 0, 0);
//					}
//					syn.printInv ();
				}
				break;
			}
		}
		// Check if we need to turn buffs on/off
		player.checkBuff ();
	}

	// Method to remove the dropped item from the previous slot
	void removePrior(slotType prevType, int prevID) {

		switch (prevType) {

		case slotType.INV:
			inv.allItems [prevID] = new AdventureItem ();
			break;

		case slotType.EQP:
			equip.allItems [prevID].equipped = false;
			player.unEquip (equip.allItems [prevID]);
			equip.allItems [prevID] = new AdventureItem ();
			break;

		case slotType.SYN:
			Destroy(syn.allSlotsClone [prevID].transform.GetChild (0).transform.gameObject);
			syn.allItems [prevID] = new AdventureItem ();
			syn.allItemsClone [prevID] = new AdventureItem ();
			break;
		}

	}

	// Method to convert a unique ID to a local one
	int uidToLocal(slotType slotT, int UID) {

		switch (slotT) {

		case slotType.INV:
//			Debug.Log ("inv");
			return inv.uidToLocal (UID);

		case slotType.EQP:
//			Debug.Log ("eqp");
			return equip.uidToLocal (UID);

		case slotType.SYN:
//			Debug.Log ("syn");
			return syn.uidToLocal (UID);

		default:
			Debug.Log ("default");
			return -1;
		}
	}

	public static slotType uidToType(List<List<int>> allUID, int UID) {
		for (int i = 0; i < allUID.Count; i++) {
			for (int j = 0; j < allUID [i].Count; j++) {
				if (UID == allUID [i] [j]) {
					return(slotType)i;
				}
			}
		}
		return slotType.NA;
	}
}
