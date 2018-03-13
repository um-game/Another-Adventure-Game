using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemConsumable : AdventureItem {

	public int HpRestored;

	public ItemConsumable(int id, string title, int value, bool isStackable, string slug, int rarity, Sprite sprite, int hp) : base(id, title, value, isStackable, slug, rarity, sprite)  {
		this.HpRestored = hp;
	}

	public ItemConsumable() : base(){}

	public override string getDataStr()
	{
		return base.getDataStr () + "\nHP restored: " + HpRestored;
	}

	public override string dbStr()
	{
		return base.dbStr() + " Hp Restored: " + HpRestored;
	}
}
