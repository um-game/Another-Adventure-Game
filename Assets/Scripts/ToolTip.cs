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
	
	public void activate(AdventureItem item)
	{
		this.item = item;
		constructDataStr ();
		tooltip.transform.position = Input.mousePosition + new Vector3 (10.0f, 60.0f, 0.0f);
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

