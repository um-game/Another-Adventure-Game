using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class statusCanvas : MonoBehaviour {

	public static statusCanvas myCanvas;

	// Use this for initialization
	void Start () {

		if (myCanvas == null) {
			myCanvas = this;
		} else if (myCanvas != this) {
			Destroy (gameObject);
		}

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
