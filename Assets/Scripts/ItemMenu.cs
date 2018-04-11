using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemMenu : MonoBehaviour {

	GameObject itemMenu;
	Button[] buttons;
	AdventureItem item;
	GameObject obj;

	Player player;

    public static ItemMenu myItemMenu;

	// Use this for initialization
	void Start () {
        if (myItemMenu == null)
        {
            itemMenu = GameObject.Find("itemMenu");
            buttons = itemMenu.GetComponentsInChildren<Button>();
            itemMenu.SetActive(false);
			player = GameObject.Find ("player").GetComponent<Player> ();

			buttons [0].onClick.AddListener (useAction);
			buttons [1].onClick.AddListener (removeAction);
			buttons [2].onClick.AddListener (cancelAction);

            myItemMenu = this;
            DontDestroyOnLoad(myItemMenu);
        }
		else if (myItemMenu != this)
        {
            Destroy(transform.root.gameObject);
        }
	}

	public void activate(AdventureItem item, GameObject obj) {

		itemMenu.SetActive (true);


//		if (player.equipment.allItems [item.itemType] != -1) {
//		}

		this.obj = obj;
		this.item = item;
	}

	public void deactivate() {
        if (itemMenu != null)
		    itemMenu.SetActive (false);
	}

	// Update is called once per frame
	void Update () {


	}
		

	void useAction(){

		Debug.Log ("clicked use button");
		player.useItem (item);
		Destroy (obj);
//		player.printStats ();
		itemMenu.SetActive (false);
	}

	void removeAction(){
		Debug.Log ("clicked remove button");
		GetComponent<Inventory> ().removeItem (item);
		Destroy (obj);
		itemMenu.SetActive (false);
	}

	void cancelAction(){ 
		itemMenu.SetActive (false);
	}
}
