using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class statusCanvas : MonoBehaviour {

	public static statusCanvas myCanvas;
    public GameObject healthBar, stamBar;

	// Use this for initialization
	void Start () {

		if (myCanvas == null) {
			myCanvas = this;
            healthBar = GameObject.Find("healthBar");
            stamBar = GameObject.Find("stamBar");
		} else if (myCanvas != this) {
			Destroy (gameObject);
		}

		
	}
	
	// Update is called once per frame
	void Update () {
        Player myPlayer = GameObject.Find("player").GetComponent<Player>();
        healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(myPlayer.health * 2, 39);
        stamBar.GetComponent<RectTransform>().sizeDelta = new Vector2(myPlayer.stamina * 2, 39);
    }
}
