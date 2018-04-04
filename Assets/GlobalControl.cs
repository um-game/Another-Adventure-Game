using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalControl : MonoBehaviour
{
    public static GlobalControl Instance;

    public Inventory inv;
    public PickupMenu pickupMenu;
    public Player myPlayer;
    public ItemDatabase itemDB;
	public Canvas canvas;

    void Awake()
    {
        if (Instance == null)
        {
            pickupMenu = GetComponent<PickupMenu>();
            inv = GameObject.Find("Inventory").GetComponent<Inventory>();
            myPlayer = GameObject.Find("player").GetComponent<Player>();
            itemDB = GetComponent<ItemDatabase>();
			//canvas = GameObject.Find ("Canvas").GetComponent<Canvas>();

			//DontDestroyOnLoad(GameObject.Find("Canvas"));
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(myPlayer);
            DontDestroyOnLoad(inv);
            DontDestroyOnLoad(itemDB);
            DontDestroyOnLoad(GetComponent<ToolTip>());
            DontDestroyOnLoad(GetComponent<ItemMenu>());
			DontDestroyOnLoad (pickupMenu);
            Instance = this;
        }
        else if (Instance != this)
        {
        }
    }
}
