using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour {


	AdventureItem item; // Item we are currently displaying info about
	GameObject tooltip;

	// Use this for initialization
	void Start () {
		tooltip = GameObject.Find ("Tooltip");
		tooltip.SetActive(false); // Do not want it to pop up on game load
	}
	
	// Update is called once per frame
	void Update () {}

	public void activate(AdventureItem item)
	{
		this.item = item;
		constructDataStr ();
		tooltip.SetActive(true);

	}


	public void deactivate()
	{
		tooltip.SetActive (false);
	}



	public void constructDataStr()
	{
		// Change the tooltip text to match item data
		tooltip.transform.GetChild (0).GetComponent<Text> ().text = item.getDataStr ();
	}
}

