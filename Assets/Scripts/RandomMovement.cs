using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour {

    public float acceleration;
    public float deceleration;

    private Rigidbody2D rb2d;
    private Vector2 input;

    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.drag = deceleration;
    }

    void Update()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
    }
	
	// Update is called once per frame
	void FixedUpdate() {
	    rb2d.AddForce(-1 * input * acceleration * Time.deltaTime, ForceMode2D.Impulse);
    }
}
