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

    void Awake()
    {
        if (Instance == null)
        {
            pickupMenu = GetComponent<PickupMenu>();
            inv = GameObject.Find("Inventory").GetComponent<Inventory>();
            myPlayer = GameObject.Find("player").GetComponent<Player>();
            itemDB = GetComponent<ItemDatabase>();

            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(myPlayer);
            DontDestroyOnLoad(inv);
            DontDestroyOnLoad(itemDB);
            DontDestroyOnLoad(GetComponent<ToolTip>());
            DontDestroyOnLoad(GetComponent<ItemMenu>());
            Instance = this;
        }
        else if (Instance != this)
        {
        }
    }
}
