using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Item : Entity {

	public string Name;
	public string Description;
	public string Icon;

	public int Price;
	public int Level;
	public int LeftTime;
	public int Count;
}
