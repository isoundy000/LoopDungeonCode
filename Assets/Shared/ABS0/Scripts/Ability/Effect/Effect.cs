using UnityEngine;
using System.Collections;
using System;

public enum EffectState {
	INACTIVE,
	ACTIVE,
	TIMEOUT,
	CANCELED,
	FINISHED
}

[Serializable]
public abstract class Effect {
	int mAbilityId;

	public virtual object Clone () {
		Effect instance = (Effect)this.MemberwiseClone();
		return instance;
	}

	public float mDuration;
	float mStartTime;

	protected EffectState mState = EffectState.INACTIVE;

	public Effect(float duration) {
		mDuration = duration;
	}

	public float Duration {
		get {
			return mDuration;
		}
	}

	virtual protected void OnActive(CharacterProperty target) {
		
	}

	virtual protected void OnTimeout(CharacterProperty target) {
	}

	virtual protected void OnCanceled(CharacterProperty target) {
	}

	protected void CancelEffect() {
		mState = EffectState.CANCELED;
	}

	public void Update(CharacterProperty target) {
		if (mState == EffectState.INACTIVE) {
			mState = EffectState.ACTIVE;
			mStartTime = Time.time;
			OnActive (target);
		}

		if (mState == EffectState.ACTIVE) {
			
			if ((Time.time - mStartTime) <= mDuration) {
				Execute (target);
			} else {
				mState = EffectState.TIMEOUT;
			}
		}

		if(mState == EffectState.TIMEOUT) {
			OnTimeout (target);
			mState = EffectState.FINISHED;
		}

		if (mState == EffectState.CANCELED) {
			OnCanceled (target);
			mState = EffectState.FINISHED;
		}

	}

	virtual protected void Execute(CharacterProperty target) {
		
	}

	public EffectState State {
		get {
			return mState;
		}
	}

	public bool Finished {
		get {
			return mState == EffectState.FINISHED;
		}
	}

	public int AbilityId {
		get {
			return mAbilityId;
		}
		set {
			mAbilityId = value;
		}
	}

	public bool Equals(Effect effect) {
		return mAbilityId == effect.mAbilityId;
	}

	public void Reset() {
		mStartTime = Time.time;
	}

	public float LivedTime { 
		get {
			return Time.time - mStartTime;
		}
	}
}
