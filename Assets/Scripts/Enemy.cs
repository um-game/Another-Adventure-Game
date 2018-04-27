using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: put common fucntionallity of enemy and payer into parent class
public class Enemy : MonoBehaviour
{

    
    public worldItem item;      // Type of item enemy will drop
    bool dying = false;         // If the enemy has died or not
    Rigidbody2D RigidBodyCat;   // Cat Fighter's Rigid Body
    int maxSpeed = 1;           // Speed of Enemy
    int range = 2;              // If player is within range, enemy will attack
    Transform target;           // Hold transform of player, used to calculate movement direction

    Vector3 currentPosition, lastPosition;  // Used for sprite flipping



    // Use this for initialization
    void Start()
    {
        RigidBodyCat = GetComponent<Rigidbody2D>();
        target = GameObject.Find("player").transform;

        currentPosition = transform.position;
        lastPosition = currentPosition;

        
        
    }

    // Update is called once per frame
    void Update()
    {
        // FIXME: enemy gets stuck
        Vector3 diff = target.transform.position - this.transform.position; // Get difference in position

        if (diff.magnitude < range)
        {
            diff.Normalize();

            // Move towards player
            RigidBodyCat.velocity = new Vector2(diff.x * maxSpeed, diff.y * maxSpeed);

        }
        else
        {
            RigidBodyCat.velocity = new Vector2(0, 0); // Otherwse we stop moving
        }

        if (currentPosition.x > lastPosition.x)
        {

            //We are moving to the right
            //set sprite to rightFacingSprite
        }
        if (currentPosition.x < lastPosition.x)
        {
            //We are moving to the left
            //set sprite to leftFacingSprite
        }
        var relativePoint = transform.InverseTransformPoint(transform.position);

        if (relativePoint.x < 0.0)
            print("Object is to the left");
        else if (relativePoint.x > 0.0)
            print("Object is to the right");
        else
            print("Object is directly ahead");

        //Update the new positions
        lastPosition = currentPosition;
        currentPosition = transform.position;




    }



    void OnCollisionEnter2D(Collision2D collision)
    {
       

        Player p = collision.gameObject.GetComponent<Player>();

        if (p != null)
        {
            this.gameObject.SetActive(false);
            dying = true;
            Destroy(this.gameObject);
        }
    }


    private void OnDestroy()
    {
        if (dying)
            Instantiate(item, transform.position, item.transform.rotation);
    }

 
}