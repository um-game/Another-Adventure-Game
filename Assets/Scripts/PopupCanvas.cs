using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class PopupCanvas : MonoBehaviour {

    public static PopupCanvas myCanvas;

    private Inventory inv;
    private Synergy syn;
    private Equipment equip;

    private List<List<int>> allUID;
    private List<List<int>> allItemID;
    private ButtonManager myManager;

    private Player myPlayer;

    // Use this for initialization
    void Start()
    {

        if (myCanvas == null)
        {

            inv = GameObject.Find("Inventory").GetComponent<Inventory>();
            inv.Start();
            syn = GameObject.Find("Synergy").GetComponent<Synergy>();
            equip = GameObject.Find("Equipment").GetComponent<Equipment>();
            myPlayer = GameObject.Find("player").GetComponent<Player>();
            
            myManager = GameObject.Find("ButtonManager").GetComponent<ButtonManager>();


            if (myManager.loading)
            {
                string path = myManager.savePath;

                //Read the text from directly from the savefile
                StreamReader reader = new StreamReader(path);

                while (!reader.EndOfStream)
                {
                    string nextSlot = reader.ReadLine();
                    string[] data = nextSlot.Split(' ');

                    if (data[0] == "Slot:")
                    {
                        int slotID = Int32.Parse(data[1]);
                        int itemID = Int32.Parse(data[3]);

                        if (slotID >= inv.uids[0] && slotID <= inv.uids[inv.uids.Count - 1])
                        {
                            inv.loadItem(itemID, slotID);
                        }
                        else if (slotID >= syn.uids[0] && slotID <= syn.uids[syn.uids.Count - 1])
                        {
                            syn.loadItem(itemID, slotID);
                        }
                        else if (slotID >= equip.uids[0] && slotID <= equip.uids[equip.uids.Count - 1])
                        {
                            equip.loadItem(itemID, slotID);
                        }
                    }
                    
                    else if (data[0] == "Stats")
                    {
                        Debug.Log(nextSlot);
                        myPlayer.attack = Int32.Parse(data[2]);
                        myPlayer.defense = Int32.Parse(data[4]);
                        myPlayer.maxSpeed = float.Parse(data[6], CultureInfo.InvariantCulture);
                        myPlayer.health = Int32.Parse(data[8]);
                        myPlayer.stamina = Int32.Parse(data[10]);
                    }
                }

                reader.Close();
            }
            

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
        // Clear old save
        string path = myManager.savePath;
        StreamWriter wiper = new StreamWriter(path, false);
        wiper.Close();

        for (int i = 0; i < allUID.Count; i++)
        {
            for (int j = 0; j < allUID[i].Count; j++)
            {

                //Write some text to the test.txt file
                StreamWriter writer = new StreamWriter(path, true);
                writer.WriteLine("Slot: " + allUID[i][j] + " Item: " + allItemID[i][j]);
                writer.Close();

                Debug.Log("Slot: " + allUID[i][j] + " Item: " + allItemID[i][j]);
            }
        }

        StreamWriter writer2 = new StreamWriter(path, true);
        writer2.WriteLine("Stats Attack: " + myPlayer.attack + " Defense: " + myPlayer.defense + " Speed: " + myPlayer.maxSpeed
            + " Health: " + myPlayer.health + " Stamina: " + myPlayer.stamina);
        writer2.Close();

    }
}
