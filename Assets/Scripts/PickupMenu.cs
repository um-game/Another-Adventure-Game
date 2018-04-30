using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupMenu : MonoBehaviour {

	int itemId; // Id of the item currently being examined
	Player player;
	public static GameObject pickupMenu;
	GameObject obj; // Game object representing item
	Button[] buttons;

    public static PickupMenu myPickupMenu;

	// Use this for initialization
	void Start () {
        if (myPickupMenu == null)
        {
            pickupMenu = GameObject.Find("pickupMenu");
            player = GetComponent<Player>();
            buttons = pickupMenu.GetComponentsInChildren<Button>();
            pickupMenu.SetActive(false);
            buttons[0].onClick.AddListener(pickupAction);
            buttons[1].onClick.AddListener(cancelAction);

            myPickupMenu = this;
        }
        else if (myPickupMenu != this)
        {
            
            GameObject pickupCopy = GameObject.Find("pickupMenu");
            Destroy(pickupCopy.transform.root.gameObject);

            Debug.Log("DESTROY");
        }
	}

	public void activate(int itemId, GameObject obj) {
        // NULL?
        pickupMenu.SetActive (true);
	
		if (player.isInvFull ()) {
			buttons [0].interactable = false;
		} else {
			buttons [0].interactable = true;
		}

		this.itemId = itemId;
		this.obj = obj;
	}

	public void deactivate() {
		pickupMenu.SetActive (false);
        player.UnPause();
	}

	void pickupAction(){
		Debug.Log ("clicked pickup button");
		player.addItemToInv (itemId);
		Destroy (obj);
        deactivate();
	}

	void cancelAction(){
		Debug.Log ("clicked cancel button");
		this.itemId = -2;
        deactivate();
	}
}
