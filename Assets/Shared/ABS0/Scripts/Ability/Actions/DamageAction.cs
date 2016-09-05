using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class DamageAction : AbilityAction {

	float value;
	float delay;

	public DamageAction(float value) {
		this.value = value;
	}

	protected override void Start ()
	{
		for (int i = 0; i < mTargets.Count; i++) {
			mTargets[i].Hit (mTargets[i], value);
		}
		status = AbilityActionStatus.Success;
	}
}
