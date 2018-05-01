using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bladetrap : MonoBehaviour
{

    public float acceleration;
    public float deceleration;

    public int attack;
    public int health;

    private int triggerDelay;

    private Rigidbody2D rb2d;
    private Vector2 input;

    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.drag = deceleration;
        rb2d.gravityScale = 0;

        if (acceleration == 0)
        {
            rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    void Update()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb2d.AddForce(-1 * input * acceleration * Time.deltaTime, ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Player myPlayer = collision.gameObject.GetComponent<Player>();
        triggerDelay = 20; // after x frames trigger this again if still colliding

        if (myPlayer != null)
        {
            int damage = myPlayer.defense - attack;
            if (damage > 0)
                damage = 0;
            myPlayer.health += damage;
            Debug.Log("Bladetrap HIT PLAYER: " + myPlayer.health);
            if (myPlayer.health == 0)
                Debug.Log("PLAYER DIED!");
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (triggerDelay <= 0)
            OnTriggerEnter2D(collision);
        else
            triggerDelay -= 1;
    }

}