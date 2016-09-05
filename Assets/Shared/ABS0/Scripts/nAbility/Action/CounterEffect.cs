using UnityEngine;
using System.Collections;
using UniRx;
using System;

public class CounterEffect : MonoBehaviour
{
    public float Time = 10;
    public int AttackID = 3;
    public string TriggerName = "AttackTrigger";

    CharacterProperty mCharacterProperty;
    AnimatorController mAnimatorController;
    Animator mAnimator;

    // Use this for initialization
    void Start()
    {
        mCharacterProperty = GetComponent<CharacterProperty>();
        mAnimator = GetComponent<Animator>();
        mAnimatorController = GetComponent<AnimatorController>();

        if (mCharacterProperty != null &&
            mAnimator != null &&
            mAnimatorController != null
            )
        {
            mAnimator.SetInteger("Attack", AttackID);
            mAnimator.SetTrigger(TriggerName);

            mAnimatorController.OnHitStartAsObservable
                .Subscribe(info =>
                {
                    mCharacterProperty.BeforeDamageTakeAsObservable
                    .Timeout(TimeSpan.FromSeconds(Time))
                    .Subscribe(damage =>
                    {
                        damage.ignore = true;
                        mAnimator.SetBool("CounterAttackResult", true);
                        mAnimator.SetTrigger(TriggerName);
                        mAnimator.SetInteger("Attack", 0);
                        Destroy(this);
                    },
                    ex => {
                        mAnimator.SetBool("CounterAttackResult", false);
                        mAnimator.SetTrigger(TriggerName);
                        mAnimator.SetInteger("Attack", 0);
                        Destroy(this);
                    });

                });


        } else
        {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
