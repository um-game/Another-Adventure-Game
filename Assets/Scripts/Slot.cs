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

	public slotType type;

	public int ID { get; set; }

	public int uniqueID { get; set; }

	// Use this for initialization
	void Start () {
		// Grab the inventory
		inv = GameObject.Find ("Inventory").GetComponent<Inventory> ();
		syn = GameObject.Find ("Synergy").GetComponent<Synergy> ();
		equip = GameObject.Find ("Equipment").GetComponent<Equipment>();
		player = GameObject.Find ("player").GetComponent<Player> ();
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

		// TODO: implement swapping
		// TODO: update stats

		int localID = uidToLocal();
		Debug.Log (localID);

		switch(type) {

		case slotType.INV:
			{
				// If there is no item in the slot, put it in there 
				if (inv.allItems [localID].ID == -1) {
					removePrior (prevType, priorLocalID);
					inv.allItems [localID] = droppedItem.item; // Update item in slot
					droppedItem.slotId = localID; // Update which slot we are in
					droppedItem.slotUID = this.uniqueID;


//					inv.allItems [priorLocalID] = new AdventureItem (); // Null out the previous slot
//					inv.allItems [this.ID] = droppedItem.item; // Update item in slot
//					droppedItem.slotId = ID; // Update which slot we are in

				} else if (droppedItem.slotUID != this.uniqueID) { // Otherwise, swap the item locations


					Transform item = this.transform.GetChild (0); // Get item in current slot
					item.GetComponent<ItemData> ().slotUID = droppedItem.slotUID; // Assign new slot id
					item.transform.SetParent (inv.allSlots [priorLocalID].transform); // Set parent to new slot
					item.transform.position = inv.allSlots [priorLocalID].transform.position;

					// Swap the items in the list(before re-assigning droppedItem's slot ID
					inv.allItems [priorLocalID] = item.GetComponent<ItemData> ().item;
					inv.allItems [localID] = droppedItem.item;

					// Move item currrently in slot to 'old' slot
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

				if(currentItemType == typeof(ItemArmor) || currentItemType == typeof(ItemWeapon)) {
					Debug.Log("EQP");

					if(equip.allItems[localID].ID == -1) {

						if (currItem.itemType == ItemType.weapon && localID == (int)ItemType.weapon) {
							removePrior (prevType, priorLocalID);
							equip.allItems [localID] = droppedItem.item; // Update item in slot
							droppedItem.slotId = localID; // Update which slot we are in
							droppedItem.slotUID = this.uniqueID;
							player.useItem (droppedItem.item);
						} else if(currItem.itemType == ItemType.shield && localID == (int)ItemType.shield) {
							removePrior (prevType, priorLocalID);
							equip.allItems [localID] = droppedItem.item; // Update item in slot
							droppedItem.slotId = localID; // Update which slot we are in
							droppedItem.slotUID = this.uniqueID;
							player.useItem (droppedItem.item);

						}  else if(currItem.itemType == ItemType.head && localID == (int)ItemType.head) {
							removePrior (prevType, priorLocalID);
							equip.allItems [localID] = droppedItem.item; // Update item in slot
							droppedItem.slotId = localID; // Update which slot we are in
							droppedItem.slotUID = this.uniqueID;
							player.useItem (droppedItem.item);

						}  else if(currItem.itemType == ItemType.chest && localID == (int)ItemType.chest) {
							removePrior (prevType, priorLocalID);
							equip.allItems [localID] = droppedItem.item; // Update item in slot
							droppedItem.slotId = localID; // Update which slot we are in
							droppedItem.slotUID = this.uniqueID;
							player.useItem (droppedItem.item);
						}
					}
				}
				break;
			}
		case slotType.SYN:
			{

				if (droppedItem.item.GetType() == typeof(ItemSynergy)) {

					if (syn.allItems [localID].ID == -1) {
						removePrior (prevType, priorLocalID); // Null out the previous slot
						syn.allItems [localID] = droppedItem.item; // Update item in slot
						syn.allItemsClone[localID] = droppedItem.item;

						// Update other panel
						GameObject itemObj = Instantiate (inv.inventoryItem);
						itemObj.transform.SetParent (syn.allSlotsClone [localID].transform);
						itemObj.GetComponent<Image> ().sprite = droppedItem.item.Sprite;
						itemObj.transform.localPosition = new Vector2 (0, 2);
						itemObj.GetComponent<ItemData>().init(droppedItem.item, localID, syn.allSlots[localID].GetComponent<Slot>().uniqueID);


						droppedItem.slotId = localID; // Update which slot we are in
						droppedItem.slotUID = this.uniqueID;
					}
					syn.printInv ();
					
				}
				break;
			}
		}
		player.checkBuff ();
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
			Destroy(syn.allSlotsClone [prevID].transform.GetChild (0).transform.gameObject);
			syn.allItems [prevID] = new AdventureItem ();
			syn.allItemsClone [prevID] = new AdventureItem ();
			break;
		}

	}

	int uidToLocal() {

		switch (this.type) {

		case slotType.INV:
			Debug.Log ("inv");
			return inv.uidToLocal (this.uniqueID);

		case slotType.EQP:
			Debug.Log ("eqp");
			return equip.uidToLocal (this.uniqueID);

		case slotType.SYN:
			Debug.Log ("syn");
			return syn.uidToLocal (this.uniqueID);

		default:
			Debug.Log ("default");
			return -1;
		}
	}
}
