using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalControl : MonoBehaviour
{
    public static GlobalControl Instance;

    public static Inventory inv;
    public static EquipMenu pickupMenu;
    public static Player myPlayer;
    public static ItemDatabase itemDB;
	public static Equipment equip;

    void Awake()
    {
        if (Instance == null)
        {
            pickupMenu = GameObject.Find("Canvas").GetComponent<EquipMenu>();
            inv = GameObject.Find("Inventory").GetComponent<Inventory>();
			equip = GameObject.Find("Equipment").GetComponent<Equipment>();
			myPlayer = GameObject.Find("player").GetComponent<Player>();
            itemDB = GetComponent<ItemDatabase>();

            
            DontDestroyOnLoad(myPlayer);
            DontDestroyOnLoad(inv);
			DontDestroyOnLoad (equip);
            /*
            DontDestroyOnLoad(itemDB);
            DontDestroyOnLoad(GetComponent<ToolTip>());
            DontDestroyOnLoad(GetComponent<ItemMenu>());
            */
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
