using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class BurningEffect : Effect {

	float mDPS;
	GameObject Prefab;

	GameObject effectInstance;

	int damageTime = -1;

	public BurningEffect(float duration, float DPS) : base(duration) {
		mDPS = DPS;
		Prefab = Resources.Load<GameObject> ("Effects/States/SCFX_Burn_Red");
	}

	protected override void OnActive (CharacterProperty target)
	{
		effectInstance = GameObject.Instantiate (Prefab, target.transform.position, target.transform.rotation) as GameObject;
		effectInstance.transform.SetParent (target.transform);
		effectInstance.transform.localPosition = Prefab.transform.position;
	}

	protected override void OnTimeout (CharacterProperty target)
	{
		GameObject.Destroy (effectInstance, Duration);
	}

	protected override void Execute (CharacterProperty target)
	{
		int t = Mathf.FloorToInt (LivedTime);
		if (t != damageTime) {
			target.Hit (target, mDPS);
			damageTime = t;
		}
	}

	public override object Clone ()
	{
		BurningEffect instance = (BurningEffect)base.Clone();
		return instance;
	}
}
