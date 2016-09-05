using UnityEngine;
using System.Collections;

public enum EntityType 
{
	NONE,
	PLAYER,
	NPC,
	MOB,
	EQUIPMENT,
	ITEM
}

public class Entity : ScriptableObject {
	public int EntityId;
	public EntityType Type;
}
