using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class StunEffect : Effect {
	GameObject effectInstance;

	public StunEffect(float duration) : base(duration) {
		
	}

	protected override void OnActive (CharacterProperty target)
	{
		GameObject Prefab = Resources.Load<GameObject> ("Effects/States/SCFX_Stun");
		effectInstance = GameObject.Instantiate (Prefab, target.transform.position, Quaternion.identity) as GameObject;
		effectInstance.transform.SetParent (target.transform);
	}

	protected override void OnTimeout (CharacterProperty target)
	{
		GameObject.Destroy (effectInstance, Duration);
	}

	public override object Clone ()
	{
		StunEffect instance = (StunEffect)base.Clone();
		return instance;
	}
}
