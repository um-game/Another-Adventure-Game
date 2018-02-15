using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float maxSpeed = 10f;

    Animator anim;
    

    // Use this for initialization
    void Start () {

        anim = GetComponent<Animator>();
        anim.SetInteger("direction", 3);
        anim.SetBool("moving", false);
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        
        if (Input.GetKeyDown(KeyCode.W)) {
            anim.SetInteger("direction", 1);
            anim.SetBool("moving", true);

            GetComponent<Rigidbody2D>().velocity = new Vector2(0, moveY * maxSpeed);
        }
        else if (Input.GetKeyDown(KeyCode.S)) {
            anim.SetInteger("direction", 3);
            anim.SetBool("moving", true);

            GetComponent<Rigidbody2D>().velocity = new Vector2(0, moveY * maxSpeed);
        }
        else if (Input.GetKeyDown(KeyCode.D)) {
            anim.SetInteger("direction", 2);
            anim.SetBool("moving", true);

            GetComponent<Rigidbody2D>().velocity = new Vector2(moveX * maxSpeed, 0);
        }
        else if (Input.GetKeyDown(KeyCode.A)) {
            anim.SetInteger("direction", 4);
            anim.SetBool("moving", true);

            GetComponent<Rigidbody2D>().velocity = new Vector2(moveX * maxSpeed, 0);
        }
        
        if (!Input.GetKeyDown(KeyCode.A) && !Input.GetKeyDown(KeyCode.S) && !Input.GetKeyDown(KeyCode.D) && !Input.GetKeyDown(KeyCode.W)) {
            anim.SetBool("moving", false);
        }
        
    }
}
