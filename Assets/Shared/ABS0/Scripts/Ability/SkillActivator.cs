using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using System;

public delegate void AnimationKeyEvent(string name);

public class SkillActivator : MonoBehaviour {

	public event AnimationKeyEvent OnAnimationKey;

	Dictionary<int, Ability> mSkills;
	Queue<Ability> mActivatedSkills;

	//Components
	Animator mAnimator;
	CharacterProperty mCharacterProperty;

    SingleAssignmentDisposable mAttackColliderDisposable;

    AnimatorController mActionController;

    TargetingController mTargetingController;

    Ability mCastingAbility;
    CharacterProperty mTarget;

	void InitSkills() {
        mSkills.Add(0, SkillDB.CreateAbility(mCharacterProperty, 0));
        mSkills.Add(1, SkillDB.CreateAbility (mCharacterProperty, 1));
        mSkills.Add(2, SkillDB.CreateAbility(mCharacterProperty, 2));
        mSkills.Add(3, SkillDB.CreateAbility(mCharacterProperty, 3));
        mSkills.Add(4, SkillDB.CreateAbility(mCharacterProperty, 4));
        mSkills.Add(5, SkillDB.CreateAbility(mCharacterProperty, 5));
    }

	// Use this for initialization
	void Start () {
        mAnimator = GetComponent<Animator> ();
		mCharacterProperty = GetComponent<CharacterProperty> ();
        mTargetingController = GetComponent<TargetingController>();

        mSkills = new Dictionary<int, Ability> ();
		mActivatedSkills = new Queue<Ability> ();

		InitSkills ();

        mAttackColliderDisposable = new SingleAssignmentDisposable();
        mActionController = GetComponent<AnimatorController>();

        mActionController.OnHitStartAsObservable.Subscribe(info =>
        {
            if (info.ability == -1 || mCastingAbility == null)
            {
                return;
            }

            AbilityActionStatus status = mCastingAbility.Perform(mTarget);
            if (status == AbilityActionStatus.Running)
            {
                mActivatedSkills.Enqueue(mCastingAbility);
            }
        });

        mActionController.OnHitEndAsObservable.Subscribe(info =>
        {
            mCastingAbility = null;
            mTarget = null;
        });
    }
	
	// Update is called once per frame
	void Update () {
		if (mActivatedSkills.Count > 0) {
			Ability skill = mActivatedSkills.Peek ();
			AbilityActionStatus status = skill.Update ();
			if (status == AbilityActionStatus.Running) {
				return;
			} else if (status == AbilityActionStatus.Success) {
				mActivatedSkills.Dequeue ();
			} else {
				mActivatedSkills.Clear ();
			}
		}

	}

    public bool CanCastSkill(int skillID, GameObject target)
    {
        CharacterProperty t = target.GetComponent<CharacterProperty>();

        if(t == null)
        {
            return false;
        }

        return CanCastSkill(skillID, t);
    }

    public bool CanCastSkill(int skillID, CharacterProperty target)
    {
       return CanCastSkill(skillID, mCharacterProperty, target);
    }

    public bool CanCastSkill(int skillID, CharacterProperty caster, CharacterProperty target) {
		bool canCast = mSkills.ContainsKey (skillID);
		canCast = canCast && mSkills [skillID].CanCastOn(target);
		if (canCast && target != null && caster.IsAlive && target.IsAlive) {
			return true;
		}
		return false;
	}

    //public Ability CastSkillOnInstant(int skillID, GameObject target)
    //{
    //    Ability ability = null;
    //    if (target == null)
    //    {
    //        return ability;
    //    }
    //    CharacterProperty targetProperty = target.GetComponent<CharacterProperty>();

    //    if (!casting && CanCastSkill(skillID, mCharacterProperty, targetProperty))
    //    {
    //        ability = mSkills[skillID];
    //        AbilityActionStatus status = ability.Perform(targetProperty);
    //        if (status == AbilityActionStatus.Running)
    //        {
    //            mActivatedSkills.Enqueue(ability);
    //        }
    //    }

    //    return ability;
    //}

    public Ability CastSkill(int skillID, GameObject target) {
        if (target == null)
        {
            return null;
        }

        mTarget = target.GetComponent<CharacterProperty>();

        if (mCastingAbility == null && CanCastSkill (skillID, mCharacterProperty, mTarget)) {
            mCastingAbility = mSkills [skillID];

		}
		return mCastingAbility;
	}

    public Ability CastSkillWithAction(int skillID, GameObject target, int actionID, string triggerName)
    {
        if (target == null)
        {
            return null;
        }

        mTarget = target.GetComponent<CharacterProperty>();

        if (mCastingAbility == null && CanCastSkill(skillID, mCharacterProperty, mTarget))
        {
            mCastingAbility = mSkills[skillID];
            mAnimator.SetInteger("Attack", actionID);
            mAnimator.SetTrigger(triggerName);

        }
        return mCastingAbility;
    }

    public Dictionary<int, Ability> Skills {
		get {
			return mSkills;
		}
	}

	public void AnimationKey(string name) {
		if (OnAnimationKey != null) {
			OnAnimationKey (name);
		}
	}
}
