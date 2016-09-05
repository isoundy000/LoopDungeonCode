using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class AnimationAction : AbilityAction {

	string mTriggerName;
	string mWaitForKey;

	public AnimationAction(string triggerName, string waitForKey) {
		mTriggerName = triggerName;
		mWaitForKey = waitForKey;
	}

	protected override void Start ()
	{
		Animator animator = mOwner.GetComponent<Animator> ();
		animator.SetTrigger ("Attack");
		animator.SetInteger ("AttackVariation", 3);
		SkillActivator skillActivator = mOwner.GetComponent<SkillActivator> ();

		AnimationKeyEvent handler = null;

		handler = (string key) => {
			if(key == mWaitForKey) {
				skillActivator.OnAnimationKey -= handler;
				status = AbilityActionStatus.Success;
			}
		};

		skillActivator.OnAnimationKey += handler;

		status = AbilityActionStatus.Running;
	}

	protected override void Update ()
	{
		status = AbilityActionStatus.Running;
	}
}
