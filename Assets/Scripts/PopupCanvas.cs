using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PopupCanvas : MonoBehaviour {

    public static PopupCanvas myCanvas;

    private Inventory inv;
    private Synergy syn;
    private Equipment equip;
    private Player player;

    private List<List<int>> allUID;
    private List<List<int>> allItemID;

    // Use this for initialization
    void Start()
    {

        if (myCanvas == null)
        {

            inv = GameObject.Find("Inventory").GetComponent<Inventory>();
            syn = GameObject.Find("Synergy").GetComponent<Synergy>();
            equip = GameObject.Find("Equipment").GetComponent<Equipment>();
            player = GameObject.Find("player").GetComponent<Player>();


            allUID = new List<List<int>>();
            allItemID = new List<List<int>>();
            allUID.Add(inv.uids);
            allUID.Add(syn.uids);
            allUID.Add(equip.uids);

            List<int> invIDs = new List<int>();
            for (int i = 0; i < allUID[0].Count; i++)
            {
                invIDs.Add(inv.allItems[i].ID);
            }
            allItemID.Add(invIDs);

            List<int> synIDs = new List<int>();
            for (int i = 0; i < allUID[1].Count; i++)
            {
                synIDs.Add(syn.allItems[i].ID);
            }
            allItemID.Add(synIDs);

            List<int> equipIDs = new List<int>();
            for (int i = 0; i < allUID[2].Count; i++)
            {
                equipIDs.Add(equip.allItems[i].ID);
            }
            allItemID.Add(equipIDs);

            myCanvas = this;

        }
        else if (myCanvas != this)
        {
            Destroy(gameObject);
        }


    }

    // Update is called once per frame
    void Update () {
		
	}

    public void UpdateLists()
    {
        allUID = new List<List<int>>();
        allItemID = new List<List<int>>();
        allUID.Add(inv.uids);
        allUID.Add(syn.uids);
        allUID.Add(equip.uids);

        List<int> invIDs = new List<int>();
        for (int i = 0; i < allUID[0].Count; i++)
        {
            invIDs.Add(inv.allItems[i].ID);
        }
        allItemID.Add(invIDs);

        List<int> synIDs = new List<int>();
        for (int i = 0; i < allUID[1].Count; i++)
        {
            synIDs.Add(syn.allItems[i].ID);
        }
        allItemID.Add(synIDs);

        List<int> equipIDs = new List<int>();
        for (int i = 0; i < allUID[2].Count; i++)
        {
            equipIDs.Add(equip.allItems[i].ID);
        }
        allItemID.Add(equipIDs);


        printAllSlots();
    }

    public void printAllSlots()
    {
        for (int i = 0; i < allUID.Count; i++)
        {
            for (int j = 0; j < allUID[i].Count; j++)
            {
                string path = "Assets/Resources/test.txt";

                //Write some text to the test.txt file
                StreamWriter writer = new StreamWriter(path, true);
                writer.WriteLine("Slot: " + allUID[i][j] + " Item: " + allItemID[i][j]);
                writer.Close();

                Debug.Log("Slot: " + allUID[i][j] + " Item: " + allItemID[i][j]);
            }
        }
    }
}
