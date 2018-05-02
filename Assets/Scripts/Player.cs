using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player: MonoBehaviour {

    public float maxSpeed;

	private float buffSpeed;
	private float baseSpeed;

	private bool attackBuff;
    private bool speedBuff;
    private bool swimming;

    Animator anim;
	Rigidbody2D rb2d;
    Renderer rend;

	public Inventory inv;
	public Equipment equipment;
	public PickupMenu pickupMenu;
	public Synergy syn;
	private StatsPanel statsPanel;
    private ButtonManager myManager;

	public int attack;
    public bool isAttacking = false;
	int baseAttack;
	public int defense;
    int staminaCost;
    int staminaRecover;
    int healthRecover;
    int tickHealthDelay;
    int tickStaminaDelay;
    int baseHealthTick;
    int baseStaminaTick;

    public static Player myPlayer;

    public int health;
    public int stamina;

	ItemWeapon weapon;
	ItemWeapon shield;
	ItemArmor chestArmor;
	ItemArmor headArmor;

	public static int UID = 0;

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
            myManager = GameObject.Find("ButtonManager").GetComponent<ButtonManager>();

			equipment = GameObject.Find ("Equipment").GetComponent<Equipment>();
            inv = GameObject.Find("Inventory").GetComponent<Inventory>();
            syn = GameObject.Find ("Synergy").GetComponent<Synergy> ();
            syn.Start();
			statsPanel = GameObject.Find ("statsPanel").GetComponent<StatsPanel> ();
			statsPanel.toggleActive ();

			baseAttack = 10;

            if (!myManager.loading)
            {
                health = 100; // Full health
                stamina = 100; // Full stamina
                attack = baseAttack; // Base attack
                defense = 10; // Base defense
            }
			
            staminaCost = 5; // Base stamina cost for melee punch
            staminaRecover = 5;
            healthRecover = 1;
            baseHealthTick = 200; // Base tick delay for health/stam recovery
            baseStaminaTick = 20;
            tickHealthDelay = baseHealthTick;
            tickStaminaDelay = baseStaminaTick;

            swimming = false;
            
			baseSpeed = maxSpeed;
			buffSpeed = maxSpeed * 2.0f;
			attackBuff = false;

			weapon = new ItemWeapon();
			shield = new ItemWeapon ();
			chestArmor = new ItemArmor ();
			headArmor = new ItemArmor ();

            loadRemoveBuff();
            //checkBuff();

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
		if (Input.GetKeyDown (KeyCode.I) || Input.GetKeyDown(KeyCode.PageDown)) {
			toggleInventory ();
            Debug.Log("OPEN INVENTORY");
		}

		if (Input.GetMouseButtonDown(0) && !GetComponent<EquipMenu>().isActiveAndEnabled) {
			GetComponent<EquipMenu> ().deactivate ();
			inv.GetComponent<ItemMenu> ().deactivate ();
		}
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{

//	    buttons:
//	    A = Return
//	    B = Space
//	    X = PageUp
//	    Y = PageDown
//
//
//	    Left bumper = Left Ctrl
//	    Right bumper = Left Alt
//	    Left trigger = Mouse button 1(no axis)
//	    Right trigger = Mouse button 0(no axis)
//	    Left flipper = Mouse button 3
//	    Right flipper = Mouse button 4
//
//
//	    pad button left = Mouse button 2
//	    pad button right = Left shift
//	        arrow next to steam button left = Tab
//	    arrow next to steam button right = Escape
//
//
//	    Axes:
//	    Joystick = acts as Horizontal / Vertical by sending(Left, Right, Up, Down arrow keys)
//	    pad left = acts as Mouse ScrollWheel axis
//	    pad right = acts as Mouse X and Mouse Y

        Debug.Log(Input.GetJoystickNames());
        if (Input.GetKeyDown(KeyCode.Return))
	    {
	        Debug.Log("test");
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        if (tickHealthDelay <= 0)
        {
            health += healthRecover;
            if (health > 100)
                health = 100;
            tickHealthDelay = baseHealthTick;
        }
        else
        {
            tickHealthDelay -= 1;
        }

        if (tickStaminaDelay <= 0)
        {
            stamina += staminaRecover;
            if (stamina > 100)
                stamina = 100;
            tickStaminaDelay = baseStaminaTick;
        }
        else
        {
            tickStaminaDelay -= 1;
        }

        if (anim != null)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (!swimming)
                    attackAction();
            }
            else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A)
                || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)
                || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)
                || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            {
                anim.SetBool("attacking", false);
                anim.SetBool("moving", true);
            }
            else
            {
                anim.SetBool("moving", false);
                rb2d.velocity = new Vector2(0, 0);
            }


            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                anim.SetInteger("direction", 1);

                rb2d.velocity = new Vector2(0, moveY * maxSpeed);
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                anim.SetInteger("direction", 3);

                rb2d.velocity = new Vector2(0, moveY * maxSpeed);
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                anim.SetInteger("direction", 2);

                rb2d.velocity = new Vector2(moveX * maxSpeed, 0);
            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                anim.SetInteger("direction", 4);

                rb2d.velocity = new Vector2(moveX * maxSpeed, 0);
			} else if(Input.GetKey(KeyCode.P)) {
				inv.printUID();
				syn.printUID ();
				equipment.printUID ();
			}
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
            // Where the warp takes you is set in the Warp component
            // with the id that can be obtained via File > Build Settings
            Warp myWarp = other.gameObject.GetComponent<Warp>();

            StartCoroutine(ChangeLevel(myWarp.dest, myWarp.warpX, myWarp.warpY, 0));
        }

        else if (other.gameObject.tag == "water")
        {
            swimming = true;

            anim.SetBool("swimming", swimming);
        }


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "water")
        {
            swimming = false;

            anim.SetBool("swimming", swimming);
        }
    }



    IEnumerator ChangeLevel(int index, float warpX, float warpY, float warpZ)
    {
        float fadeTime = GameObject.Find("EventSystem").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        transform.position = new Vector3(warpX, warpY, warpZ);
        SceneManager.LoadScene(index);
    }
		
	private void toggleInventory () {
		inv.toggleActive ();
		equipment.toggleActive ();
		syn.toggleActive ();
		statsPanel.toggleActive ();
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
        staminaCost = 10; // TODO: weapon stam cost

		if (newWeapon.itemType == ItemType.weapon) {
			this.weapon = newWeapon;

		} else {
			this.shield = newWeapon;
		}

		if (newWeapon.equipped == false) {

			// Put in equipment
			equipment.addItem (newWeapon.ID);
		}
	}

	public void setArmor(ItemArmor newArmor){

		// Check if there is something equipped on the chest
		if (this.chestArmor.ID != -1 && newArmor.itemType == ItemType.chest) {
			this.defense -= this.chestArmor.Def;
		} else if (this.headArmor.ID != -1 && newArmor.itemType == ItemType.head) {
			this.defense -= this.headArmor.Def;
		}

		this.defense += newArmor.Def;

		// Determine weather head or chest
		if (newArmor.itemType == ItemType.chest) {

			this.chestArmor = newArmor;
		} else {
			this.headArmor = newArmor;
		}

		if (newArmor.equipped == false) {

			// Put in equipment
			equipment.addItem (newArmor.ID);
		}
	}

	public void useItem(AdventureItem item, int slotUID) {

		// Can check type and act accordingly or create use function and pass player
		item.use (this);
		inv.removeItem (item, slotUID);
		statsPanel.updateStats ();
		printStats ();
	}

	public void printStats()
	{
		Debug.Log ("Attack: " + attack + " Defense: " + defense + " Speed: " + maxSpeed
            + " Health: " + health + " Stamina: " + stamina);
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

//		item.equipped = false;

		if(item.GetType() == typeof(ItemSynergy)) {
			item.equipped = false;
			syn.removeItem (item);
			checkBuff ();

		} else {

			if (item.equipped) {
				item.equipped = false;
				equipment.removeItem (item);
			}
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
		statsPanel.updateStats ();
		printStats ();
	}

	public void equipSynItem(ItemSynergy synItem) {
		syn.addItem (synItem.ID);
		checkBuff();
	}

	public void checkBuff() {

		List<AdventureItem> allSyn = syn.allItems;

		Dictionary<string, int> contains = new Dictionary<string, int> () { { "red", 0 },
																			{ "green", 0 },
																			{ "blue", 0 },
																			{ "purple", 0 },
																			{ "white", 0 } };

		// Figure out what items are present
		foreach (AdventureItem item in allSyn) {

			switch (item.Slug) 
			{
			case "red_synergy":
				contains ["red"] += 1;
				break;

			case "green_synergy":
				contains ["green"] += 1;
				break;

			case "blue_synergy":
				contains ["blue"] += 1;
				break;

			case "purple_synergy":
				contains ["purple"] += 1;
				break;

			case "white_synergy":
				contains ["white"] += 1;
				break;
			}
		}

		// Turn buffs on/off
		if (contains ["red"] == 1 && contains ["blue"] == 1 && speedBuff == false) {
            speedBuff = true;
            maxSpeed *= 2;
			Debug.Log ("RB BUFF SPEED");
		} else if ((contains["red"] != 1 || contains["blue"] != 1) && speedBuff) {
            speedBuff = false;
            maxSpeed /= 2;
			Debug.Log ("RB UNBUFF SPEED");
		}

		// Just turn buff on ' IE gain attack if on
		if (contains ["green"] == 1 && contains ["purple"] == 1 && attackBuff == false) {
			attackBuff = true;
			Debug.Log ("ATK BUFF");

			attack += 20;

		} else if ((contains ["green"] != 1 || contains ["purple"] != 1) && attackBuff) {
			attackBuff = false;
			attack -= 20; 
			Debug.Log ("ATK DEBUFF");
		}

        statsPanel.updateStats();
	}

    public void attackAction()
    {
        int potentialState = stamina - staminaCost;

        if (potentialState >= 0)
        {
            anim.SetTrigger("attacking");
            stamina -= staminaCost;
        }
    }

    public void fireProjectile()
    {
        return;
    }

	public bool isInvFull() {
		return inv.isFull ();
	}

    public void loadRemoveBuff()
    {
        List<AdventureItem> allSyn = syn.allItems;

        Dictionary<string, int> contains = new Dictionary<string, int>() { { "red", 0 },
                                                                            { "green", 0 },
                                                                            { "blue", 0 },
                                                                            { "purple", 0 },
                                                                            { "white", 0 } };

        // Figure out what items are present
        foreach (AdventureItem item in allSyn)
        {

            switch (item.Slug)
            {
                case "red_synergy":
                    contains["red"] += 1;
                    break;

                case "green_synergy":
                    contains["green"] += 1;
                    break;

                case "blue_synergy":
                    contains["blue"] += 1;
                    break;

                case "purple_synergy":
                    contains["purple"] += 1;
                    break;

                case "white_synergy":
                    contains["white"] += 1;
                    break;
            }
        }

        if (contains["green"] == 1 && contains["purple"] == 1 && attackBuff == false)
        {
            attackBuff = true;
            Debug.Log("ATK BUFF");
        }

        if (contains["red"] == 1 && contains["blue"] == 1 && speedBuff == false)
        {
            speedBuff = true;
            Debug.Log("RB BUFF SPEED");
        }
    }
}
