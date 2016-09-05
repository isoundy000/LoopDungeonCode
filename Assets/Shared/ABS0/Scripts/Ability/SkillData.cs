using UnityEngine;
using System.Collections;
using System;

public enum AbilityType {
	BUFF,
	DEBUFF,
	HOT,
	DOT,
	HEAL,
	DAMAGE
}

[Serializable]
public class SkillData : ScriptableObject {

	public int Id;
	public string Name;
	public string Icon;

	public AbilityType Type;
	public float[] Values;

}
