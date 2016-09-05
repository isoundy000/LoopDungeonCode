using UnityEngine;
using System.Collections;
using System;
using UniRx;

[Serializable]
public class DelayAction : AbilityAction {

	float delay;

	public DelayAction(float value) {
		this.delay = value;
	}

	protected override void Start ()
	{
		Observable.Timer (TimeSpan.FromSeconds (delay)).Subscribe (_ => {
			status = AbilityActionStatus.Success;
		});
		status = AbilityActionStatus.Running;
	}

	protected override void Update ()
	{
		if (status != AbilityActionStatus.Success) {
			status = AbilityActionStatus.Running;
		}
	}
}
