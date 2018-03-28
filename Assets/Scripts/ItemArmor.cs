using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemArmor : AdventureItem {

	public int Def { get; set; }

	public string Loc { get; set; }  // String representation of where the item may be worn

	public ItemArmor(int id, string title, int value, bool isStackable, string slug, int rarity, Sprite sprite, int def) : base(id, title, value, isStackable, slug, rarity, sprite)  {
		this.Def = def;

		if(title.Contains("chest")) {
			this.itemType = ItemType.chest;
		} else {
			this.itemType = ItemType.head;
		}
	}

	public ItemArmor() : base(){}

	public override string getDataStr()
	{
		return base.getDataStr () + "\nDefense: " + Def;
	}

	public override string dbStr()
	{
		return base.dbStr() + " Defense: " + Def;
	}

	public override void use(Player player)
	{
		player.setArmor (this);
	}
}
