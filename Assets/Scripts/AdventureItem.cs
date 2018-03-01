using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Regular old classes can be used at will throughout the project as long as they are located within Unity's Assets folder


// This class will be the base class for all items.
public class AdventureItem 
{
	public int ID { get; set; }

	public string Title { get; set; } // String representation of the item(title was used instead of 'name' to avoid namespace conflicts).

	public int Value { get; set; }

	public bool IsStackable { get; set; }

	public int Rarity { get; set; }

	// This is used to get the sprite associiated with this particular item
	// As such, it should match the filename of the sprite(
	// e.g. if the sprite is named 'steel_ring.png' then Slug should be 'steel_ring'
	// Since the slug represents a filename, spaces ARE NOT ALLOWED
	public string Slug { get; set; }

	public Sprite Sprite { get; set; }

	public AdventureItem(int id, string title, int value, bool isStackable, string slug, int rarity, Sprite sprite) {
		this.ID = id;
		this.Title = title;
		this.Value = value;
		this.Slug = slug;
		this.IsStackable = isStackable;
		this.Rarity = rarity;
		this.Sprite = sprite;
	}

	// We may use this to represent an 'empty' item
	public AdventureItem() 
	{
		ID = -1;
	}

	virtual public string getDataStr() {

		return "Name: " + Title + "\nValue: " + Value + "\nRarity: " + Rarity; 

	}

	virtual public string dbStr() {
		return "Name: " + Title + " Value: " + Value + " Rarity: " + Rarity + " sprite name: " + this.Sprite.name;
	}
}