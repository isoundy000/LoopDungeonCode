using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class EffectAction : AbilityAction {

	IList<Effect> mEffects;

	public EffectAction(Effect effect) {
		mEffects = new List<Effect> ();
		mEffects.Add (effect);
	}

	public EffectAction(Effect[] effects) {
		mEffects = new List<Effect> ();
		foreach (Effect effect in effects) {
			mEffects.Add (effect);
		}
	}

	protected override void Start ()
	{
		foreach (Effect effect in mEffects) {
			Effect cloned = effect.Clone () as Effect;
			cloned.AbilityId = AbilityId;
			foreach (CharacterProperty target in mTargets) {
                if (target == null)
                    break;
				EffectActivator effectActivator = target.GetComponent<EffectActivator> ();
				if (effectActivator != null) {
					effectActivator.AddEffect (cloned as Effect);
				}
			}
		}
		status = AbilityActionStatus.Success;
	}
}
