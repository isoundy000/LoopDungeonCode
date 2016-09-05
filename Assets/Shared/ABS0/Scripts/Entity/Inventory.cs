using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Inventory {

	public Item[] Items;

	public Inventory() {
		
	}

	public int ItemsCount {
		get {
			return Items.Length;
		}
	}


	public int AddItem(Item item) {
		int index = -1;
		for (int i = 0; i < ItemsCount; i++) {
			if (Items [i] == null || Items [i].Type == EntityType.NONE) {
				Items [i] = item;
				index = i;
				break;
			}
		}

		return index;
	}

	public bool RemoveItem(int index) {
		var result = false;

		if (Items [index] != null) {
			Items [index] = null;
		}

		return result;
	}
}
