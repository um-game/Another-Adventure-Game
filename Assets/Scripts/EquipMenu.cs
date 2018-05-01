using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipMenu : MonoBehaviour {

	AdventureItem item; // Id of the item currently being examined
	Player player;
	public static GameObject equipMenu;
	GameObject obj; // Game object representing item
	Button[] buttons;
	int slotUID;
	Synergy syn;

	public static EquipMenu myEquipMenu;

    private PopupCanvas myCanvas;

	// Use this for initialization
	void Start () {
		if (myEquipMenu == null)
		{
			equipMenu = GameObject.Find ("equipMenu");
            myCanvas = GameObject.Find("PopupCanvas").GetComponent<PopupCanvas>();
			player = GetComponent<Player> ();
			buttons = equipMenu.GetComponentsInChildren<Button> ();
			syn = GameObject.Find ("Synergy").GetComponent<Synergy>();
			equipMenu.SetActive (false);
			buttons [0].onClick.AddListener (unequipAction);
			buttons [1].onClick.AddListener (cancelAction);
			myEquipMenu = this;
		}
		else if (myEquipMenu != this)
		{
			GameObject pickupCopy = GameObject.Find("pickupMenu");
			Destroy(pickupCopy.transform.root.gameObject);

			Debug.Log("DESTROY");
		}
	}

	public void activate(AdventureItem item, GameObject obj, int slotUID) {
		// NULL?
		equipMenu.SetActive (true);

		this.obj = obj;
		this.item = item;
	}

	public void deactivate() {
		equipMenu.SetActive (false);
	}

	void unequipAction(){
		Debug.Log ("clicked unequip button");
		player.addItemToInv (item.ID);

		if (syn.uidToLocal (slotUID) != -1) {
			syn.removeItem (this.item);
			player.checkBuff ();
		} else {
			player.unEquip (item);
			player.equipment.printEquipment ();
		}
		Destroy (obj);
		deactivate();

        myCanvas.UpdateLists();
	}
		
	void cancelAction(){ 
		equipMenu.SetActive (false);
	}
}
