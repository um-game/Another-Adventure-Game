using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

	public AdventureItem item;
	public int amt { get; set; }
	public int slotId; // Keep track of which slot we are in
	public int slotUID;

	ToolTip tooltip;
	Inventory inv;
	Synergy syn;
	Equipment equip;
	ItemMenu itemMenu;
	EquipMenu equipMenu;

	// Use this for initialization
	void Start () {
		inv = GameObject.Find ("Inventory").GetComponent<Inventory> ();
		syn = GameObject.Find ("Synergy").GetComponent<Synergy> ();
		equip = GameObject.Find ("Equipment").GetComponent<Equipment> ();

		tooltip = inv.GetComponent<ToolTip> ();
		itemMenu = inv.GetComponent<ItemMenu> ();
		equipMenu = GameObject.Find("player").GetComponent<EquipMenu> ();
	}
	
	// Update is called once per frame
	void Update () {}

	public void init(AdventureItem item, int slotId, int uid) {
		this.item = item;
		this.slotId = slotId;
		this.slotUID = uid;
		amt = 1;
	}

	public void removeItem() {
		this.item = new AdventureItem ();
		amt = 0;
	}
		
	public void increaseAmt(int amt) {
		this.amt += amt;
		this.transform.GetChild (0).GetComponent<Text> ().text = this.amt.ToString(); // Update label to reflect change in amount
	}

	public void decreaseAmt(int amt) {
		this.amt -= amt;
		this.transform.GetChild(0).GetComponent<Text> ().text = this.amt.ToString(); // Update label to reflect change in amount
	}
		
	// OnPointerEnter/Exit are fired when the pointer hovers over / moves off of the entity this script is attatched to
	public void OnPointerEnter(PointerEventData eventData)
	{
		tooltip.activate (item);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		tooltip.deactivate ();
	}

	public void OnPointerClick(PointerEventData eventData) {

		// This little block determines what happens when an item is clicked on
		if (item != null && !item.equipped) {
			itemMenu.activate (item, this.gameObject);
			equipMenu.deactivate ();
		} else if(item.equipped) {
			itemMenu.deactivate ();
			equipMenu.activate (item, this.gameObject);
		}
	}

	// These next few methods define most of the 'drag and drop' behavior
	public void OnBeginDrag(PointerEventData eventData) {
		// Prevents equipped item from bieng grabbed
//		if(item.equipped){ return; }

//		Debug.Log (item.equipped);
		// If we click an item, grab it.
		if (item != null) {
			this.transform.SetParent(this.transform.parent.parent); // Change parent to canvas so item is rendered on top of slots
			this.transform.position = eventData.position; // Update position
			GetComponent<CanvasGroup> ().blocksRaycasts = false; // This allows the item to be drug and dropped at will
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		// Prevents equipped item from bieng drug
//		if(item.equipped){ return; }

		// Update position of item transform
		if (item != null) {
			this.transform.position = eventData.position;
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		// Prevents equipped item from bieng 'dropped'
//		if(item.equipped){return;}


		this.GetComponent<CanvasGroup> ().blocksRaycasts = true;

		slotType currType = slotType.NA;

		List<List<int>> allUID = new List<List<int>> ();

		allUID.Add (inv.uids);
		allUID.Add (syn.uids);
		allUID.Add (equip.uids);

		for (int i = 0; i < allUID.Count; i++) {

			for (int j = 0; j < allUID [i].Count; j++) {

				if (this.slotUID == allUID [i] [j]) {
					currType = (slotType)i;
					break;
				}
			}
		}

		int localID;

		switch (currType) {

		case slotType.INV:
			localID = inv.uidToLocal (this.slotUID);
			this.transform.SetParent (inv.allSlots [localID].transform);
			break;

		case slotType.EQP:
			localID = equip.uidToLocal (this.slotUID);
			this.transform.SetParent (equip.allSlots [slotId].transform);
			break;

		case slotType.SYN:
			localID = syn.uidToLocal (this.slotUID);
			this.transform.SetParent (syn.allSlots [slotId].transform);
			break;

		default:
			localID = -1;
			Debug.Log ("ERROR NA TYPE PRESENT");
			break;
		}
		
//		this.transform.SetParent(inv.allSlots[slotId].transform);
		this.transform.localPosition = new Vector3 (0, 0, 0);
	}
}
