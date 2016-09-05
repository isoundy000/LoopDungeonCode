using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Character : Entity {

	public string Name;

	public float HP = 100;
	public float MP = 100;

	public float MaxHP = 100;
	public float MaxMP = 100;

	public float AttackValue;
	public float DefenceValue;

	public float HitRate = 1.0f;
	public float DodgeRate = 0.0f;

    public float CriticalRate = 0.0f;

    public float WalkSpeed = 200;
	public float AttackSpeed = 1;
	public float AttackRange = 0.5f;

    public float ViewDistance = 15;
    public float FieldOfViewAngle = 360;

    public int[] Skills;
}
