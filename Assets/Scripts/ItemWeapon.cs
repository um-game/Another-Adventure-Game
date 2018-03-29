using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWeapon : AdventureItem {

	// TODO: add defense for shield?
	public int Atk { get; set; }

	public ItemWeapon(int id, string title, int value, bool isStackable, string slug, int rarity, Sprite sprite, int atk) : base(id, title, value, isStackable, slug, rarity, sprite)  {
		this.Atk = atk;

		if(title.ToUpperInvariant().Contains("SHIELD")){
			this.itemType = ItemType.shield;
		} else {
			this.itemType = ItemType.weapon;
		}
	}

	public ItemWeapon() : base(){}

	public override string getDataStr()
	{
		return base.getDataStr () + "\nAttack: " + Atk;
	}

	public override string dbStr()
	{
		return base.dbStr() + " Atttack: " + Atk;
	}

	public override void use(Player player)
	{
		player.setWeapon (this);
	}
}
