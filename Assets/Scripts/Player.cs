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
	public Equipment equipment;
	public Synergy syn;
	public PickupMenu pickupMenu;
	int attack;
	int defense;

    public static Player myPlayer;
    
	public int health { get; set; }

	ItemWeapon weapon;
	ItemWeapon shield;
	ItemArmor chestArmor;
	ItemArmor headArmor;

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

			equipment = GameObject.Find ("Equipment").GetComponent<Equipment>();
            inv = GameObject.Find("Inventory").GetComponent<Inventory>();
			syn = GameObject.Find ("Synergy").GetComponent<Synergy> ();

			health = 100; // Full health
			attack = 10; // Base attack
			defense = 10; // Base defense

			weapon = new ItemWeapon();
			shield = new ItemWeapon ();
			chestArmor = new ItemArmor ();
			headArmor = new ItemArmor ();

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
		if (Input.GetKeyDown (KeyCode.I)) {
			toggleInventory ();
            Debug.Log("OPEN INVENTORY");
		}

		if (Input.GetMouseButtonDown(0) && !GetComponent<EquipMenu>().isActiveAndEnabled) {
			GetComponent<EquipMenu> ().deactivate ();
			inv.GetComponent<ItemMenu> ().deactivate ();
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {

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

            Pause();
            
			pickupMenu.activate (itemDat.id, other.gameObject);
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
		equipment.toggleActive ();
		syn.toggleActive ();
		PauseGameFeature ();
		// Stop player
		anim.SetBool("moving", false);
		rb2d.velocity = new Vector2(0, 0);
	}

	public void addItemToInv(int id) {
		inv.addItem (id);
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
		equipment.addItem (newWeapon.ID);
	}

	public void setArmor(ItemArmor newArmor){

		// Check if there is something equipped
		if (this.chestArmor.ID != -1) {
			this.defense -= this.chestArmor.Def;
		}

		this.defense += newArmor.Def;

		// Determine weather head or chest
		if (newArmor.itemType == ItemType.chest) {

			this.chestArmor = newArmor;
		} else {
			this.headArmor = newArmor;
		}


		// Put in equipment
		equipment.addItem (newArmor.ID);
	}

	public void useItem(AdventureItem item) {

		// Can check type and act accordingly or create use function and pass player
		item.use (this);
		inv.removeItem (item);
		printStats ();
	}

	public void printStats()
	{
		Debug.Log ("Health: " + health + "\nAttack: " + attack + "\nDefense: " + defense + "\nWeapon: " 
			+ weapon.Title + "\nArmor: " + chestArmor.Title);
	}
		
	// Pauses / unpauses the game by essentially 'stopping time'
	public void   PauseGameFeature()
	{
		// If not paused, pause
		if(Time.timeScale == 1)
		{
			Pause();
		}
		// Else unpause
		else
		{
			UnPause();
		}   
	}

	public void Pause()
	{
		Time.timeScale = 0;
		Debug.Log ("PAUSE");

	}
	public void UnPause()
	{
		Time.timeScale = 1;
		Debug.Log("UNPAUSE");
	}

	public void unEquip(AdventureItem item) {

		item.equipped = false;

		if(item.GetType() == typeof(ItemSynergy)) {

			syn.removeItem (item);

		} else {

			equipment.removeItem (item);

	//		Debug.Log ("Unequipping: " + item.itemType);

			// Un-equipping sword
			if (item.itemType == ItemType.weapon) {
				ItemWeapon weapon = (ItemWeapon)item;
				attack -= weapon.Atk;
				this.weapon = new ItemWeapon (); // Set to bad ID

			// Un-equipping shield
			} else if (item.itemType == ItemType.shield) {
				this.shield = new ItemWeapon();

			// Un-equipping chest piece
			} else if (item.itemType == ItemType.chest) {
				ItemArmor armor = (ItemArmor)item;
				defense -= armor.Def;
				this.chestArmor = new ItemArmor();

			// Un-equipping head piece
			} else if (item.itemType == ItemType.head) {
				ItemArmor armor = (ItemArmor)item;
				defense -= armor.Def;
				this.headArmor = new ItemArmor();
			}
		}
//		equipment.printEquipment();

		printStats ();
	}

	public void equipSynItem(ItemSynergy synItem) {
		syn.addItem (synItem.ID);
	}
}
