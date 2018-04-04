using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalControl : MonoBehaviour
{
    public static GlobalControl Instance;

    public static Inventory inv;
    public static PickupMenu pickupMenu;
    public static Player myPlayer;
    public static ItemDatabase itemDB;

    void Awake()
    {
        if (Instance == null)
        {
            pickupMenu = GameObject.Find("Canvas").GetComponent<PickupMenu>();
            inv = GameObject.Find("Inventory").GetComponent<Inventory>();
            myPlayer = GameObject.Find("player").GetComponent<Player>();
            itemDB = GetComponent<ItemDatabase>();
            
            DontDestroyOnLoad(myPlayer);
            DontDestroyOnLoad(inv);
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
