using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canv : MonoBehaviour {

	public static canv canvas = null;

	public void Awake(){

		if (canvas == null) {
			canvas = this;
		}  else if (canvas != this) {
			Destroy (gameObject);
		}
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
}

