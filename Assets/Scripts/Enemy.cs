using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: put common fucntionallity of enemy and payer into parent class
public class Enemy : MonoBehaviour
{


    Animator anim;
    public worldItem item;      // Type of item enemy will drop
    bool dying = false;         // If the enemy has died or not
    Rigidbody2D RigidBodyCat;   // Cat Fighter's Rigid Body
    int maxSpeed = 1;           // Speed of Enemy
    int range = 2;              // If player is within range, enemy will attack
    int attRange = 1;
    Transform target;           // Hold transform of player, used to calculate movement direction
    int facing = 0;

    Vector3 currentPosition, lastPosition;  // Used for sprite flipping



    // Use this for initialization
    void Start()
    {
        RigidBodyCat = GetComponent<Rigidbody2D>();
        target = GameObject.Find("player").transform;

        currentPosition = transform.position;
        lastPosition = currentPosition;

        anim = GetComponent<Animator>();



    }

    // Update is called once per frame
    void Update()
    {
        // FIXME: enemy gets stuck
        Vector3 diff = target.transform.position - this.transform.position; // Get difference in position
        anim.SetInteger("attRange", (int)diff.magnitude);

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
            anim.SetInteger("facing", 1);
            

        }
        if (currentPosition.x < lastPosition.x)
        {
            anim.SetInteger("facing", 3);
        }

        var relativePoint = transform.InverseTransformPoint(transform.position);

       
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