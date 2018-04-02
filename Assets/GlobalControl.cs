using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalControl : MonoBehaviour
{
    public static GlobalControl Instance;

    public Inventory inv;
    public PickupMenu pickupMenu;

    void Awake()
    {
        if (Instance == null)
        {
            pickupMenu = GetComponent<PickupMenu>();
            inv = GameObject.Find("Inventory").GetComponent<Inventory>();

            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
