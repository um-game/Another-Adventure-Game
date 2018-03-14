using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public worldItem item; // Type of item enemy will drop

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D collision) {
		this.gameObject.SetActive (false);
		Destroy (this.gameObject);
	}

    private void OnDestroy()
    {
        Instantiate(item, transform.position, item.transform.rotation);
        //item.transform.position = new Vector3 (this.gameObject.transform.position.x, this.gameObject.transform.position.y + 1, 0);
    }
}
