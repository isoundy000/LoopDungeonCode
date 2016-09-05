using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UniRx;

public enum AbilityActionStatus {
	Failure = 0,
	Success = 1,
	Running = 2,
	Resting = 3,
	Error   = 4
}

[Serializable]
public abstract class AbilityAction {

	public float MaxRange = float.PositiveInfinity;

	protected CharacterProperty mOwner;
	protected IList<CharacterProperty> mTargets;

	int mAbilityId;

	public AbilityActionStatus status = AbilityActionStatus.Resting;

	Subject<int> mActionSubject;

	public IObservable<int> AsObservable()
	{
		return mActionSubject;
	}

	protected void EndAction(AbilityActionStatus value) {
		status = value;
		if (status == AbilityActionStatus.Success) {
			mActionSubject.OnNext (mAbilityId);
			mActionSubject.OnCompleted ();
		}
	}

	public AbilityAction() {
		mTargets = new List<CharacterProperty> ();
		mActionSubject = new Subject<int> ();
	}

	public void SetOwner(CharacterProperty value, int abilityId) {
		mOwner = value;
		mAbilityId = abilityId;
	}

	public void SetTarget(CharacterProperty value) {
		mTargets.Clear ();
		mTargets.Add (value);
	}

	public void SetTarget(IList<CharacterProperty> value) {
		mTargets.Clear ();
		mTargets = value;
	}

	public void Execute() {
		if (status == AbilityActionStatus.Resting) {
			Start ();
		} else if (status == AbilityActionStatus.Running) {
			Update ();
		}
	}

	public IObservable<int> StartAction() {
		return mActionSubject;
	}

	public void Init() {
		status = AbilityActionStatus.Resting;
	}

	protected virtual void Start() {
		status = AbilityActionStatus.Success;
	}

	protected virtual void Update() {
		status = AbilityActionStatus.Success;
	}

	public int AbilityId {
		get {
			return mAbilityId;
		}
	}


	#region IObservable implementation

	public IDisposable Subscribe (IObserver<AbilityAction> observer)
	{
		throw new NotImplementedException ();
	}

	#endregion
}
