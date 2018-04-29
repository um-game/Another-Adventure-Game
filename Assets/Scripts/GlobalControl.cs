﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalControl : MonoBehaviour
{
    public static GlobalControl Instance;

    public static Inventory inv;
	public static Synergy syn;
	public static PickupMenu pickupMenu;
    public static Player myPlayer;
    public static ItemDatabase itemDB;
	public static Equipment equip;
	public static EquipMenu equipMenu;
    public static GameObject statusCanvas;

    void Awake()
    {
        if (Instance == null)
        {
			pickupMenu = GameObject.Find("PopupCanvas").GetComponent<PickupMenu>();
			equipMenu = GameObject.Find("PopupCanvas").GetComponent<EquipMenu>();
            statusCanvas = GameObject.Find("StatusCanvas");

            inv = GameObject.Find("Inventory").GetComponent<Inventory>();
			syn = GameObject.Find("Synergy").GetComponent<Synergy>();
			equip = GameObject.Find("Equipment").GetComponent<Equipment>();
			myPlayer = GameObject.Find("player").GetComponent<Player>();
            itemDB = GetComponent<ItemDatabase>();

            DontDestroyOnLoad(myPlayer);
            DontDestroyOnLoad(inv);
			DontDestroyOnLoad (equip);
			DontDestroyOnLoad(syn);

            /*
            DontDestroyOnLoad(itemDB);
            DontDestroyOnLoad(GetComponent<ToolTip>());
            DontDestroyOnLoad(GetComponent<ItemMenu>());
            */
            DontDestroyOnLoad(statusCanvas);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(statusCanvas);
            Destroy(gameObject);
        }
    }
}