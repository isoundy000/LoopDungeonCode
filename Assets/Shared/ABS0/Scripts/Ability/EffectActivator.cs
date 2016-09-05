using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectActivator : MonoBehaviour {

	IList<Effect> mEffects;

	CharacterProperty mCharacterProperty;

	// Use this for initialization
	void Start () {
		mEffects = new List<Effect> ();
		mCharacterProperty = GetComponent<CharacterProperty> ();
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = mEffects.Count - 1; i >= 0; i--)
		{
			Effect effect = mEffects [i];
			effect.Update (mCharacterProperty);
			if (effect.Finished)
				mEffects.RemoveAt(i);
		}
	}

	public void AddEffect(Effect effect) {
		Effect hasEffect = null;
		foreach (Effect item in mEffects) {
			if (item.AbilityId == effect.AbilityId) {
				hasEffect = item;
			}
		}
		if (hasEffect == null) {
			mEffects.Add (effect);
			effect.Update (mCharacterProperty);
		} else {
			hasEffect.Reset ();
		}
	}

	public bool HasEffectOfAbility(int id) {
		foreach (Effect effect in mEffects) {
			if (effect.AbilityId == id) {
				return true;
			}
		}
		return false;
	}
}
