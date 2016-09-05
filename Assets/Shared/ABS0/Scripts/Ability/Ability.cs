using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UniRx;

[Serializable]
public class Ability {

	public int Id;
	public string Name;
	public string Icon;
	public float CoolDown;
	public float ChargingTime;
	public AbilityType Type;
	public string Description;
	public float MaxRange = float.PositiveInfinity;

	float mCost;

	List<AbilityAction> mActions;
	int currentIndex;

	CharacterProperty mOwner;
	CharacterProperty mTarget;
	float mLastTime;

	Subject<Ability> onExit;

	public IObservable<Ability> OnExitAsObservable()
	{
		return onExit ?? (onExit = new Subject<Ability>());
	}

	public void SetOwner(CharacterProperty value) {
		mOwner = value;
	}

	public Ability(CharacterProperty owner, int id, string name, float coolDown, float cost) {
		mOwner = owner;
		Id = id;
		Name = name;
		CoolDown = coolDown;
		mCost = cost;
		mActions = new List<AbilityAction> ();

	}

	public Ability AddAction(AbilityAction action) {
		action.SetOwner(mOwner, Id);
		mActions.Add (action);
		MaxRange = Mathf.Min (MaxRange, action.MaxRange);
		return this;
	}

	public bool CanCastOn(CharacterProperty target) {
		if (!mOwner.HasEnoughMP (mCost) || (CoodDownLeft > 0)) {
			return false;
		}

//		if (Type == SkillType.HEAL && target.gameObject.layer != mOwner.gameObject.layer) {
//			return false;
//		}

		Transform ProjectileCastPoint = mOwner.transform.FindChild ("ProjectileCastPoint");
		Transform CastPoint = (ProjectileCastPoint == null) ? mOwner.transform : ProjectileCastPoint;

		float distance = Vector3.Distance (CastPoint.position, target.transform.position);
		if (distance > MaxRange) {
			return false;
		}

		return true;
	}

	public AbilityActionStatus Perform(CharacterProperty target) {
		mTarget = target;

        mOwner.ReduceMP(mCost);

		if (CoodDownLeft <= 0) {
			mLastTime = Time.time;

			for (int i = 0; i < mActions.Count; i++) {
				mActions [i].Init ();
			}

			currentIndex = 0;


			for(currentIndex = 0; currentIndex < mActions.Count; currentIndex++) {
				AbilityAction action = mActions [currentIndex];
				action.SetTarget (mTarget);
				action.Execute ();
				AbilityActionStatus status = action.status;
				if (status != AbilityActionStatus.Success) {
					return status;
				}
			}
		}

		return AbilityActionStatus.Success;
	}

	public float CoodDownLeft {
		get {
			return CoolDown - (Time.time - mLastTime);
		}
	}

	public AbilityActionStatus Update() {
		if (currentIndex < mActions.Count) {
			for (; currentIndex < mActions.Count; currentIndex++) {
				AbilityAction action = mActions [currentIndex];
				action.SetTarget (mTarget);
				action.Execute ();
				AbilityActionStatus status = action.status;
				if (status != AbilityActionStatus.Success) {
					return status;
				}
			}
		}

		if (onExit != null) {
			onExit.OnNext (this);
		}

		return AbilityActionStatus.Success;

	}
}
