using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class SlowEffect : Effect {
	GameObject effectInstance;
	public float mSlowRatio;

	public SlowEffect(float duration, float slowRatio) : base(duration) {
		mSlowRatio = slowRatio;
	}

	protected override void OnActive (CharacterProperty target)
	{
		target.WalkSpeed = target.WalkSpeed * mSlowRatio;

		GameObject Prefab = Resources.Load<GameObject> ("Effects/States/SCFX_Slow");
		effectInstance = GameObject.Instantiate (Prefab, target.transform.position, target.transform.rotation) as GameObject;
		effectInstance.transform.SetParent (target.transform);
		effectInstance.transform.localPosition = Prefab.transform.position;


	}

	protected override void OnTimeout (CharacterProperty target)
	{
		target.ResetWalkSpeed ();
		GameObject.Destroy (effectInstance);
	}

	public override object Clone ()
	{
		SlowEffect instance = (SlowEffect)base.Clone();
		return instance;
	}
}
