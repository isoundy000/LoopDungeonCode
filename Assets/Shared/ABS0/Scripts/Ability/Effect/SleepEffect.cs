using UnityEngine;
using System.Collections;
using System;
using UniRx;

[Serializable]
public class SleepEffect : Effect {

	GameObject effectInstance;

	public SleepEffect(float duration) : base(duration) {

	}

	protected override void OnActive (CharacterProperty target)
	{
		GameObject Prefab = Resources.Load<GameObject> ("Effects/States/SCFX_Sleep");
		effectInstance = GameObject.Instantiate (Prefab, target.transform.position, target.transform.rotation) as GameObject;
		effectInstance.transform.SetParent (target.transform);
		effectInstance.transform.localPosition = Prefab.transform.position;

        target.OnDamageTakeAsObservable.Subscribe(_ =>
       {
           CancelEffect();
       });

		GameObject.Destroy (effectInstance, Duration);
	}

	protected override void OnTimeout (CharacterProperty target)
	{
		GameObject.Destroy (effectInstance);
	}

	protected override void OnCanceled (CharacterProperty target)
	{
		GameObject.Destroy (effectInstance);
	}

	public override object Clone ()
	{
		SleepEffect instance = (SleepEffect)base.Clone();
		return instance;
	}
}
