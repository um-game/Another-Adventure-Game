﻿
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;

public class ItemDatabase : MonoBehaviour {
	
	private List<AdventureItem> itemList; // List of all items
	private Dictionary<string, Sprite> sprites;
	private JsonData jsonItemData;

	// Use this for initialization
	public void Start () {
		jsonItemData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json")); // Read in JSON data
		loadSpriteDict ("Sprites/Items/roguelikeitems"); // Load item spritesheet
		createDatabase (); 

		//printDB (); // used to debug DB
	}

	// Update is called once per frame
	void Update () {}

	// This method w
	void loadSpriteDict(string sheetName) {
		Sprite[] spritesData = Resources.LoadAll<Sprite> (sheetName);
		sprites = new Dictionary<string, Sprite> ();

		for (int i = 0; i < spritesData.Length; i++) 
		{
			sprites.Add (spritesData [i].name, spritesData [i]);
		}
	}

	Sprite getSpriteByName(string name) {
		if(sprites.ContainsKey(name)){
			return sprites[name];
		}
		return null;
	}

	void createDatabase() {
		itemList = new List<AdventureItem>();

		// Read in all the armor
		foreach(JsonData armor in jsonItemData["armor"]) {
			itemList.Add (new ItemArmor ((int)armor["id"], armor["title"].ToString(), (int)armor["value"],  (bool)armor["stackable"], armor["slug"].ToString(), (int)armor["rarity"], getSpriteByName(armor["slug"].ToString()), (int)armor["def"]));
		}

		// Read in all the weapons
		foreach(JsonData weapon in jsonItemData["weapon"]) {
			itemList.Add (new ItemWeapon ((int)weapon["id"], weapon["title"].ToString(), (int)weapon["value"],  (bool)weapon["stackable"], weapon["slug"].ToString(), (int)weapon["rarity"], getSpriteByName(weapon["slug"].ToString()), (int)weapon["atk"]));
		}

		// Read in all the consumables
		foreach (JsonData consumable in jsonItemData["consumable"]) {
			itemList.Add (new ItemConsumable ((int)consumable ["id"], consumable ["title"].ToString (), (int)consumable ["value"], (bool)consumable ["stackable"], consumable ["slug"].ToString (), (int)consumable ["rarity"], getSpriteByName (consumable ["slug"].ToString ()), (int)consumable ["hp"]));
		}

		// Read in all the synergy items
		foreach (JsonData synergy in jsonItemData["synergy"]) {
			itemList.Add (new ItemSynergy ((int)synergy ["id"], synergy ["title"].ToString (), (int)synergy["value"], (bool)synergy["stackable"], synergy ["slug"].ToString (), (int)synergy["rarity"], getSpriteByName (synergy["slug"].ToString())));
		}
	}

	public AdventureItem getItem(int id) {

		// Check if item is at given index
		if (itemList[id].ID == id) {
			return itemList[id];
		}

		// If not, loop through it until we find it
		for (int i = 0; i < itemList.Count; i++) {
			if (itemList[i].ID == id) {
				return itemList[i];
			}
		}
		return new AdventureItem (); // Return item w/ bad ID if we didn't find anything
	}

	private void printDB()
	{
		for (int i = 0; i < itemList.Count; i++) {
			Debug.Log (itemList[i].dbStr());
		}
	}

	public List<AdventureItem> getItemByRarity(int rarity) {

		List<AdventureItem> items = new List<AdventureItem> ();

		// Build the list of items with given rarity
		foreach(AdventureItem item in itemList){

			if (item.Rarity == rarity) {
				items.Add (item);
			}
		}
		return items;
	}
}


