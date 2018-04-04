using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player: MonoBehaviour {

    public float maxSpeed;

    Animator anim;
	Rigidbody2D rb2d;
    Renderer rend;

	public Inventory inv;
	Equipment equip;
	bool isInvOpen; // Used to block input to player while inventory is open
	bool menuOpen;
	public PickupMenu pickupMenu;
	int attack;
	int defense;

    public static Player myPlayer;
    

	public int health { get; set; }

	ItemWeapon weapon;
	ItemWeapon shield;
	ItemArmor armor;

    // Use this for initialization
    void Start () {
       
        if (myPlayer == null)
        {
            anim = GetComponent<Animator>();
            rb2d = GetComponent<Rigidbody2D>();
            rend = GetComponent<Renderer>();
            anim.SetInteger("direction", 3);
            anim.SetBool("moving", false);

            pickupMenu = GetComponent<PickupMenu>();

			equip = GameObject.Find ("Equipment").GetComponent<Equipment>();
            inv = GameObject.Find("Inventory").GetComponent<Inventory>();
            isInvOpen = false; // Assume the inventory is closed upon loading
            menuOpen = false;

			health = 100; // Full health
			attack = 10; // Base attack
			defense = 10; // Base defense
			weapon = new ItemWeapon();
			armor = new ItemArmor ();

            // DontDestroyOnLoad(gameObject);
            myPlayer = this;
        }
        else if (myPlayer != this)
        {
            Destroy(gameObject);
        }
        
        

	}

	void Update() {
		// Toggle the inventory(if aother menu isnt already open
		if (!menuOpen && Input.GetKeyDown (KeyCode.I) ) {
			toggleInventory ();
            Debug.Log("OPEN INVENTORY");
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		// Block input while inentory is open
		if (menuOpen || isInvOpen) {
			return;
		}

        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        
        if (Input.GetKey(KeyCode.W)) {
            anim.SetInteger("direction", 1);
            anim.SetBool("moving", true);

           rb2d.velocity = new Vector2(0, moveY * maxSpeed);
        }
        else if (Input.GetKey(KeyCode.S)) {
            anim.SetInteger("direction", 3);
            anim.SetBool("moving", true);

           rb2d.velocity = new Vector2(0, moveY * maxSpeed);
        }
        else if (Input.GetKey(KeyCode.D)) {
            anim.SetInteger("direction", 2);
            anim.SetBool("moving", true);

           rb2d.velocity = new Vector2(moveX * maxSpeed, 0);
        }
        else if (Input.GetKey(KeyCode.A)) {
            anim.SetInteger("direction", 4);
            anim.SetBool("moving", true);

            rb2d.velocity = new Vector2(moveX * maxSpeed, 0);
        }
        
        else {
            anim.SetBool("moving", false);
            rb2d.velocity = new Vector2(0, 0);
        }   
    }

	// This method is fired whenever the Player's collider passes through an 'isTrigger' collider
	void OnTriggerEnter2D(Collider2D other){

		// Check if trigger is world item
		worldItem itemDat = other.gameObject.GetComponent<worldItem> ();

        // Debug.Log("Touched:" + other.gameObject.tag);


        if (itemDat != null) {
			// Stop player
			anim.SetBool("moving", false);
			rb2d.velocity = new Vector2(0, 0);
            
			pickupMenu.activate (itemDat.id, other.gameObject);
			menuOpen = true;
		}

        else if (other.gameObject.tag == "warp")
        {
            Warp myWarp = other.gameObject.GetComponent<Warp>();
            

            // Debug.Log("Going to level: " + index);
            Scene nextScene = SceneManager.GetSceneByBuildIndex(myWarp.dest);
            SceneManager.LoadScene(myWarp.dest);
            
            transform.position = new Vector3(myWarp.warpX, myWarp.warpY, 0);
            
            
        }
    }

    /*
     IEnumerator ChangeLevel(int index)
    {
        float fadeTime = GameObject.Find("EventSystem").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(index);
    }
    */
		
	private void toggleInventory () {
		inv.toggleActive ();
		equip.toggleActive ();
		isInvOpen = !isInvOpen;

		// Stop player
		anim.SetBool("moving", false);
		rb2d.velocity = new Vector2(0, 0);
	}

	public void addItemToInv(int id) {
		inv.addItem (id);
		menuOpen = false;
	}

	public void closeMenu() {
		menuOpen = false;
	}

	public void setWeapon(ItemWeapon newWeapon) {


		// If we did not have anything equipped and not shield, don't reduce attack
		if (this.weapon.ID != -1 && newWeapon.itemType != ItemType.shield) {
			attack -= this.weapon.Atk;
		}

		// Add new new weapon's attack
		attack += newWeapon.Atk;

		if (newWeapon.itemType == ItemType.weapon) {
			this.weapon = newWeapon;
		} else {
			this.shield = newWeapon;
		}

		// Put in equipment
		equip.addItem (newWeapon.ID);
	}

	public void setArmor(ItemArmor newArmor){
		if (this.armor.ID != -1) {
			this.defense -= this.armor.Def;
		}

		this.defense += newArmor.Def;

		this.armor = newArmor;

		// Put in equipment
		equip.addItem (newArmor.ID);
	}

	public void useItem(AdventureItem item) {

		// Can check type and act accordingly or create use function and pass player
		item.use (this);
		inv.removeItem (item);
	}

	public void printStats()
	{
		Debug.Log ("Health: " + health + "\nAttack: " + attack + "\nDefense: " + defense + "\nWeapon: " 
			+ weapon.Title + "\nArmor: " + armor.Title);
	}
}
