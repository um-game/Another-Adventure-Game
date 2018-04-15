using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSynergy : AdventureItem {

	public ItemSynergy(int id, string title, int value, bool isStackable, string slug, int rarity, Sprite sprite) : base(id, title, value, isStackable, slug, rarity, sprite)  {

	}

	public ItemSynergy(): base() {
		//		ID = -1;
	}

	public override string getDataStr()
	{
		return base.getDataStr ();
	}

	public override string dbStr()
	{
		return base.dbStr ();
	}

	public override void use(Player player)
	{
		player.equipSynItem (this);
	}
}
