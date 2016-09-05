using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class AreaOfEffectAction : AbilityAction {
	static readonly string Prefix = "Effects/";

	string mName;
	float mRadius;

	public AreaOfEffectAction(float radius, string name) {
		mRadius = radius;
		mName = name;
	}

	protected override void Start ()
	{
		Vector3 position = mTargets [0].gameObject.transform.position;
		GameObject Prefab = Resources.Load<GameObject> (Prefix + mName);
		GameObject.Instantiate (Prefab, position, Quaternion.identity);
		status = AbilityActionStatus.Success;
	}
}
